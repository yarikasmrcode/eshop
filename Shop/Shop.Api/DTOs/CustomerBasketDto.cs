﻿using Shop.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Shop.Api.DTOs
{
    public class CustomerBasketDto
    {
        [Required]
        public string Id { get; set; }
        public List<BasketItemDto> Items { get; set; }
        public int? DeliveryMethodId { get; set; }
        public string ClientServer { get; set; }
        public string PaymentIntentId { get; set; }
    }
}
