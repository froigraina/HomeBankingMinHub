using HomeBankingMinHub.Models;
using HomeBankingMinHub.Models.DTOS;
using HomeBankingMinHub.Models.Enum;
using HomeBankingMinHub.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System;
using System.Linq;

namespace HomeBankingMinHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private IClientRepository _clientRepository;
        private IAccountRepository _accountRepository;
        private ILoanRepository _loanRepository;
        private IClientLoanRepository _clientLoanRepository;
        private ITransactionRepository _transactionRepository;

        public LoansController(IClientRepository clientRepository, IAccountRepository accountRepository, ILoanRepository loanRepository, IClientLoanRepository clientLoanRepository, ITransactionRepository transactionRepository)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _loanRepository = loanRepository;
            _clientLoanRepository = clientLoanRepository;
            _transactionRepository = transactionRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var loans = _loanRepository.GetAll();

                var loanDTOs = loans.Select(loan => new LoanDTO
                {
                    Id = loan.Id,
                    Name = loan.Name,
                    MaxAmount = loan.MaxAmount,
                    Payments = loan.Payments
                }).ToList();
                return Ok(loanDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost]

        public IActionResult Post([FromBody] LoanAplicattionDTO loanAplicattionDTO)
        {
            try
            {
                //Verifications

                // Autenticacion
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : String.Empty;
                if (email == String.Empty)
                {
                    return Forbid("Don`t have authorization");
                }

                Client client = _clientRepository.FindByEmail(email);
                if (client == null)
                {
                    return Forbid("");
                }
                //Si el prestamo existe
                var loan = _loanRepository.FindById(loanAplicattionDTO.LoanId);
                if (loan == null)
                {
                    return Forbid("El prestamo no existe");
                }

                //Si el monto es valido
                if (loanAplicattionDTO.Amount <= 0 || loanAplicattionDTO.Amount > loan.MaxAmount)
                {
                    return Forbid("El monto del prestamo es invalido");
                }

                //Si los pagos no estan vacios

                if (string.IsNullOrEmpty(loanAplicattionDTO.Payments))
                {
                    return Forbid("Pagos no especificados");
                }

                //Si existe la cuenta de destino

                var account = _accountRepository.FindByNumber(loanAplicattionDTO.ToAccountNumber);
                if (account == null)
                {
                    return Forbid("La cuenta de destino no existe");
                }

                //Si la cuenta de destino pertenece al cliente autenticado

                if(account.ClientId != client.Id)
                {
                    return Forbid("El cliente no es el titular de la cuenta de destino");
                }

                //Calcular el monto a acreditar +20%

                double clientLoanAmount = loanAplicattionDTO.Amount * 1.2;

                //Crear el objeto y guardar

                var clientLoan = new ClientLoan
                {
                    ClientId = client.Id,
                    LoanId = loan.Id,
                    Amount = clientLoanAmount,
                    Payments = loanAplicattionDTO.Payments

                };

                _clientLoanRepository.Save(clientLoan);

                //  Crear transaccion para el cliente

                var transaction = new Transaction
                {
                    Type = TransactionType.CREDIT.ToString(),
                    Amount = clientLoanAmount,
                    Description = $"Prestamo solicitado ({loan.Name})",
                    Date = DateTime.Now,
                    AccountId = account.Id,

                };

                _transactionRepository.Save(transaction);

                //Actualizar el balance de la cuenta y guardarla
                account.Balance += clientLoanAmount;
                _accountRepository.Save(account);

                // Devolver una respuesta exitosa
                return Ok("Préstamo solicitado exitosamente");

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }


}


