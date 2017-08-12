using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPNETPayPal.Services
{
    interface IPayPalPaymentService
    {
        Payment CreatePayment(string url, string intent);

        Payment ExecutePayment(string paymentId, string payerId);

        List<Transaction> GetTransactions();

        APIContext GetApiContext();

    }
}
