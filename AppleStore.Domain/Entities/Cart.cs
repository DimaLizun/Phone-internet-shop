using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppleStore.Domain.Entities
{
    
   public class Cart
    {
        private List<CartProperty> cartCollection = new List<CartProperty>();

        public void AddItem(Product product,int quantity)
        {
            CartProperty cart = cartCollection
                .Where(p => p.Product.ProductId == product.ProductId)
                .FirstOrDefault();

            if (cart == null)
            {
                cartCollection.Add(
                    new CartProperty
                    {
                        Product = product,
                        Quantity = quantity
                    }
                 );
            }
            else
            {
                cart.Quantity += quantity;
            }
        }

        public void RemoveLine(Product product)
        {
            cartCollection.RemoveAll(p => p.Product.ProductId == product.ProductId);
        }

        public decimal ComputeTotalValue()
        {
             return cartCollection.Sum(s => s.Product.Price * s.Quantity);
        }

        public void Clear()
        {
            cartCollection.Clear();
        }

        public IEnumerable<CartProperty> Lines
        {
            get { return cartCollection; }
        }

    }

  
    public class CartProperty
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
