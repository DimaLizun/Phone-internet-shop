using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Moq;
using AppleStore.Domain.Abstract;
using AppleStore.Domain.Entities;
using AppleStore.WebUI.Controllers;
using AppleStore.WebUI.Models;
using AppleStore.WebUI.HtmlHelpers;

namespace AppleStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test_Paginate()
        {
            // Организация (arrange)
            Mock<IAppleRepository> mock = new Mock<IAppleRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product { ProductId = 1, Name = "Product1"},
                new Product { ProductId = 2, Name = "Product2"},
                new Product { ProductId = 3, Name = "Product3"},
                new Product { ProductId = 4, Name = "Product4"},
                new Product { ProductId = 5, Name = "Product5"}
            });
            ProductController controller = new ProductController(mock.Object);
            controller.sizePage = 3;

            // Действие (act)
            IEnumerable<Product> result = (IEnumerable<Product>)(ViewResult)controller.List(null,2).Model;

            // Утверждение (assert)
            List<Product> products = result.ToList();
            Assert.IsTrue(products.Count == 2);
            Assert.AreEqual(products[0].Name, "Product4");
            Assert.AreEqual(products[1].Name, "Product5");
        }


        [TestMethod]
        public void TestLinksofPage() 
        {
            HtmlHelper myHelper = null;

            PagingInfo info = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            Func<int, string> pageUrlDelegate = i => "Page" + i;
            MvcHtmlString res = myHelper.Helper(info, pageUrlDelegate);

            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                res.ToString());
        }


        [TestMethod]
        public void Can_Filter_()
        {
            // Организация (arrange)
            Mock<IAppleRepository> mock = new Mock<IAppleRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
    {
        new Product { ProductId = 1, Name = "Игра1", Category="Cat1"},
        new Product { ProductId = 2, Name = "Игра2", Category="Cat2"},
        new Product { ProductId = 3, Name = "Игра3", Category="Cat1"},
        new Product { ProductId = 4, Name = "Игра4", Category="Cat2"},
        new Product { ProductId = 5, Name = "Игра5", Category="Cat3"}
    });
            ProductController controller = new ProductController(mock.Object);
            controller.sizePage = 3;

            // Action
            List<Product> result = ((ProductListView)controller.List("Cat2", 1).Model).Products.ToList();

            // Assert
            Assert.AreEqual(result.Count(), 2);
            Assert.IsTrue(result[0].Name == "Игра2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "Игра4" && result[1].Category == "Cat2");
        }


        [TestMethod]
        public void Can_Create_Categories()
        {
            Mock<IAppleRepository> mock = new Mock<IAppleRepository>();
            mock.Setup(p => p.Products).Returns(new List<Product>
            {
                new Product { ProductId = 1, Name = "Product1", Category = "A"},
                new Product { ProductId = 2, Name = "Product2", Category = "B"},
                new Product { ProductId = 3, Name = "Product3", Category = "C"},
                new Product { ProductId = 4, Name = "Product4", Category = "A"},
                new Product { ProductId = 5, Name = "Product5", Category = "B"}
            });

            NavController target = new NavController(mock.Object);
            List<string> res = ((IEnumerable<string>)target.Menu().Model).ToList();

            Assert.AreEqual(res.Count(), 3);
            Assert.AreEqual(res[0], "A");
            Assert.AreEqual(res[1], "B");
            Assert.AreEqual(res[2], "C");
        }



        [TestMethod]
        public void Indicates_Selected_Category()
        {
            // Организация - создание имитированного хранилища
            Mock<IAppleRepository> mock = new Mock<IAppleRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
        new Product { ProductId = 1, Name = "Product1", Category="Симулятор"},
        new Product { ProductId = 2, Name = "Product2", Category="Шутер"}
    });

            // Организация - создание контроллера
            NavController target = new NavController(mock.Object);

            // Организация - определение выбранной категории
            string categoryToSelect = "Шутер";

            // Действие
            string result = target.Menu(categoryToSelect).ViewBag.SelectedCategory;

            // Утверждение
            Assert.AreEqual(categoryToSelect, result);
        }
    }
}

