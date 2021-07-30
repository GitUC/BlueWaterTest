using BlueWater.OrderManagement.Common.Contracts;
using System;

namespace BlueWater.OrderManagement.Services.Interfaces
{
    public interface IOrderProcess
    {
        Guid CreateOrder(Orders order);

        string GetOrderStatus(string id);

        void DispatchOrder();
    }
}
