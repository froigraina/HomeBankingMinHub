using HomeBankingMinHub.Models;
using System.Linq;

namespace HomeBankingMinHub.Repositories
{
    public class CardRepository : RepositoryBase<Card>, ICardRepository
    {
        public CardRepository(HomeBankingContext repositoryContext) : base(repositoryContext)
        {
        }
        
        public Card FindByNumber(string cardNumber)
        {
            return FindByCondition(card => card.Number == cardNumber).FirstOrDefault();
        }

        public void Save(Card card)
        {
            Create(card);
            SaveChanges();
        }
    }
}

