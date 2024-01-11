﻿using Money.Domain.Entities;
using Money.Domain.Repositories;

namespace Money.Infrastructure.Repositories;

public class TransactionRepository(MoneyContext context) : ITransactionRepository
{
    private readonly MoneyContext _context = context;

    public async Task Save(Transaction transaction)
    {
        await _context.Transaction.AddAsync(transaction);
        await _context.SaveChangesAsync();
    }
}
