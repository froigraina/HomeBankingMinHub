﻿using HomeBankingMinHub.Models;
using HomeBankingMinHub.Models.DTOS;
using HomeBankingMinHub.Models.Enum;
using HomeBankingMinHub.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace HomeBankingMinHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private IClientRepository _clientRepository;
        private IAccountRepository _accountRepository;
        private ITransactionRepository _transactionRepository;

        public TransactionsController(IClientRepository clientRepository, IAccountRepository accountRepository, ITransactionRepository transactionRepository)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
        }

        [HttpPost]

        public IActionResult Post([FromBody] TransferDTO transferDTO)
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return Forbid("Email vacio");
                }
                Client client = _clientRepository.FindByEmail(email);
                if (client == null)
                {
                    return Forbid("No existe el cliente");
                }

                if (transferDTO.FromAccountNumber == string.Empty || transferDTO.ToAccountNumber == string.Empty)
                {
                    return Forbid("Cuenta de origen o cuenta de destino no proporcionada.");
                }
                if (transferDTO.FromAccountNumber == transferDTO.ToAccountNumber)
                {
                    return Forbid("No se permite la transferencia a la misma cuenta.");
                }
                if (transferDTO.Amount == 0 || transferDTO.Description == string.Empty)
                {
                    return Forbid("Monto o descripción no proporcionados.");
                }

                //Looking for the accounts

                Account fromAccount = _accountRepository.FindByNumber(transferDTO.FromAccountNumber);
                if (fromAccount == null)
                {
                    return Forbid("Cuenta de origen no existe");
                }
                //Check the amounts

                if (fromAccount.Balance < transferDTO.Amount)
                {
                    return Forbid("Fondos insuficientes");
                }

                //looking for the destiny account
                Account toAccount = _accountRepository.FindByNumber(transferDTO.ToAccountNumber);
                if (toAccount == null)
                {
                    return Forbid("Cuenta de destino no existe");
                }

                //Create Transactions
                //DEBIT

                _transactionRepository.Save(new Transaction
                {
                    Type = TransactionType.DEBIT.ToString(),
                    Amount = transferDTO.Amount * -1,
                    Description = transferDTO.Description + " " + toAccount.Number,
                    AccountId = fromAccount.Id,
                    Date = DateTime.Now,
                });

                //CREDIT
                _transactionRepository.Save(new Transaction
                {
                    Type = TransactionType.CREDIT.ToString(),
                    Amount = transferDTO.Amount,
                    Description = transferDTO.Description + " " + fromAccount.Number,
                    AccountId = toAccount.Id,
                    Date = DateTime.Now,
                });

                fromAccount.Balance = fromAccount.Balance - transferDTO.Amount;
                //update "from account"
                _accountRepository.Save(fromAccount);

                //update balance
                toAccount.Balance = toAccount.Balance + transferDTO.Amount;
                //update "to account"

                _accountRepository.Save(toAccount);


                return Created("Creado con exito", fromAccount);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

      
    }
}
