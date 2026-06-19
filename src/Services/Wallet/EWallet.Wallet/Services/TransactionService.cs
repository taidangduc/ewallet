using EWallet.Common.Core;
using EWallet.Common.Exceptions;
using EWallet.Wallet.DTOs;
using EWallet.Wallet.Entities;
using EWallet.Wallet.ExternalServices.Payment;
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

    public async Task<List<TransactionDTO>> GetTransactionsAsync(Guid userId)
    {
        var wallet = await _walletRepository.GetQueryable().FirstOrDefaultAsync(x => x.UserId == userId);

        if (wallet == null)
        {
            throw new NotFoundException("Wallet not found.");
        }

        var transactions = await _transactionRepository.GetQueryable()
            .Where(x => x.WalletId == wallet.Id)
            .OrderByDescending(x => x.CreatedDateTime)
            .Take(5)
            .Select(x => new TransactionDTO
            {
                Id = x.Id,
                WalletId = x.WalletId,
                Type = x.Type,
                Status = x.Status,
                Amount = x.Amount,
                Description = x.Description,
                CreatedDateTime = x.CreatedDateTime
            })
            .ToListAsync();

        return transactions;
    }
    /*
    * This is first version of project, so I will keep the transaction processing logic simple
    * with mock payment gateway and use unit of work pattern to ensure data consistency.
    * If you read this comment, I hope you can await the next version of project.
    */
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

        if (model.Type == TransactionType.Withdraw && wallet.Balance < model.Amount)
        {
            throw new ValidationException("Insufficient balance.");
        }

        var response = await ProcessPaymentAsync(model);

        if (response.Success == false)
        {
            await _transactionRepository.AddAsync(new Entities.Transaction
            {
                Id = Guid.NewGuid(),
                WalletId = wallet.Id,
                Type = model.Type,
                Status = TransactionStatus.Failed,
                Amount = model.Amount,
                Description = response.Message,
                CreatedDateTime = DateTimeOffset.UtcNow
            });

            await _unitOfWork.SaveChangesAsync();

            throw new Exception($"Payment processing: {response.Message}");
        }

        if (model.Type == TransactionType.Deposit)
        {
            wallet.Balance += model.Amount;
        }
        else if (model.Type == TransactionType.Withdraw)
        {
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
            Status = TransactionStatus.Success,
            Amount = model.Amount,
            CreatedDateTime = DateTimeOffset.UtcNow
        };

        using (var _transaction = await _unitOfWork.BeginTransactionAsync())
        {
            try
            {
                await _transactionRepository.AddAsync(transaction);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }

    public Task<PaymentResponse> ProcessPaymentAsync(CreateTransactionModel model)
    {
        if (model.Type == TransactionType.Deposit)
        {
            var charge = new PaymentRequest
            {
                CardId = model.CardId
            };

            return _paymentGateway.ChargeAsync(charge);
        }
        else if (model.Type == TransactionType.Withdraw)
        {
            var payout = new PayoutRequest
            {
                CardId = model.CardId
            };

            return _paymentGateway.PayoutAsync(payout);
        }
        else
        {
            throw new ValidationException("Invalid transaction type.");
        }
    }
}