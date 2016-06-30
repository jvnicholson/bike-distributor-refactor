using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BikeDistributor
{
    public enum ReceiptFormat
    {
        Text,
        Html,
        Json
    }

    public class Order
    {
        private const double TaxRate = .0725d;
        private readonly IList<LineItem> _lineItems = new List<LineItem>();

        public Order(string company)
        {
            Company = company;
            DiscountRules = new List<LineItemDiscountRule>();

            initDiscountRules();
        }

        public string Company { get; private set; }
        public List<LineItemDiscountRule> DiscountRules { get; set; }

        public void AddDiscountRule(double minPrice, int minQuantity, double discount)
        {
            DiscountRules.Add(new LineItemDiscountRule(minPrice, minQuantity, discount));
        }

        public void AddLineItem(LineItem lineItem)
        {
            lineItem.SetDiscountRule(getDiscountRuleForLineItem(lineItem));
            _lineItems.Add(lineItem);
        }

        private string buildReceiptBody(ReceiptFormat format)
        {
            var sbBody = new StringBuilder();

            switch (format)
            {
                case ReceiptFormat.Html:
                    if (!_lineItems.Any()) break;

                    sbBody.Append("<ul>");
                    foreach (var lineItem in _lineItems)
                    {
                        sbBody.Append(lineItem.Print(format));
                    }
                    sbBody.Append("</ul>");
                    break;

                case ReceiptFormat.Json:
                    sbBody.Append(@"""lineItems"": [");
                    for (var i = 0; i < _lineItems.Count; i++)
                    {
                        sbBody.Append(@"""" + _lineItems[i].Print(format) + @"""");
                        if (i < _lineItems.Count - 1)
                            sbBody.Append(",");
                    }
                    sbBody.Append(@"], ");
                    break;

                default:
                    foreach (var lineItem in _lineItems)
                    {
                        sbBody.AppendLine(lineItem.Print(format));
                    }
                    break;
            }

            return sbBody.ToString();
        }

        private string buildReceiptFooter(ReceiptFormat format, double subTotal)
        {
            StringBuilder sbFooter = new StringBuilder();
            var tax = subTotal * TaxRate;

            switch (format)
            {
                case ReceiptFormat.Html:
                    sbFooter.Append(string.Format("<h3>Sub-Total: {0}</h3>", subTotal.ToString("C")));
                    sbFooter.Append(string.Format("<h3>Tax: {0}</h3>", tax.ToString("C")));
                    sbFooter.Append(string.Format("<h2>Total: {0}</h2>", (subTotal + tax).ToString("C")));
                    sbFooter.Append("</body></html>");
                    break;

                case ReceiptFormat.Json:
                    sbFooter.Append(@"""footer"": {");
                    sbFooter.Append(string.Format("\"subtotal\": \"{0}\", ", subTotal.ToString("C")));
                    sbFooter.Append(string.Format("\"tax\": \"{0}\", ", tax.ToString("C")));
                    sbFooter.Append(string.Format("\"total\": \"{0}\"", (subTotal + tax).ToString("C")));
                    sbFooter.Append(@"}}");
                    break;

                default:
                    sbFooter.AppendLine(string.Format("Sub-Total: {0}", subTotal.ToString("C")));
                    sbFooter.AppendLine(string.Format("Tax: {0}", tax.ToString("C")));
                    sbFooter.Append(string.Format("Total: {0}", (subTotal + tax).ToString("C")));
                    break;
            }

            return sbFooter.ToString();
        }

        private string buildReceiptHeader(ReceiptFormat format)
        {
            StringBuilder sbHeader;

            switch (format)
            {
                case ReceiptFormat.Html:
                    sbHeader = new StringBuilder(string.Format("<html><body><h1>Order Receipt for {0}</h1>", Company));
                    break;

                case ReceiptFormat.Json:
                    sbHeader = new StringBuilder(string.Format("{{\"header\": \"Order Receipt for {0}\", ", Company));
                    break;

                default:
                    sbHeader = new StringBuilder(string.Format("Order Receipt for {0}{1}", Company, Environment.NewLine));
                    break;
            }
                
            return sbHeader.ToString();
        }

        private LineItemDiscountRule getDiscountRuleForLineItem(LineItem lineItem)
        {
            foreach (var discountRule in DiscountRules.OrderByDescending(r => r.MinimumPrice))
            {
                if (lineItem.Bike.RetailPrice >= discountRule.MinimumPrice &&
                    lineItem.Quantity >= discountRule.MinimumQuantity)

                    return discountRule;
            }

            return null;
        }

        private double getLineItemsSubTotal()
        {
            var subtotal = 0d;
            foreach (var lineItem in _lineItems)
            {
                subtotal += lineItem.Amount;
            }

            return subtotal;
        }

        private void initDiscountRules()
        {
            // $1000: 10% discount for quantities >= 20
            DiscountRules.Add(new LineItemDiscountRule(1000d, 20, 0.1));

            // $2000: 20% discount for quantities >= 10
            DiscountRules.Add(new LineItemDiscountRule(2000d, 10, 0.2));

            // $5000: 20% discount for quantities >= 5
            DiscountRules.Add(new LineItemDiscountRule(5000d, 5, 0.2));
        }

        public string Receipt(ReceiptFormat format)
        {
            StringBuilder result = new StringBuilder();
            var subTotal = getLineItemsSubTotal();

            result.Append(buildReceiptHeader(format));
            result.Append(buildReceiptBody(format));
            result.Append(buildReceiptFooter(format, subTotal));
            
            return result.ToString();
        }
    }
}
