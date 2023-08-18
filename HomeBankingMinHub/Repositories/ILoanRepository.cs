using HomeBankingMinHub.Models;
using System.Collections;
using System.Collections.Generic;

namespace HomeBankingMinHub.Repositories
{
    public interface ILoanRepository
    {
        IEnumerable<Loan> GetAll();
        Loan FindById(long id);
    }
}
