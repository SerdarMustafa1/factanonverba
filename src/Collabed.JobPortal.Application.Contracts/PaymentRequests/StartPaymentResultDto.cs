using System;

namespace Collabed.JobPortal.PaymentRequests
{
    [Serializable]
    public class StartPaymentResultDto
    {
        public string CheckoutLink { get; set; }
    }
}
