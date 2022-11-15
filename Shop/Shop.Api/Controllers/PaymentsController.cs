using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Core.Entities;
using Shop.Core.Interfaces;
using Stripe;

namespace Shop.Api.Controllers
{
    public class PaymentsController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        public PaymentsController(IPaymentService service)
        {
            _paymentService = service;
        }

        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            return await _paymentService.CreateOrUpdatePaymentIntent(basketId);
        }
    }
}
