using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.Services;
using MyShop.WebUI.Controllers;
using MyShop.WebUI.Tests.Mocks;

namespace MyShop.WebUI.Tests.Controllers
{
    [TestClass]
    public class BasketControllerTest
    {
        [TestMethod]
        public void CanAddBasketItem()
        {
            //setup 
            IRepository<Basket> baskets = new MockContext<Basket>();
            IRepository<Product> products = new MockContext<Product>();
            IRepository<Order> order = new MockContext<Order>();

            var httpContext = new MockHttpContext();
            
            IBasketService basketService = new BasketService(products, baskets);
            IOrderService orderService = new OrderService(order);

            var controller = new BasketController(basketService, orderService);

            controller.ControllerContext = new System.Web.Mvc.ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);

            //Act - Run your tests
            //basketService.addToBasket(httpContext, "1");

            controller.AddToBasket("1");

            Basket basket = baskets.Collection().FirstOrDefault();


            //Assert
            Assert.IsNotNull(basket);
            Assert.AreEqual(1, basket.BasketItems.Count);
            Assert.AreEqual("1", basket.BasketItems.ToList().FirstOrDefault().ProductId);
        }

        [TestMethod]
        public void CanGetSummaryViewModel()
        {
            //setup 
            IRepository<Basket> baskets = new MockContext<Basket>();
            IRepository<Product> products = new MockContext<Product>();
            IRepository<Order> order = new MockContext<Order>();

            products.Insert(new Product() { Id = "1", Price = 10.00m });
            products.Insert(new Product() { Id = "2", Price = 50.00m });
            products.Insert(new Product() { Id = "3", Price = 90.00m });
            products.Insert(new Product() { Id = "4", Price = 70.00m });
            products.Insert(new Product() { Id = "5", Price = 20.00m });

            Basket basket = new Basket();

            basket.BasketItems.Add(new BasketItem() { ProductId = "1", Quantity = 2 });
            basket.BasketItems.Add(new BasketItem() { ProductId = "2", Quantity = 3 });
            basket.BasketItems.Add(new BasketItem() { ProductId = "3", Quantity = 1 });
            basket.BasketItems.Add(new BasketItem() { ProductId = "4", Quantity = 4 });
            basket.BasketItems.Add(new BasketItem() { ProductId = "5", Quantity = 6 });

            baskets.Insert(basket);

            IBasketService basketService = new BasketService(products, baskets);
            IOrderService orderService = new OrderService(order);
            var controller = new BasketController(basketService, orderService);

            var httpContext = new MockHttpContext();

            httpContext.Request.Cookies.Add(new System.Web.HttpCookie("eCommerceBasket") { Value = basket.Id });
            controller.ControllerContext = new System.Web.Mvc.ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);

            var result = controller.BasketSummary() as PartialViewResult;
            var basketSummary = (BasketSummaryViewModel)result.ViewData.Model;

            Assert.AreEqual(16, basketSummary.BasketCount);
            Assert.AreEqual(660, basketSummary.BasketTotal);
        }

        [TestMethod]
        public void canCeckoutAndCreateOrder()
        {
            IRepository<Product> products = new MockContext<Product>();

            products.Insert(new Product()
            {
                Id = "1",
                Price = 10.0m
            });
            products.Insert(new Product()
            {
                Id = "2",
                Price = 30.0m
            });

            IRepository<Basket> basktes = new MockContext<Basket>();
            Basket basket = new Basket();
            basket.BasketItems.Add(new BasketItem() { ProductId = "1", Quantity = 2, BasketId = basket.Id });
            basket.BasketItems.Add(new BasketItem() { ProductId = "2", Quantity = 3, BasketId = basket.Id });
            basktes.Insert(basket);

            IBasketService basketService = new BasketService(products, basktes);

            IRepository<Order> orders = new MockContext<Order>();
            IOrderService orderService = new OrderService(orders);

            var controller = new BasketController(basketService, orderService);

            var httpContext = new MockHttpContext();

            httpContext.Request.Cookies.Add(new System.Web.HttpCookie("eCommerceBasket") { Value = basket.Id });

            controller.ControllerContext = new ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);

            //Act
            Order order = new Order();
            controller.Checkout(order);

            //assert
            Assert.AreEqual(2, order.OrderItems.Count);
            Assert.AreEqual(0, basket.BasketItems.Count);

            Order orderInRep = orders.Find(order.Id);
            Assert.AreEqual(2, orderInRep.OrderItems.Count);
        }
    }
}
