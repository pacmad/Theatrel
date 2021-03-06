﻿using System;
using System.Threading;
using theatrel.Interfaces;

namespace theatrel.TLBot.Interfaces
{
    public interface ITLBotService : IDISingleton
    {
        event EventHandler<ITLMessage> OnMessage;

        void SendMessageAsync(long chatId, string message);
        void Start(CancellationToken cancellationToken);
        void Stop();
    }
}
