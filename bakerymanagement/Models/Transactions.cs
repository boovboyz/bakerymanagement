namespace bakerymanagement.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public User User { get; set; }
        public Product Product { get; set; }
        public int QuantityChanged { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
