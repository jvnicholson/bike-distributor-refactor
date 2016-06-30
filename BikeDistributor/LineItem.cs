namespace BikeDistributor
{
    public class LineItem
    {
        public LineItem(Bike bike, int quantity, LineItemDiscountRule discountRule = null)
        {
            Bike = bike;
            Quantity = quantity;
            DiscountRule = discountRule ?? new LineItemDiscountRule();

            updateAmount();
        }

        public double Amount { get; private set; }
        public LineItemDiscountRule DiscountRule { get; private set; }
        public Bike Bike { get; private set; }
        public int Quantity { get; private set; }
        
        public string Print(ReceiptFormat format)
        {
            switch (format)
            {
                case ReceiptFormat.Html:
                    return string.Format("<li>{0} x {1} = {2}</li>", Quantity, Bike.Print(), Amount.ToString("C"));
                case ReceiptFormat.Json:
                    return string.Format("{0} x {1} = {2}", Quantity, Bike.Print(), Amount.ToString("C"));
                default:
                    return string.Format("\t{0} x {1} = {2}", Quantity, Bike.Print(), Amount.ToString("C"));
            }
        }

        public void SetDiscountRule(LineItemDiscountRule discountRule)
        {
            if (discountRule == null)
                return;

            DiscountRule = discountRule;
            updateAmount();
        }

        private void updateAmount()
        {
            Amount = Quantity * Bike.RetailPrice * (1 - DiscountRule.Discount);
        }
    }
}
