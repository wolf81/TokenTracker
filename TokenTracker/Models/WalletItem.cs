using SQLite;

namespace TokenTracker.Models
{
    public abstract class WalletItemBase
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        public string Symbol { get; set; }

        public int Amount { get; set; }

        public decimal Price { get; set; }
    }

    public class WalletAddItem : WalletItemBase { }

    public class WalletViewItem : WalletItemBase
    {
        public decimal TotalPrice => Amount * Price;

        public string Description => $"{Amount} x {Price:0.00}";
    }
}
