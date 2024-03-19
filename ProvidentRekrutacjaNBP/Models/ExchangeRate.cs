namespace NBP.Worker.Models
{
    public class ExchangeRate
    {
        public string table { get; set; }
        public string no { get; set; }
        public DateTime effectiveDate { get; set; }
        public List<Rate> rates { get; set; }
    }
}
