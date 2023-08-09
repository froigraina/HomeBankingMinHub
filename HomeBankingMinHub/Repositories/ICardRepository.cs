using HomeBankingMinHub.Models;

namespace HomeBankingMinHub.Repositories
{
    public interface ICardRepository
    {
        Card FindByNumber(string number);
        void Save(Card card);
    }
}
