using PayPal.Api;
using System.Collections.Generic;
using System.Configuration;

namespace ASPNETPayPal.Services
{
    public static class PayPalBillingService
    {
        private static List<PaymentDefinition> GetPaymentDefinitions(bool trial, int trialLength, decimal trialPrice, string frequency)
        {
            var paymentDefinitions = new List<PaymentDefinition>();

            if (trial)
            {
                paymentDefinitions.Add(
                    new PaymentDefinition
                    {
                        name = "Trial",
                        type = "TRIAL",
                        frequency = frequency
                    }
                );
            }

            var regularPayment = new PaymentDefinition
            {
                name = "Standard Plan",
                type = "REGULAR",
                frequency = frequency
            };

            paymentDefinitions.Add(regularPayment);

            return paymentDefinitions;
        }

        public static Plan CreatePlan(string name, string description, string frequency,
            int frequencyInterval, decimal price, bool trial = false, int trialLength = 0)
        {
            return new Plan
            {
                name = name,
                description = description,
                type = "Fixed",
                merchant_preferences = new MerchantPreferences
                {
                   // setup_fee = GetCurrency("1"),
                    return_url = "/Home/PaymentSuccessful",
                    cancel_url = "/Home/PaymentCancelled",
                    auto_bill_amount = "YES",
                    initial_fail_amount_action = "CONTINUE",
                    max_fail_attempts = "0"
                },
                payment_definitions = GetPaymentDefinitions(trial, trialLength, price, frequency)
            };
        }

        public static void ActivatePlan(Plan plan)
        {
            var apiContext = GetApiContext();

            var patchRequest = new PatchRequest()
            {
                new Patch()
                {
                    op = "replace",
                    path = "/",
                    value = plan.state = "ACTIVE"
                }
            };

            plan.Update(apiContext, patchRequest);
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
    }


}