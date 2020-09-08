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

    public class WalletAddTokenItem : WalletItemBase { }

    public class WalletViewTotalItem : WalletItemBase { }

    public class WalletViewTokenItem : WalletItemBase
    {
        public decimal TotalPrice => Amount * Price;

        public string Description => $"{Amount} × {Price:0.00}";
    }
}
