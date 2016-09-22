using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AppleStore.Domain.Entities;
using System.Web.Mvc;

namespace AppleStore.WebUI.Infrastructure.Binders
{
    public class CartModelBinder: IModelBinder
    {
        private const string sessionkey = "Cart";

        public object BindModel(ControllerContext controllerContext,
           ModelBindingContext bindingContext)
        {
            // Получить объект Cart из сеанса
            Cart cart = null;
            if (controllerContext.HttpContext.Session != null)
            {
                cart = (Cart)controllerContext.HttpContext.Session[sessionkey];
            }

            // Создать объект Cart если он не обнаружен в сеансе
            if (cart == null)
            {
                cart = new Cart();
                controllerContext.HttpContext.Session[sessionkey] = cart;
            }

            // Возвратить объект Cart
            return cart;
        }

    }
}