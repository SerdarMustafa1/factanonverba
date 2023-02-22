using Collabed.JobPortal.PaymentRequests;
using System;
using Volo.Abp.Application.Dtos;

namespace Collabed.JobPortal.Payment
{
    public class PaymentRequestGetListInput : PagedAndSortedResultRequestDto
    {
        public string ProductName { get; set; }

        public DateTime? MaxCreationTime { get; set; }

        public DateTime? MinCreationTime { get; set; }

        public PaymentRequestState? Status { get; set; }
    }
}
