using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlueWater.OrderManagement.Common.Contracts
{
    public class Orders
    {
        /// <summary>
        /// User account 
        /// </summary>
        [Required]
        [DataType(DataType.EmailAddress)]
        public string UserAccount { get; set; }

        /// <summary>
        /// List of order items
        /// </summary>
        public List<OrderDetails> OrderDetails {get; set;}
    }
}
