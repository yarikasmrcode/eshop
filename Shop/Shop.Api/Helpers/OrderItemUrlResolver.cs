using AutoMapper;
using AutoMapper.Execution;
using Shop.Api.DTOs;
using Shop.Core.Entities.OrderAggregate;

namespace Shop.Api.Helpers
{
    public class OrderItemUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration config;
        public OrderItemUrlResolver(IConfiguration config)
        {
            this.config = config;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ItemOrdered.PictureUrl))
            {
                return config["ApiUrl"] + source.ItemOrdered.PictureUrl;
            }

            return null;
        }
    }
}
