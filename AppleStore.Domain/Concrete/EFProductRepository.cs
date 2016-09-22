using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppleStore.Domain.Concrete;
using AppleStore.Domain.Abstract;
using AppleStore.Domain.Entities;

namespace AppleStore.Domain.Concrete
{
    /// <summary>
    /// Work with Entity Framework
    /// </summary>
   public class EFProductRepository: IAppleRepository
    {
        EFDbContext db = new EFDbContext();

        public IEnumerable<Product> Products 
        {
            get { return db.Products;}
        }

        public void SaveProduct(Product product)
        {
            if (product.ProductId == 0)
            {
                db.Products.Add(product);
            }

            else
            {
                Product dbEntry = db.Products.Find(product.ProductId);

                if (dbEntry != null)
                {
                    dbEntry.Name = product.Name;
                    dbEntry.Description = product.Description;
                    dbEntry.Price = product.Price;
                    dbEntry.Category = product.Category;
                    dbEntry.ImageData = product.ImageData;
                    dbEntry.ImageMimeType = product.ImageMimeType;
                }
            }

            db.SaveChanges();
        }

        public Product DeleteProduct(int ProductId)
        {
            Product dbEntry = db.Products.Find(ProductId);
            if (dbEntry != null)
            {
                db.Products.Remove(dbEntry);
                db.SaveChanges();
            }
            return dbEntry;
        }
    }
}
