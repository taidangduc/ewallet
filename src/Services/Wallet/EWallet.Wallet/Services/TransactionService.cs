using EWallet.Common.Core;
using EWallet.Common.Exceptions;
using EWallet.Wallet.DTOs;
using EWallet.Wallet.Entities;
using EWallet.Wallet.ExternalServices.Payment;
using EWallet.Wallet.Models;
using EWallet.Wallet.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EWallet.Wallet.Services;

public class TransactionService : ITransactionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly IPaymentGateway _paymentGateway;

    public TransactionService(
        ITransactionRepository transactionRepository,
        IWalletRepository walletRepository,
        IUnitOfWork unitOfWork,
        IPaymentGateway paymentGateway)
    {
        _unitOfWork = unitOfWork;
        _transactionRepository = transactionRepository;
        _walletRepository = walletRepository;
        _paymentGateway = paymentGateway;
    }

    public async Task<List<TransactionDTO>> GetTransactionsAsync(Guid walletId)
    {
        var transactions = await _transactionRepository.GetQueryable()
            .Where(x => x.WalletId == walletId)
            .Select(x => new TransactionDTO
            {
                Id = x.Id,
                WalletId = x.WalletId,
                Type = x.Type,
                Amount = x.Amount,
                Description = x.Description,
                CreatedDateTime = x.CreatedDateTime
            })
            .ToListAsync();

        return transactions;
    }

    public async Task CreateTransactionAsync(CreateTransactionModel model)
    {
        if (model.Amount <= 0)
        {
            throw new ValidationException("Amount must be greater than zero.");
        }

        var wallet = await _walletRepository.GetQueryable().FirstOrDefaultAsync(x => x.UserId == model.UserId);

        if (wallet == null)
        {
            throw new NotFoundException("Wallet not found.");
        }

        if (model.Type == TransactionType.Deposit)
        {
            var charge = new PaymentRequest
            {
                CardId = model.CardId
            };

            var response = await _paymentGateway.ChargeAsync(charge);

            if (response.Success == false)
            {
                throw new Exception($"Charge failed: {response.Message}");
            }

            wallet.Balance += model.Amount;
        }
        else if (model.Type == TransactionType.Withdraw)
        {
            if (wallet.Balance < model.Amount)
            {
                throw new ValidationException("Insufficient balance.");
            }

            var payout = new PayoutRequest
            {
                CardId = model.CardId
            };

            var response = await _paymentGateway.PayoutAsync(payout);

            if (response.Success == false)
            {
                throw new Exception($"Payout failed: {response.Message}");
            }

            wallet.Balance -= model.Amount;
        }
        else
        {
            throw new ValidationException("Invalid transaction type.");
        }

        var transaction = new Entities.Transaction
        {
            Id = Guid.NewGuid(),
            WalletId = wallet.Id,
            Type = model.Type,
            Amount = model.Amount,
            CreatedDateTime = DateTimeOffset.UtcNow
        };

        using (var _transaction = await _unitOfWork.BeginTransactionAsync())
        {
            await _transactionRepository.AddAsync(transaction);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();
        }
    }
}