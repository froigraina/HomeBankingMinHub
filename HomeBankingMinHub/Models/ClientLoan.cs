namespace HomeBankingMinHub.Models
{
    //Tabla intermedia
    public class ClientLoan
    {
        public long Id { get; set; }
        public double Amount { get; set; }
        public string Payments { get; set; }

        //Relacion con client
        public long ClientId { get; set; }
        public Client Client { get; set; }

        //Relacion con loan
        public long LoanId { get; set; }
        public Loan Loan { get; set; }
    }
}
