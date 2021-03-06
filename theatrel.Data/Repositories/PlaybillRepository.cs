﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using theatrel.Common.Enums;
using theatrel.DataAccess.Structures.Entities;
using theatrel.DataAccess.Structures.Interfaces;
using theatrel.Interfaces.Playbill;

namespace theatrel.DataAccess.Repositories
{
    internal class PlaybillRepository : IPlaybillRepository
    {
        private readonly AppDbContext _dbContext;

        public PlaybillRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private int GetPerformanceEntityId(IPerformanceData data) => GetPerformanceEntityId(data, out _, out _);

        private int GetPerformanceEntityId(IPerformanceData data, out int locationId, out int typeId)
        {
            LocationsEntity l = _dbContext.PerformanceLocations.AsNoTracking()
                .FirstOrDefault(x => x.Name == data.Location);
            locationId = l?.Id ?? -1;

            PerformanceTypeEntity t = _dbContext.PerformanceTypes.AsNoTracking().FirstOrDefault(x => x.TypeName == data.Type);
            typeId = t?.Id ?? -1;

            if (l == null || t == null)
                return -1;

            var performanceId = _dbContext.Performances.AsNoTracking()
                .FirstOrDefault(p => p.Location == l && p.Type == t && p.Name == data.Name)?.Id ?? -1;

            return performanceId;
        }

        private PerformanceEntity AddPerformanceEntity(IPerformanceData data, int locationId, int typeId)
        {
            try
            {
                LocationsEntity location = locationId != -1
                    ? _dbContext.PerformanceLocations.FirstOrDefault(l => l.Id == locationId)
                    : new LocationsEntity { Name = data.Location };

                PerformanceTypeEntity type = typeId != -1
                    ? _dbContext.PerformanceTypes.FirstOrDefault(t => t.Id == typeId)
                    : new PerformanceTypeEntity {TypeName = data.Type};

                PerformanceEntity performance = new PerformanceEntity
                {
                    Name = data.Name,
                    Location = location,
                    Type = type,
                };

                _dbContext.Performances.Add(performance);

                return performance;
            }
            catch (Exception e)
            {
                Console.WriteLine($"AddPerformanceEntity DbException: {e.Message}");
                throw;
            }
        }

        public async Task<PlaybillEntity> AddPlaybill(IPerformanceData data)
        {
            try
            {
                int performanceId = GetPerformanceEntityId(data, out int location, out int type);
                PerformanceEntity performance = -1 == performanceId
                    ? AddPerformanceEntity(data, location, type)
                    : _dbContext.Performances
                        .Include(p => p.Location)
                        .Include(p => p.Type)
                        .FirstOrDefault(p => p.Id == performanceId);

                var playBillEntry = new PlaybillEntity
                {
                    Performance = performance,
                    Url = data.Url,
                    When = data.DateTime,
                    Changes = new List<PlaybillChangeEntity>
                    {
                        new PlaybillChangeEntity
                        {
                            LastUpdate = DateTime.Now,
                            MinPrice = data.MinPrice,
                            ReasonOfChanges = (int) ReasonOfChanges.Creation,
                        }
                    }  
                };

                _dbContext.Playbill.Add(playBillEntry);
                _dbContext.Add(playBillEntry.Changes.First());

                await _dbContext.SaveChangesAsync();

                if (performance != null)
                {
                    _dbContext.Entry(performance.Location).State = EntityState.Detached;
                    _dbContext.Entry(performance.Type).State = EntityState.Detached;
                    _dbContext.Entry(performance).State = EntityState.Detached;
                }

                return playBillEntry;
            }
            catch (Exception ex)
            {
                Trace.TraceInformation($"AddPlaybill DbException {ex.Message} InnerException {ex.InnerException?.Message}");
            }

            return null;
        }

        public PlaybillEntity Get(IPerformanceData data)
        {
            try
            {
                var performanceId = GetPerformanceEntityId(data);
                if (-1 == performanceId)
                    return null;

                return _dbContext.Playbill
                    .Include(x => x.Changes)
                    .Where(x => x.When == data.DateTime && x.PerformanceId == performanceId)
                    .AsNoTracking()
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                Trace.TraceInformation($"Get PlaybillEntity DbException {ex.Message} InnerException {ex.InnerException?.Message}");
            }

            return null;
        }

        private Task<PlaybillEntity> GetById(long id)
            => _dbContext.Playbill.AsNoTracking().SingleOrDefaultAsync(u => u.Id == id);

        public async Task<bool> AddChange(PlaybillEntity entity, PlaybillChangeEntity change)
        {
            PlaybillEntity oldValue = await GetById(entity.Id);

            if (oldValue == null)
                return false;

            entity.Changes.Add(change);
            _dbContext.Add(change);

            _dbContext.Entry(entity).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Trace.TraceInformation($"AddChange DbException {ex.Message} InnerException {ex.InnerException?.Message}");
                return false;
            }
        }

        private Task<PlaybillChangeEntity> GetChangeById(long id)
            => _dbContext.PerformanceChanges.AsNoTracking().SingleOrDefaultAsync(u => u.Id == id);

        public async Task<bool> Update(PlaybillChangeEntity entity)
        {
            PlaybillChangeEntity oldValue = await GetChangeById(entity.Id);

            if (oldValue == null)
                return false;

            PlaybillEntity playbillEntity = null;
            try
            {
                playbillEntity = _dbContext.Playbill
                    .Include(p => p.Performance)
                        .ThenInclude(p => p.Type)
                    .Include(p => p.Performance)
                        .ThenInclude(p => p.Location)
                    .Include(p => p.Changes)
                    .FirstOrDefault(p => p.Id == entity.PlaybillEntityId);

                var updatedChange = playbillEntity?.Changes.FirstOrDefault(ch => ch.Id == entity.Id);
                if (updatedChange == null)
                    return false;

                updatedChange.LastUpdate = entity.LastUpdate;

                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Trace.TraceInformation(
                    $"Update Change entity DbException {ex.Message} InnerException {ex.InnerException?.Message}");
                return false;
            }
            finally
            {
                if (playbillEntity != null)
                {
                    _dbContext.Entry(playbillEntity.Performance.Location).State = EntityState.Detached;
                    _dbContext.Entry(playbillEntity.Performance.Type).State = EntityState.Detached;
                    _dbContext.Entry(playbillEntity.Performance).State = EntityState.Detached;
                    foreach (var change in playbillEntity.Changes)
                        _dbContext.Entry(change).State = EntityState.Detached;

                    _dbContext.Entry(playbillEntity).State = EntityState.Detached;
                }
            }
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }
    }
}
