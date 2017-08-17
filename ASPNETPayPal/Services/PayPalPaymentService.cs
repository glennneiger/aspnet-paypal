using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace ASPNETPayPal.Services
{
    public static class PayPalPaymentService 
    {
        public static Payment CreatePayment(string url, string intent)
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
                transactions = GetTransactions(),
                redirect_urls = GetReturnUrls(url, intent)
                
            };

            // Created payment.
            return payment.Create(apiContext);
        }


        private static RedirectUrls GetReturnUrls(string baseUrl, string intent)
        {
            var returnUrl = intent == "sale" ? "/SinglePayments/PaymentSuccessful" : "/SinglePayments/AuthorizeSuccessful";

            // Redirect URLS
            // These URLs will determine how the user is redirected from PayPal 
            // once they have either approved or canceled the payment.
            return new RedirectUrls()
            {
                cancel_url = baseUrl + "/Home/PaymentCancelled",
                return_url = baseUrl + returnUrl
            };
        }

        public static Payment ExecutePayment(string paymentId, string payerId)
        {
            var context = GetApiContext();

            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            var payment = new Payment() { id = paymentId };

            // Execute
            var executed = payment.Execute(context, paymentExecution);

            return executed;
        }

        private static APIContext GetApiContext()
        {
            // Fetch configuration values.
            var config = new Dictionary<string, string>();
            config.Add("clientId", ConfigurationManager.AppSettings["clientId"]);
            config.Add("mode", ConfigurationManager.AppSettings["mode"]);
            config.Add("clientSecret", ConfigurationManager.AppSettings["clientSecret"]);

            var token = new OAuthTokenCredential(config).GetAccessToken();

            return new APIContext(token);
        }

        private static List<Transaction> GetTransactions()
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