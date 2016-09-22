using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppleStore.Domain.Entities;

namespace AppleStore.Domain.Abstract
{
    /// <summary>
    /// interface for Ninject
    /// </summary>
    public interface IAppleRepository
    {
        IEnumerable<Product> Products { get; }
        void SaveProduct(Product Product);
        Product DeleteProduct(int gameId);
    }
}
