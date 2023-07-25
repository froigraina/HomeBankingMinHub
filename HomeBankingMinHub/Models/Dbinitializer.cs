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
        }
    }
}
