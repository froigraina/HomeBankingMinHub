﻿using HomeBankingMinHub.Models.Enum;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;


namespace HomeBankingMinHub.Models
{
    public class Dbinitializer
    {
        public static void Initialize(HomeBankingContext context)
        {
            if (!context.Clients.Any())
            {
                var clients = new Client[]
                {
                    new Client {
                        FirstName = "Francisco",
                        LastName = "Roig",
                        Email = "froigraina@gmail.com",
                        Password = "123456",
                    },
                    new Client {
                        FirstName = "Damian",
                        LastName = "Barrionuevo",
                        Email = "damibarrionuevo@gmail.com",
                        Password = "123456",
                    },
                };
                foreach (var client in clients)
                {
                    context.Clients.Add(client);
                }
            }
            context.SaveChanges();

            if (!context.Accounts.Any())
            {
                var accountRoig = context.Clients.FirstOrDefault(client => client.Email == "froigraina@gmail.com");
                if (accountRoig != null)
                {
                    var accounts = new Account[]
                    {
                        new Account{ClientId= accountRoig.Id, CreationDate = DateTime.Now, Number="CTA001", Balance=5000}
                    };
                    foreach (Account account in accounts)
                    {
                        context.Accounts.Add(account);
                    }
                }
                context.SaveChanges();

            }

            if (!context.Transactions.Any())
            {
                var account1 = context.Accounts.FirstOrDefault(c => c.Number == "CTA001");
                if (account1 != null)
                {
                    var transactions = new Transaction[]
                    {
                        new Transaction {AccountId = account1.Id, Amount = 10000, Date = DateTime.Now.AddHours(-5), Description = "Transferencia Recibida", Type = TransactionType.CREDIT.ToString()},
                        new Transaction {AccountId = account1.Id, Amount = -2000,Date= DateTime.Now.AddHours(-6), Description = "Compra en tienda Mercado Libre", Type = TransactionType.DEBIT.ToString()},
                        new Transaction {AccountId = account1.Id, Amount = -3000, Date = DateTime.Now.AddHours(-7), Description = "Compra en tienda xxxx", Type = TransactionType.DEBIT.ToString()},
                    };
                    foreach (Transaction transaction in transactions)
                    {
                        context.Transactions.Add(transaction);
                    }
                    context.SaveChanges();
                }
            }

            if (!context.Loans.Any())
            {
                var loans = new Loan[]
                {
                    new Loan {Name = "Hipotecario", MaxAmount = 500000, Payments = "12,24,36,48,60"},
                    new Loan {Name = "Personal", MaxAmount = 100000, Payments = "6,12,24"},
                    new Loan {Name = "Automotriz", MaxAmount = 300000, Payments= "6,12,24,36" }
                };
                foreach (Loan loan in loans)
                {
                    context.Loans.Add(loan);
                }
                context.SaveChanges();
                var client1 = context.Clients.FirstOrDefault(c => c.Email == "froigraina@gmail.com");
                if (client1 != null)
                {
                    var loan1 = context.Loans.FirstOrDefault(l => l.Name == "Hipotecario");
                    if (loan1 != null)
                    {
                        var clientLoan1 = new ClientLoan
                        {
                            Amount = 400000,
                            ClientId = client1.Id,
                            LoanId = loan1.Id,
                            Payments = "60"
                        };
                        context.ClientLoans.Add(clientLoan1);
                    }
                    var loan2 = context.Loans.FirstOrDefault(l => l.Name == "Personal");
                    if (loan2 != null)
                    {
                        var clientLoan2 = new ClientLoan
                        {
                            Amount = 50000,
                            ClientId = client1.Id,
                            LoanId = loan2.Id,
                            Payments = "12"
                        };
                        context.ClientLoans.Add(clientLoan2);
                    }
                    var loan3 = context.Loans.FirstOrDefault(l => l.Name == "Automotriz");
                    if (loan3 != null)
                    {
                        var clientLoan3 = new ClientLoan
                        {
                            Amount = 100000,
                            ClientId = client1.Id,
                            LoanId = loan3.Id,
                            Payments = "24"
                        };
                        context.ClientLoans.Add(clientLoan3);
                    }
                    context.SaveChanges();
                }
            }

            if (!context.Cards.Any())
            {
                var client1 = context.Clients.FirstOrDefault(c => c.Email == "froigraina@gmail.com");
                if (client1 != null)
                {
                    var cards = new Card[]
                    {
                        new Card
                        {
                            ClientId = client1.Id,
                            CardHolder = client1.FirstName+ " " + client1.LastName,
                            Type = CardType.DEBIT.ToString(),
                            Color = CardColor.GOLD.ToString(),
                            Number = "3325-6745-7876-4445",
                            Cvv = 990,
                            FromDate = DateTime.Now,
                            ThruDate = DateTime.Now.AddYears(4),
                        },
                        new Card
                        {
                            ClientId = client1.Id,
                            CardHolder = client1.FirstName+ " " + client1.LastName,
                            Type = CardType.CREDIT.ToString(),
                            Color= CardColor.TITANIUM.ToString(),
                            Number = "2234-6745-552-7888",
                            Cvv = 750,
                            FromDate = DateTime.Now,
                            ThruDate = DateTime.Now.AddYears(5),

                        },
                    };
                    foreach (Card card in cards)
                    {
                        context.Cards.Add(card);
                    }
                    context.SaveChanges();
                }
            }
        }

    }
}
