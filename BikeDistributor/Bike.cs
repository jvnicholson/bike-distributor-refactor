namespace BikeDistributor
{
    public class Bike
    {
        public Bike(string brand, string model, double retailPrice)
        {
            Brand = brand;
            Model = model;
            RetailPrice = retailPrice;
        }

        public string Brand { get; private set; }
        public string Model { get; private set; }
        public double RetailPrice { get; private set; }

        public string Print()
        {
            return $"{Brand} {Model}";
        }
    }
}
