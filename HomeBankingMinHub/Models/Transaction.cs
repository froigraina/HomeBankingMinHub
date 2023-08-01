using System;

namespace HomeBankingMinHub.Models
{
    public class Transaction
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }

        //Relacion con la clase Account
        public Account Account { get; set; }
        public long AccountId { get; set; } 

    }
}
