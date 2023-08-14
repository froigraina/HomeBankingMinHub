using HomeBanking.Repositories;
using HomeBankingMinHub.Models;
using HomeBankingMinHub.Models.DTOS;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeBankingMinHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private IAccountRepository _accountRepository;

        public AccountsController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpGet]

        public IActionResult Get()
        {
            try
            {
                var accounts = _accountRepository.GetAllAccounts();
                var accountsDTO = new List<AccountDTO>();
                foreach (Account account in accounts)
                {
                    var newAccountDTO = new AccountDTO
                    {
                        Id = account.Id,
                        Number = account.Number,
                        CreationDate = account.CreationDate,
                        Balance = account.Balance,
                        Transactions = account.Transactions.Select(t => new TransactionDTO
                        {
                            Id = t.Id,
                            Type = t.Type,
                            Amount = t.Amount,
                            Description = t.Description,
                            Date = t.Date,
                        }).ToList()
                    };
                    accountsDTO.Add(newAccountDTO);

                }
                return Ok(accountsDTO);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            try
            {
                var account = _accountRepository.FindById(id);
                if (account == null)
                {
                    return NotFound();
                }

                var accountDTO = new AccountDTO
                {
                    Id = account.Id,
                    Number = account.Number,
                    CreationDate = account.CreationDate,
                    Balance = account.Balance,
                    Transactions = account.Transactions.Select(t => new TransactionDTO
                    {
                        Id = t.Id,
                        Type = t.Type,
                        Amount = t.Amount,
                        Description = t.Description,
                        Date = t.Date,
                    }).ToList()
                };
                return Ok(accountDTO);


            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public AccountDTO Post (long clientID)
        {
            Random rnd = new Random();
            Account account;
            string newAccountNumber;
            try
            {
                //look for existing account number
                do
                {
                    newAccountNumber = "CTA-"+ rnd.Next(100000,999999);
                    account = _accountRepository.FindByNumber(newAccountNumber);
                }
                while 
                    (account != null);
                
                Account newAccount = new Account
                {
                    Number = newAccountNumber,
                    CreationDate = DateTime.Now,
                    Balance = 0.0,
                    ClientId = clientID
                };

                _accountRepository.Save(newAccount);
                AccountDTO accountDTO = new AccountDTO
                {
                    Id = newAccount.Id,
                    Number = newAccount.Number,
                    CreationDate = newAccount.CreationDate,
                    Balance = newAccount.Balance,
                };
                return accountDTO;
            }
            catch
            {
                return null;
            }
        }

    }
}
