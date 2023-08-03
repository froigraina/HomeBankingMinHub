using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Runtime.Intrinsics.X86;

namespace HomeBankingMinHub.Models.DTOS
{
    public class CardDTO
    {
        public long Id { get; set; }
        public string CardHolder { get; set; }
        public string Type { get; set; }
        public string Color { get; set; }
        public string Number { get; set; }
        public int Cvv { get; set; }
        public DateTime? FromDate{ get; set; }
        public DateTime? ThruDate { get; set; }
    }
}
