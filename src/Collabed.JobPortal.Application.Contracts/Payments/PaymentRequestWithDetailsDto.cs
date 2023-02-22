using Collabed.JobPortal.PaymentRequests;
using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;

namespace Collabed.JobPortal.Payments
{
    public class PaymentRequestWithDetailsDto : ExtensibleEntityDto<Guid>, IHasCreationTime
    {
        public string CustomerId { get; set; }

        public string ProductName { get; set; }

        public float Price { get; set; }

        public string Currency { get; set; }

        public PaymentRequestState State { get; set; }

        public string FailReason { get; set; }

        public DateTime CreationTime { get; }
    }
}
