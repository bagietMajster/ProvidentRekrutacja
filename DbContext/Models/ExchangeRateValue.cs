using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NBP.Context.Models
{
    public class ExchangeRateValue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column(TypeName = "varchar")]
        public string CurrencyCode { get; set; }
        [Column(TypeName = "double")]
        public double Mid { get; set; }
        [Column(TypeName = "integer")]
        public int ExchangeRateTableId { get; set; }

        public ExchangeRateTable ExchangeRateTable { get; set; }
    }
}
