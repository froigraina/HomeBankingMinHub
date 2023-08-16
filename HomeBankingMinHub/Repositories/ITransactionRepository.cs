
using HomeBankingMinHub.Models;
using System.Collections.Generic;

namespace HomeBankingMinHub.Repositories
{
    public interface ITransactionRepository
    {
        Transaction FindByNumber(long id);
        void Save(Transaction transaction);


    }
}
