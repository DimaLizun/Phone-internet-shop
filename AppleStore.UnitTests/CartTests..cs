using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppleStore.Domain.Entities;
using System.Linq;
using System.Collections.Generic;
using Moq;
using AppleStore.Domain.Abstract;
using System.Web.Mvc;
using AppleStore.WebUI.Controllers;

namespace AppleStore.UnitTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            Product p1 = new Product() { ProductId = 1, Name = "Product1" };
            Product p2= new Product() { ProductId = 2, Name = "Product2" };

            Cart cart = new Cart();
            cart.AddItem(p1,1);
            cart.AddItem(p2,2);

            List<CartProperty> res = cart.Lines.OrderBy(c => c.Product.ProductId).ToList();

            Assert.AreEqual(res.Count(),2);
            Assert.AreEqual(res[0].Product,p1);
            Assert.AreEqual(res[1].Product, p2);
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            // Организация - создание нескольких тестовых игр
            Product p1 = new Product { ProductId = 1, Name = "p1" };
            Product p2 = new Product { ProductId = 2, Name = "p2" };
            Product p3 = new Product { ProductId = 3, Name = "p3" };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - добавление нескольких игр в корзину
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 4);
            cart.AddItem(p3, 2);
            cart.AddItem(p2, 1);

            // Действие
            cart.RemoveLine(p2);

            // Утверждение
            Assert.AreEqual(cart.Lines.Where(c => c.Product == p2).Count(), 0);
            Assert.AreEqual(cart.Lines.Count(), 2);
        }

        [TestMethod]
        public void Calculate_Cart_Total()
        {
            // Организация - создание нескольких тестовых игр
            Product p1 = new Product { ProductId = 1, Name = "p1", Price = 100 };
            Product p2 = new Product { ProductId = 2, Name = "p2", Price = 55 };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);
            cart.AddItem(p1, 5);
            decimal result = cart.ComputeTotalValue();

            // Утверждение
            Assert.AreEqual(result, 655);
        }

        [TestMethod]
        public void Can_Clear_Contents()
        {
            // Организация - создание нескольких тестовых игр
            Product p1 = new Product { ProductId = 1, Name = "p1", Price = 100 };
            Product p2 = new Product { ProductId = 2, Name = "p2", Price = 55 };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);
            cart.AddItem(p1, 5);
            cart.Clear();

            // Утверждение
            Assert.AreEqual(cart.Lines.Count(), 0);
        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {

            // Организация - создание имитированного обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Организация - создание пустой корзины
            Cart cart = new Cart();

            // Организация - создание деталей о доставке
            ShoppingDetails shippingDetails = new ShoppingDetails();

            // Организация - создание контроллера
            CartController controller = new CartController(null, mock.Object);

            // Действие
            ViewResult result = controller.Checkout(cart, shippingDetails);

            // Утверждение — проверка, что заказ не был передан обработчику 
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShoppingDetails>()),
                Times.Never());

            // Утверждение — проверка, что метод вернул стандартное представление 
            Assert.AreEqual("", result.ViewName);

            // Утверждение - проверка, что-представлению передана неверная модель
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);


        }

        }
}
