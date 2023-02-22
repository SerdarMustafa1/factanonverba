using Collabed.JobPortal.Payment;
using Collabed.JobPortal.PaymentRequests;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Collabed.JobPortal.Payments
{
    // TODO: Proper Admin permissions (Post MVP)
    //[Authorize(PaymentAdminPermissions.Request.Default)]
    [Authorize]
    public class PaymentRequestAdminAppService : ApplicationService, IPaymentRequestAdminAppService
    {
        protected IPaymentRequestRepository PaymentRequestRepository { get; }

        public PaymentRequestAdminAppService(IPaymentRequestRepository paymentRequestRepository)
        {
            PaymentRequestRepository = paymentRequestRepository;
        }

        public async Task<PagedResultDto<PaymentRequestWithDetailsDto>> GetListAsync(PaymentRequestGetListInput input)
        {
            var paymentRequests = await PaymentRequestRepository.GetListAsync(
                input.SkipCount,
                input.MaxResultCount,
                input.Sorting,
                input.ProductName,
                input.MaxCreationTime,
                input.MinCreationTime,
                input.Status
            );

            var totalCount = await PaymentRequestRepository.GetCountAsync(
                input.ProductName,
                input.MaxCreationTime,
                input.MinCreationTime,
                input.Status
            );

            return new PagedResultDto<PaymentRequestWithDetailsDto>(
                totalCount,
                ObjectMapper.Map<List<PaymentRequest>, List<PaymentRequestWithDetailsDto>>(paymentRequests)
            );
        }
    }
}
