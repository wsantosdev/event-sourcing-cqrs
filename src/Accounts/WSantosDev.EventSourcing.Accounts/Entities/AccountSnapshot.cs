﻿using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Accounts
{
    public record AccountSnapshot(AccountId AccountId, int EntityVersion, IEnumerable<Entry> Entries) : ISnapshot;
}
