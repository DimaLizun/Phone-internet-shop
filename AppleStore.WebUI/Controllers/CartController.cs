using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppleStore.Domain.Abstract;
using AppleStore.Domain.Entities;
using AppleStore.WebUI.Models;



namespace AppleStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private IAppleRepository repository;
        private IOrderProcessor orderProcessor;

        public CartController(IAppleRepository repos, IOrderProcessor orderProcess)
        {
            repository = repos;
            orderProcessor = orderProcess;
        }

        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        public RedirectToRouteResult AddCart(Cart cart, int ProductId, string returnUrl)
        {
            Product product = repository.Products
                .FirstOrDefault(p => p.ProductId == ProductId);

            if (product != null)
            {
                cart.AddItem(product, 1);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int ProductId, string returnUrl)
        {
            Product product = repository.Products
                .FirstOrDefault(p => p.ProductId == ProductId);

            if (product != null)
            {
                cart.RemoveLine(product);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        public ViewResult Checkout()
        {
            return View(new ShoppingDetails());
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart,ShoppingDetails shoppingDetails)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Извините, ваша корзина пуста!");
            }

            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart, shoppingDetails);
                cart.Clear();
                return View("Completed");
            }

            else
            {
                return View(shoppingDetails);
            }
        }




    }
}