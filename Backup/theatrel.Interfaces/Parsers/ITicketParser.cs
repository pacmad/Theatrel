﻿using AngleSharp.Dom;

namespace theatrel.Interfaces.Parsers
{
    public interface ITicketParser : IDIRegistrable
    {
        ITicket Parse(IElement ticket);
    }
}
