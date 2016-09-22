using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppleStore.Domain.Entities;
using AppleStore.Domain.Abstract;
using AppleStore.WebUI.Models;


namespace AppleStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IAppleRepository repository;

        public int sizePage = 4;


        public ProductController(IAppleRepository rep)
        {
            repository = rep;
        }


        public ViewResult List(string category, int page = 1)
        {
            ProductListView model = new ProductListView()
            {
                Products = repository.Products
                      .Where(p => category == null || p.Category == category)
                      .OrderBy(p => p.ProductId)
                      .Skip((page - 1) * sizePage)
                      .Take(sizePage),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = sizePage,
                    TotalItems = repository == null ?
                    repository.Products.Count() :
                    repository.Products.Where(game => game.Category == category).Count()
                },
                CurrentCategory = category
            };

            return View(model);
        }

        public FileContentResult GetImage(int productId)
        {
            Product game = repository.Products
                .FirstOrDefault(g => g.ProductId == productId);

            if (game != null)
            {
                return File(game.ImageData, game.ImageMimeType);
            }
            else
            {
                return null;
            }


        }
    }
}