using HomeBankingMinHub.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HomeBankingMinHub.Repositories
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(HomeBankingContext repositoryContext) : base(repositoryContext)
        {
        }
        public IEnumerable<Account> GetAllAccounts()
        {
            return FindAll().Include(account => account.Transactions).ToList();
        }

        public Account FindById(long id)
        {
            return FindByCondition(account => account.Id == id).Include(account => account.Transactions).FirstOrDefault();
        }
    }
}
