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
                        new Account{ClientId= accountRoig.Id, CreationDate = DateTime.Now, Number=String.Empty, Balance=5000}
                    };
                    foreach (Account account in accounts)
                    {
                        context.Accounts.Add(account);
                    }
                }
                context.SaveChanges();

            }
        }

    }
}
