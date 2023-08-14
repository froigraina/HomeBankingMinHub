using HomeBanking.Controllers;
using HomeBanking.Models;
using HomeBanking.Models.DTOs;
using HomeBanking.Repositories;
using HomeBankingMinHub.Models;
using HomeBankingMinHub.Models.DTOS;
using HomeBankingMinHub.Models.DTOSs;
using HomeBankingMinHub.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;

namespace HomeBankingMinHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private IClientRepository _clientRepository;
        private AccountsController _accountsController;
        private CardsController _cardsController;


        public ClientsController(IClientRepository clientRepository, AccountsController accountsController, CardsController cardsController)
        {
            _clientRepository = clientRepository;
            _accountsController = accountsController;
            _cardsController = cardsController;


        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var clients = _clientRepository.GetAllClients();
                var clientsDTO = new List<ClientDTO>();
                foreach (Client client in clients)
                {
                    var newClientDTO = new ClientDTO
                    {
                        Id = client.Id,
                        Email = client.Email,
                        FirstName = client.FirstName,
                        LastName = client.LastName,
                        Accounts = client.Accounts.Select(ac => new AccountDTO
                        {
                            Id = ac.Id,
                            Balance = ac.Balance,
                            CreationDate = ac.CreationDate,
                            Number = ac.Number
                        }).ToList(),
                        Credits = client.ClientLoans.Select(cl => new ClientLoanDTO
                        {
                            Id = cl.Id,
                            LoanId = cl.LoanId,
                            Name = cl.Loan.Name,
                            Amount = cl.Amount,
                            Payments = int.Parse(cl.Payments)
                        }).ToList(),
                        Cards = client.Cards.Select(c => new CardDTO
                        {
                            Id = c.Id,
                            CardHolder = c.CardHolder,
                            Color = c.Color,
                            Cvv = c.Cvv,
                            FromDate = c.FromDate,
                            Number = c.Number,
                            ThruDate = c.ThruDate,
                            Type = c.Type
                        }).ToList(),
                    };
                    clientsDTO.Add(newClientDTO);
                }
                return Ok(clientsDTO);

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
                var client = _clientRepository.FindById(id);
                if (client == null)
                {
                    return NotFound();
                }

                var clientDTO = new ClientDTO
                {
                    Id = client.Id,
                    Email = client.Email,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    Accounts = client.Accounts.Select(ac => new AccountDTO
                    {
                        Id = ac.Id,
                        Balance = ac.Balance,
                        CreationDate = ac.CreationDate,
                        Number = ac.Number
                    }).ToList(),
                    Credits = client.ClientLoans.Select(cl => new ClientLoanDTO
                    {
                        Id = cl.Id,
                        LoanId = cl.LoanId,
                        Name = cl.Loan.Name,
                        Amount = cl.Amount,
                        Payments = int.Parse(cl.Payments)
                    }).ToList(),
                    Cards = client.Cards.Select(c => new CardDTO
                    {
                        Id = c.Id,
                        CardHolder = c.CardHolder,
                        Color = c.Color,
                        Cvv = c.Cvv,
                        FromDate = c.FromDate,
                        Number = c.Number,
                        ThruDate = c.ThruDate,
                        Type = c.Type
                    }).ToList(),
                };
                return Ok(clientDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("current")]
        public IActionResult GetCurrent()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return Forbid();
                }
                Client client = _clientRepository.FindByEmail(email);
                if (client == null)
                {
                    return Forbid();
                }
                var clientDTO = new ClientDTO
                {
                    Id = client.Id,
                    Email = client.Email,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    Accounts = client.Accounts.Select(ac => new AccountDTO
                    {
                        Id = ac.Id,
                        Balance = ac.Balance,
                        CreationDate = ac.CreationDate,
                        Number = ac.Number
                    }).ToList(),
                    Credits = client.ClientLoans.Select(cl => new ClientLoanDTO
                    {
                        Id = cl.Id,
                        LoanId = cl.LoanId,
                        Name = cl.Loan.Name,
                        Amount = cl.Amount,
                        Payments = int.Parse(cl.Payments)
                    }).ToList(),
                    Cards = client.Cards.Select(c => new CardDTO
                    {
                        Id = c.Id,
                        CardHolder = c.CardHolder,
                        Color = c.Color,
                        Cvv = c.Cvv,
                        FromDate = c.FromDate,
                        Number = c.Number,
                        ThruDate = c.ThruDate,
                        Type = c.Type
                    }).ToList()


                };
                return Ok(clientDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Client client)

        {
            Random rnd = new Random();

            try
            {
                if (String.IsNullOrEmpty(client.Email) || String.IsNullOrEmpty(client.Password) || String.IsNullOrEmpty(client.FirstName) || String.IsNullOrEmpty(client.LastName))
                    return StatusCode(403, "datos invalidos");

                Client user = _clientRepository.FindByEmail(client.Email);

                if (user != null)
                {
                    return StatusCode(403, "Email esta en uso");

                }
                Client newClient = new Client
                {
                    Email = client.Email,
                    Password = client.Password,
                    FirstName = client.FirstName,
                    LastName = client.LastName
                };
                _clientRepository.Save(newClient);

                //do
                //{
                //    newAccountNumber = "CTA" + new Random().Next(100000, 999999).ToString();
                //    account = _accountRepository.FindByNumber(newAccountNumber);
                //} while (account != null);

                AccountDTO newAccountDTO = _accountsController.Post(newClient.Id);
                if (newAccountDTO == null)
                {
                    return StatusCode(500, "account not created at accountsController");
                }
                return Created("", newClient);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("current/accounts")]

        public IActionResult Post()
        {
            IEnumerable<AccountDTO> accounts;

            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == String.Empty)
                {
                    return Forbid();
                }

                Client client = _clientRepository.FindByEmail(email);
                if (client == null)
                {
                    return Forbid("No existe el cliente");
                }

                accounts = client.Accounts.Select(ac => new AccountDTO
                {
                    Id = ac.Id,
                    Balance = ac.Balance,
                    CreationDate = ac.CreationDate,
                    Number = ac.Number
                }).ToList();

                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("current/accounts")]
        public IActionResult PostAccount()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : String.Empty;
                if (email == String.Empty)
                {
                    return Forbid();
                }

                Client client = _clientRepository.FindByEmail(email);
                if (client == null)
                {
                    return Forbid("not existing client");
                }

                //validate max accounts
                if (client.Accounts.Count >= 3)
                {
                    return Forbid("$\"Over max 3 cards permitted\"");
                }

                var account = _accountsController.Post(client.Id);

                if (account == null)
                {
                    return StatusCode(500, "account not created at accountsController");
                }

                return Created("", account);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("current/cards")]
        public IActionResult GetCards()
        {
            IEnumerable<CardDTO> cards;

            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : String.Empty;
                if (email == String.Empty)
                {
                    return Forbid();
                }

                Client client = _clientRepository.FindByEmail(email);

                if (client == null)
                {
                    return Forbid();
                }

                cards = client.Cards.Select(ac => new CardDTO
                {
                    Id = ac.Id,
                    CardHolder = ac.CardHolder,
                    Color = ac.Color,
                    Cvv = ac.Cvv,
                    FromDate = ac.FromDate,
                    Number = ac.Number,
                    ThruDate = ac.ThruDate,
                    Type = ac.Type

                }).ToList();


                return Ok(cards);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("current/cards")]
        public IActionResult PostCard([FromBody] Card card)
        {
            string cardHolder = String.Empty;
            string newCardType = String.Empty;
            int cardsAmount = 0;

            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : String.Empty;
                if (email == String.Empty)
                {
                    return Forbid("Do not have authorization");
                }

                Client client = _clientRepository.FindByEmail(email);
                if (client == null)
                {
                    return Forbid("Not existing client");
                }

                //validate data
                if (String.IsNullOrEmpty(card.Type) ||
                    String.IsNullOrEmpty(card.Color) ||
                    !Card.IsCardType(card.Type) ||
                    !Card.IsCardColor(card.Color))
                {
                    return StatusCode(403, "Invalid Data");
                }

                foreach (Card cardAux in client.Cards)
                {
                    if (cardAux.Type == card.Type)
                    {
                        cardsAmount++;
                    }
                }

                if (cardsAmount >= 3)
                {
                    return Forbid($"Over max card type {card.Type} permitted");
                }

                cardHolder = $"{client.FirstName} {client.LastName}";

                CardDTO newCardDTO = _cardsController.Post(cardHolder, client.Id, card);
                if (newCardDTO == null)
                {
                    return StatusCode(403, "Card not created at cardsController");
                }

                return Created("", newCardDTO);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }

}

