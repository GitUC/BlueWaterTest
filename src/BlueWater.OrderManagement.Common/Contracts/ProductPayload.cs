using System;
using System.ComponentModel.DataAnnotations;

namespace BlueWater.OrderManagement.Common.Contracts
{
    public class ProductPayload
    {
        /// <summary>
        /// Product Name
        /// </summary>
        [Required]
        public string ProductName { get; set; }


        /// <summary>
        /// Product unit price
        /// </summary>
        [Required]
        [Range(0.1, 1000.00)]
        public double UnitPrice { get; set; }

    }
}
