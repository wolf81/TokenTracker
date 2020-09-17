using SQLite;
using TokenTracker.ViewModels.Base;

namespace TokenTracker.Models
{
    public abstract class WalletItemBase : ExtendedBindableObject
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        private string tokenSymbol;
        public string TokenSymbol
        {
            get => tokenSymbol;
            set { SetProperty(ref tokenSymbol, value); Update(); }
        }

        private string currencySymbol;
        public string CurrencySymbol
        {
            get => currencySymbol;
            set { SetProperty(ref currencySymbol, value); Update(); }
        }

        private decimal multiplyFactor = new decimal(1.0);
        public decimal MultiplyFactor
        {
            get => multiplyFactor;
            set { SetProperty(ref multiplyFactor, value); Update(); }
        }

        private int amount;
        public int Amount
        {
            get => amount;
            set { SetProperty(ref amount, value); Update(); }
        }

        private decimal price;
        public decimal Price
        {
            get => price;
            set { SetProperty(ref price, value); Update(); }
        }

        protected abstract void Update();
    }

    public class WalletAddTokenItem : WalletItemBase
    {
        protected override void Update() { }
    }

    public class WalletViewTotalItem : WalletItemBase
    {
        private string value;
        public string Value
        {
            get => value;
            private set { SetProperty(ref this.value, value); }
        }

        protected override void Update()
        {
            Value = $"{Price:0.00} {CurrencySymbol}";
        }
    }

    public class WalletViewTokenItem : WalletItemBase
    {
        private decimal totalPrice;
        public decimal TotalPrice
        {
            get => totalPrice;
            set => SetProperty(ref totalPrice, value);
        } 

        private string description;
        public string Description
        {
            get => description;
            private set { SetProperty(ref description, value); }
        }

        private string value;
        public string Value
        {
            get => value;
            private set { SetProperty(ref this.value, value); }
        }

        protected override void Update()
        {
            var price = decimal.Multiply(Price, MultiplyFactor);
            TotalPrice = Amount * price;
            Description = $"{Amount} × {price:0.00} {CurrencySymbol}";
            Value = $"{TotalPrice:0.00} {CurrencySymbol}";
        }
    }
}
