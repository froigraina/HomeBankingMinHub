using System.Collections.Generic;
using HomeBanking.Models;
using HomeBankingMinHub.Models;

namespace HomeBanking.Repositories
{
    public interface IClientRepository
    {
        IEnumerable<Client> GetAllClients();
        void Save(Client client);
        Client FindById(long id);
        Client FindByEmail(string email);

    }
}