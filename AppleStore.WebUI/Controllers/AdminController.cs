using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppleStore.Domain.Abstract;
using AppleStore.Domain.Entities;


namespace AppleStore.WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private IAppleRepository AppleRepository;

        public AdminController(IAppleRepository Repository)
        {
            AppleRepository = Repository;
        }

        
        public ViewResult Index()
        {
            return View(AppleRepository.Products);
        }

        

        public ViewResult Edit(int productId)
        {
            Product game = AppleRepository.Products
                .FirstOrDefault(g => g.ProductId == productId);
            return View(game);
        }

        [HttpPost]
        public ActionResult Edit(Product game, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    game.ImageMimeType = image.ContentType;
                    game.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(game.ImageData, 0, image.ContentLength);
                }
                AppleRepository.SaveProduct(game);
                TempData["message"] = string.Format("Изменения в игре \"{0}\" были сохранены", game.Name);
                return RedirectToAction("Index");
            }
            else
            {
                return View(game);
            }
        }

        public ViewResult Create()
        {
            return View("Edit", new Product());
        }


        public ActionResult Delete(int ProductId)
        {
            Product deletedProduct = AppleRepository.DeleteProduct(ProductId);
            if (deletedProduct != null)
            {
                TempData["message"] = string.Format("Продукт \"{0}\" был удален",
                    deletedProduct.Name);
            }
            return RedirectToAction("Index");
        }
    }
}