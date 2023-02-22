using System;

namespace Collabed.JobPortal.Web.PaymentRequest
{
    public interface IPaymentUrlBuilder
    {
        Uri BuildCheckoutUrl(Guid paymentRequestId);

        Uri BuildReturnUrl(Guid paymentRequestId);
    }
}