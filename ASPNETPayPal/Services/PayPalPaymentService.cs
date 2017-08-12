using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PayPal.Api;

namespace ASPNETPayPal.Services
{
    public class PayPalPaymentService : IPayPalPaymentService
    {
        public Payment CreatePayment(string url, string intent)
        {

            // Using apiContext to authenticate call.
            var apiContext = GetApiContext();

            var payment = new Payment
            {
                intent = "sale",
                payer = new Payer
                {
                    payment_method = "paypal"
                }, 
                transactions = GetTransactions()
            };

            // Created payment.
            var created = payment.Create(apiContext);

            return created;
        }

        public Payment ExecutePayment(string paymentId, string payerId)
        {
            throw new NotImplementedException();
        }

        public APIContext GetApiContext()
        {
            // Fetch configuration values.
            var configuration = ConfigManager.Instance.GetProperties();

            var token = new OAuthTokenCredential(configuration).GetAccessToken();

            return new APIContext(token);
        }

        public List<Transaction> GetTransactions()
        {
            // Returning a list of transactions for the payment.

            return new List<Transaction>()
            {
                new Transaction
                {
                    description = "Random Transaction Description.",
                    invoice_number = new Guid().ToString(),
                    amount = new Amount
                    {
                        currency = "USD",
                        total = "50.00",
                        details = new Details
                        {
                            tax = "10",
                            shipping = "10",
                            subtotal = "30"
                        }
                    }
                }
            };
        }
    }
}