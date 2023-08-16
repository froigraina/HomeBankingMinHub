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

        public void Save(Account account)
        {
            if (account.Id == 0)
            {
                Create(account);
            }
            else
            {
                Update(account);
            }

            SaveChanges();
        }



        public IEnumerable<Account> GetAccountsByClient(long clientId)
        {
            return FindByCondition(account => account.ClientId == clientId)
                .Include(account => account.Transactions)
                .ToList();
        }

        public Account FindByNumber(string accountNumber)
        {
            return FindByCondition(account => account.Number == accountNumber).FirstOrDefault();
        }

        public Account FinByNumber(string number)
        {
            return FindByCondition(account => account.Number.ToUpper() == number.ToUpper())
            .Include(account => account.Transactions)
            .FirstOrDefault();
        }

    }
}
