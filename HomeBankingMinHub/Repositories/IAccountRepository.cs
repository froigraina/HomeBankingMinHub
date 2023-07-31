using HomeBankingMinHub.Models;
using System.Collections;
using System.Collections.Generic;

namespace HomeBankingMinHub.Repositories
{
    public interface IAccountRepository
    {
        IEnumerable<Account> GetAllAccounts();
        Account FindById(long id);
    }
}
