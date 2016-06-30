namespace BikeDistributor
{
    public class LineItemDiscountRule
    {
        public LineItemDiscountRule(double minPrice = 0d, int minQuantity = 1, double discount = 0d)
        {
            MinimumPrice = minPrice >= 0d ? minPrice : 0d;
            MinimumQuantity = minQuantity >= 1 ? minQuantity : 1;
            Discount = discount >= 0d ? discount : 0d;
        }

        public double MinimumPrice { get; private set; }
        public int MinimumQuantity { get; private set; }
        public double Discount { get; private set; }
    }
}
