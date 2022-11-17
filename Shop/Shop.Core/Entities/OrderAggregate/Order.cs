using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Core.Entities.OrderAggregate
{
    public class Order : BaseEntity
    {
        public Order() 
        {

        }
        public Order(IReadOnlyList<OrderItem> orderItems,
                    string buyerEmail,
                    Address shippedToAddress,
                    DeliveryMethod deliveryMethod, 
                    decimal subtotal, string paymentIntentId)
        {
            BuyerEmail = buyerEmail;
            ShippedToAddress = shippedToAddress;
            DeliveryMethod = deliveryMethod;
            OrderItems = orderItems;
            Subtotal = subtotal;
            PaymentIntentId = paymentIntentId;
        }

        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public Address ShippedToAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public IReadOnlyList<OrderItem> OrderItems { get; set; }
        public decimal Subtotal { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public string PaymentIntentId { get; set; }
    
        public decimal GetTotal()
        {
            return Subtotal + DeliveryMethod.Price;
        }
    }
}
