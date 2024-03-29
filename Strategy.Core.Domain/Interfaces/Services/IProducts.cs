﻿using Strategy.Core.Domain.Enum;
using Strategy.Core.Domain.Models;

namespace Strategy.Core.Domain.Interfaces.Services
{
    public interface IProducts
    {
        AccountType AccountType { get; }
        Task Save(AccountBase request);
    }
}