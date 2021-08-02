using BlueWater.OrderManagement.Common.Contracts;
using System;
using System.Threading.Tasks;

namespace BlueWater.OrderManagement.Services.Interfaces
{
    public interface IOrderProcess
    {
        Task CreateOrder(Orders order);

        string GetOrderStatus(string id);

        Task DispatchOrder();
    }
}
