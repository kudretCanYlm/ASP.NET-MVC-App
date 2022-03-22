using SportStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportStore.Domain.Abstract
{
    public interface IOrderProcessor
    {
        //bu class bind edilecek
        void ProcessOrder(Cart cart, ShippingDetails shippingDetails);
    }
}
