using System;

namespace BlueWater.OrderMangement.DataAccess.DataModel
{
    public class Product
    {
        /// <summary>
        /// ProductId
        /// </summary>
        public Guid ProductId { get; set; }


        /// <summary>
        /// Product Name
        /// </summary>
        public string ProductName { get; set; }


        /// <summary>
        /// Product unit price
        /// </summary>
        public double UnitPrice { get; set; }


        /// <summary>
        /// Last Edit datetime for the Product 
        /// </summary>
        public DateTime LastEnditTime { get; set; }
    }
}
