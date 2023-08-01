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
        }

    }
}
