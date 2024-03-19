using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NBP.Context.Models
{
    public class ExchangeRateTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column(TypeName = "timestamp")]
        public DateTime EffectiveDate { get; set; }
        [Column(TypeName = "varchar(1)")]
        public string TableType { get; set; }
        [Column(TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; }

        public List<ExchangeRateValue> ExchangeRates { get; set; }
    }
}
