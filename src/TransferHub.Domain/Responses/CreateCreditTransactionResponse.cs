﻿using TransferHub.Domain.Entities;

namespace TransferHub.Domain.Responses;

public class CreateCreditTransactionResponse
{
    public Guid Id { get; set; }
    public string Type { get; set; } = null!;
    public Guid ParticipantId { get; set; } 
    public string Amount { get; set; } = null!;
    public DateTime CreatedAt { get; set; } 

    public CreateCreditTransactionResponse()
    {
    }

    public CreateCreditTransactionResponse(Transaction transaction)
    {
        Id = transaction.Id;
        Type = transaction.Type.ToString();
        ParticipantId = transaction.ParticipantId;
        Amount = transaction.Amount.ToString("0.00");
        CreatedAt = transaction.CreatedAt;
    }
}
