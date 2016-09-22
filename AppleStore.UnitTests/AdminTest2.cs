using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using AppleStore.Domain.Abstract;
using AppleStore.Domain.Entities;
using AppleStore.WebUI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace AppleStore.UnitTests
{
    [TestClass]
    public class AdminTest2
    {
        [TestMethod]
        public void Index_Contains_All_Games()
        {
            Mock<IAppleRepository> rep = new Mock<IAppleRepository>();
            rep.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product { ProductId = 1, Name = "prod1" },
                new Product { ProductId = 2, Name = "prod2" },
                new Product { ProductId = 3, Name = "prod3" },
                new Product { ProductId = 4, Name = "prod4" }
            });

            AdminController controller = new AdminController(rep.Object);

            List<Product> res = ((IEnumerable<Product>)controller.Index().ViewData.Model).ToList();

            Assert.AreEqual(res.Count, 4);
            Assert.AreEqual("prod1",res[0].Name);
        }



        [TestMethod]
        public void Can_Edit_Game()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IAppleRepository> mock = new Mock<IAppleRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
    {
        new Product { ProductId = 1, Name = "Игра1"},
        new Product { ProductId = 2, Name = "Игра2"},
        new Product { ProductId = 3, Name = "Игра3"},
        new Product{ ProductId = 4, Name = "Игра4"},
        new Product { ProductId = 5, Name = "Игра5"}
    });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие
            Product game1 = controller.Edit(1).ViewData.Model as Product;
            Product game2 = controller.Edit(2).ViewData.Model as Product;
            Product game3 = controller.Edit(3).ViewData.Model as Product;

            // Assert
            Assert.AreEqual(1, game1.ProductId);
            Assert.AreEqual(2, game2.ProductId);
            Assert.AreEqual(3, game3.ProductId);
        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IAppleRepository> mock = new Mock<IAppleRepository>();

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Организация - создание объекта Game
            Product game = new Product { Name = "Test" };

            // Действие - попытка сохранения товара
            ActionResult result = controller.Edit(game);

            // Утверждение - проверка того, что к хранилищу производится обращение
            mock.Verify(m => m.SaveProduct(game));

            // Утверждение - проверка типа результата метода
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IAppleRepository> mock = new Mock<IAppleRepository>();

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Организация - создание объекта Game
            Product game = new Product { Name = "Test" };

            // Организация - добавление ошибки в состояние модели
            controller.ModelState.AddModelError("error", "error");

            // Действие - попытка сохранения товара
            ActionResult result = controller.Edit(game);

            // Утверждение - проверка того, что обращение к хранилищу НЕ производится 
            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never());

            // Утверждение - проверка типа результата метода
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}
