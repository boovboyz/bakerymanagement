using System.Collections.Generic;

namespace bakerymanagement.Models
{
    public class UpdateInventoryViewModel
    {
        public string UserName { get; set; }
        public List<ProductUpdateModel>
    ProductUpdates
        { get; set; }
    }

    public class ProductUpdateModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int CurrentQuantity { get; set; }
        public int QuantityChanged { get; set; }
    }
}