using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BikeDistributor.Test
{
    [TestClass]
    public class OrderTest
    {
        private static readonly Bike Defy = new Bike("Giant", "Defy 1", 1000d);
        private static readonly Bike Elite = new Bike("Specialized", "Venge Elite", 2000d);
        private static readonly Bike DuraAce = new Bike("Specialized", "S-Works Venge Dura-Ace", 5000d);
        private static readonly Bike ValueKing = new Bike("Diamond Back", "Value King Pro", 499.99d);

        private const string ResultStatementOneDefy = @"Order Receipt for Anywhere Bike Shop
	1 x Giant Defy 1 = $1,000.00
Sub-Total: $1,000.00
Tax: $72.50
Total: $1,072.50";

        private const string ResultStatementMultipleItems = @"Order Receipt for Anywhere Bike Shop
	5 x Specialized S-Works Venge Dura-Ace = $20,000.00
	10 x Specialized Venge Elite = $16,000.00
	30 x Diamond Back Value King Pro = $13,499.73
Sub-Total: $49,499.73
Tax: $3,588.73
Total: $53,088.46";

        private const string ResultStatementOneElite = @"Order Receipt for Anywhere Bike Shop
	1 x Specialized Venge Elite = $2,000.00
Sub-Total: $2,000.00
Tax: $145.00
Total: $2,145.00";

        private const string ResultStatementTenElite = @"Order Receipt for Anywhere Bike Shop
	10 x Specialized Venge Elite = $16,000.00
Sub-Total: $16,000.00
Tax: $1,160.00
Total: $17,160.00";

        private const string ResultStatementOneDuraAce = @"Order Receipt for Anywhere Bike Shop
	1 x Specialized S-Works Venge Dura-Ace = $5,000.00
Sub-Total: $5,000.00
Tax: $362.50
Total: $5,362.50";
        
        private const string ResultStatementFiveDuraAce = @"Order Receipt for Anywhere Bike Shop
	5 x Specialized S-Works Venge Dura-Ace = $20,000.00
Sub-Total: $20,000.00
Tax: $1,450.00
Total: $21,450.00";

        private const string ResultStatementThirtyValueKing = @"Order Receipt for Anywhere Bike Shop
	30 x Diamond Back Value King Pro = $13,499.73
Sub-Total: $13,499.73
Tax: $978.73
Total: $14,478.46";

        private const string HtmlResultStatementOneDefy = @"<html><body><h1>Order Receipt for Anywhere Bike Shop</h1><ul><li>1 x Giant Defy 1 = $1,000.00</li></ul><h3>Sub-Total: $1,000.00</h3><h3>Tax: $72.50</h3><h2>Total: $1,072.50</h2></body></html>";
        private const string HtmlResultStatementOneElite = @"<html><body><h1>Order Receipt for Anywhere Bike Shop</h1><ul><li>1 x Specialized Venge Elite = $2,000.00</li></ul><h3>Sub-Total: $2,000.00</h3><h3>Tax: $145.00</h3><h2>Total: $2,145.00</h2></body></html>";
        private const string HtmlResultStatementOneDuraAce = @"<html><body><h1>Order Receipt for Anywhere Bike Shop</h1><ul><li>1 x Specialized S-Works Venge Dura-Ace = $5,000.00</li></ul><h3>Sub-Total: $5,000.00</h3><h3>Tax: $362.50</h3><h2>Total: $5,362.50</h2></body></html>";
        private const string HtmlResultStatementMultipleItems = @"<html><body><h1>Order Receipt for Anywhere Bike Shop</h1><ul><li>5 x Specialized S-Works Venge Dura-Ace = $20,000.00</li><li>10 x Specialized Venge Elite = $16,000.00</li><li>30 x Diamond Back Value King Pro = $13,499.73</li></ul><h3>Sub-Total: $49,499.73</h3><h3>Tax: $3,588.73</h3><h2>Total: $53,088.46</h2></body></html>";

        private const string JsonResultStatementOneDefy = @"{""header"": ""Order Receipt for Anywhere Bike Shop"", ""lineItems"": [""1 x Giant Defy 1 = $1,000.00""], ""footer"": {""subtotal"": ""$1,000.00"", ""tax"": ""$72.50"", ""total"": ""$1,072.50""}}";
        private const string JsonResultStatementMultipleItems = @"{""header"": ""Order Receipt for Anywhere Bike Shop"", ""lineItems"": [""5 x Specialized S-Works Venge Dura-Ace = $20,000.00"",""10 x Specialized Venge Elite = $16,000.00"",""30 x Diamond Back Value King Pro = $13,499.73""], ""footer"": {""subtotal"": ""$49,499.73"", ""tax"": ""$3,588.73"", ""total"": ""$53,088.46""}}";

        [TestMethod]
        public void ReceiptOneDefy()
        {
            var order = new Order("Anywhere Bike Shop");
            order.AddLineItem(new LineItem(Defy, 1));
            Assert.AreEqual(ResultStatementOneDefy, order.Receipt(ReceiptFormat.Text));
        }

        [TestMethod]
        public void ReceiptOneElite()
        {
            var order = new Order("Anywhere Bike Shop");
            order.AddLineItem(new LineItem(Elite, 1));
            Assert.AreEqual(ResultStatementOneElite, order.Receipt(ReceiptFormat.Text));
        }

        [TestMethod]
        public void ReceiptTenElite()
        {
            var order = new Order("Anywhere Bike Shop");
            order.AddLineItem(new LineItem(Elite, 10));
            Assert.AreEqual(ResultStatementTenElite, order.Receipt(ReceiptFormat.Text));
        }

        [TestMethod]
        public void ReceiptOneDuraAce()
        {
            var order = new Order("Anywhere Bike Shop");
            order.AddLineItem(new LineItem(DuraAce, 1));
            Assert.AreEqual(ResultStatementOneDuraAce, order.Receipt(ReceiptFormat.Text));
        }

        [TestMethod]
        public void ReceiptFiveDuraAce()
        {
            var order = new Order("Anywhere Bike Shop");
            order.AddLineItem(new LineItem(DuraAce, 5));
            Assert.AreEqual(ResultStatementFiveDuraAce, order.Receipt(ReceiptFormat.Text));
        }

        [TestMethod]
        public void ReceiptThirtyValueKing_CustomDiscount()
        {
            var order = new Order("Anywhere Bike Shop");

            order.AddDiscountRule(400d, 30, 0.1d);
            order.AddLineItem(new LineItem(ValueKing, 30));
            Assert.AreEqual(ResultStatementThirtyValueKing, order.Receipt(ReceiptFormat.Text));
        }

        [TestMethod]
        public void HtmlReceiptOneDefy()
        {
            var order = new Order("Anywhere Bike Shop");
            order.AddLineItem(new LineItem(Defy, 1));
            Assert.AreEqual(HtmlResultStatementOneDefy, order.Receipt(ReceiptFormat.Html));
        }

        [TestMethod]
        public void HtmlReceiptOneElite()
        {
            var order = new Order("Anywhere Bike Shop");
            order.AddLineItem(new LineItem(Elite, 1));
            Assert.AreEqual(HtmlResultStatementOneElite, order.Receipt(ReceiptFormat.Html));
        }

        [TestMethod]
        public void HtmlReceiptOneDuraAce()
        {
            var order = new Order("Anywhere Bike Shop");
            order.AddLineItem(new LineItem(DuraAce, 1));
            Assert.AreEqual(HtmlResultStatementOneDuraAce, order.Receipt(ReceiptFormat.Html));
        }

        [TestMethod]
        public void JsonReceiptOneDefy()
        {
            var order = new Order("Anywhere Bike Shop");
            order.AddLineItem(new LineItem(Defy, 1));
            Assert.AreEqual(JsonResultStatementOneDefy, order.Receipt(ReceiptFormat.Json));
        }

        [TestMethod]
        public void JsonReceipt_MultipleLineItems_MultipleDiscounts()
        {
            var order = new Order("Anywhere Bike Shop");

            order.AddDiscountRule(400d, 30, 0.1d);
            order.AddLineItem(new LineItem(DuraAce, 5));
            order.AddLineItem(new LineItem(Elite, 10));
            order.AddLineItem(new LineItem(ValueKing, 30));

            Assert.AreEqual(JsonResultStatementMultipleItems, order.Receipt(ReceiptFormat.Json));
        }

        [TestMethod]
        public void Receipt_MultipleLineItems_MultipleDiscounts()
        {
            var order = new Order("Anywhere Bike Shop");

            order.AddDiscountRule(400d, 30, 0.1d);
            order.AddLineItem(new LineItem(DuraAce, 5));
            order.AddLineItem(new LineItem(Elite, 10));
            order.AddLineItem(new LineItem(ValueKing, 30));

            Assert.AreEqual(ResultStatementMultipleItems, order.Receipt(ReceiptFormat.Text));
        }

        [TestMethod]
        public void HtmlReceipt_MultipleLineItems_MultipleDiscounts()
        {
            var order = new Order("Anywhere Bike Shop");

            order.AddDiscountRule(400d, 30, 0.1d);
            order.AddLineItem(new LineItem(DuraAce, 5));
            order.AddLineItem(new LineItem(Elite, 10));
            order.AddLineItem(new LineItem(ValueKing, 30));

            Assert.AreEqual(HtmlResultStatementMultipleItems, order.Receipt(ReceiptFormat.Html));
        }
    }
}


