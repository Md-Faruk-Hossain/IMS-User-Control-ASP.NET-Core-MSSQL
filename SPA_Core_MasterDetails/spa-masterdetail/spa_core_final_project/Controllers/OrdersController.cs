using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using spa_core_final_project.Models;
using spa_core_final_project.ViewModels;

namespace spa_core_final_project.Controllers
{
    public class OrdersController : Controller
    {
        private readonly SellManagementDBContext db;

        public OrdersController(SellManagementDBContext db)
        {
            this.db = db;
        }
        public ActionResult Index()
        {
            return PartialView(db.Orders.OrderByDescending(x => x.OrderId).Include(x => x.Customer).Include(x => x.OrderDetails).ThenInclude(x => x.Product).ToList());
        }
        public ActionResult Create()
        {
            ViewBag.customers = new SelectList(db.Customers, "CustomerId", "CustomerName");
            ViewBag.products = new SelectList(db.Products, "ProductId", "ProductName");

            return PartialView();
        }
        [HttpPost]
        public ActionResult Create(Order order, int[] singleProductId, int[] SingleProductQuantity)
        {
            ModelState.Remove("Customer.CustomerName");
            ModelState.Remove("Customer.CustomerAddress");
            if (ModelState.IsValid)
            {
                if (order.Customer.CustomerId != 0)
                {
                    Order or = new Order()
                    {
                        CustomerId = order.Customer.CustomerId

                    };

                    if (singleProductId != null && singleProductId.Count() > 0)
                    {
                        for (int i = 0; i < singleProductId.Length; i++)
                        {
                            if (singleProductId[i] == 0)
                                continue;

                            OrderDetail od = new OrderDetail()
                            {
                                Order = or,
                                OrderId = or.OrderId,
                                ProductId = singleProductId[i],
                                Price = db.Products.Find(singleProductId[i]).Price,
                                Quantity = SingleProductQuantity[i]
                            };
                            db.OrderDetails.Add(od);
                        }
                    }
                    db.SaveChanges();
                    return Ok("success");
                }
            }
            ViewBag.customers = new SelectList(db.Customers, "CustomerId", "CustomerName");
            ViewBag.products = new SelectList(db.Products, "ProductId", "ProductName");
            return Ok("failed");
        }
        public ActionResult Edit(int id)
        {
            //Order currentOrder = db.Orders.First(x => x.OrderId == id);

            Order currentOrder = db.Orders.Where(x => x.OrderId == id).Include(x => x.Customer).Single();
            var orderDetailsList = db.OrderDetails.Where(x => x.OrderId == currentOrder.OrderId);

            ViewBag.customers = new SelectList(db.Customers, "CustomerId", "CustomerName");
            ViewBag.products = db.Products.ToList();

            OrderVM orderVM = new OrderVM() { Order = currentOrder };

            foreach (var item in orderDetailsList)
            {
                orderVM.OrderDetails.Add(new OrderDetailVM()
                {
                    OrderDetailsId = item.OrderDetailId,
                    OrderId = item.OrderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                });
            }

            return PartialView(orderVM);
        }

        [HttpPost]
        public ActionResult Edit(OrderVM orderVM, int[] ProductId, int[] Quantity)
        {
            if (orderVM.OrderDetails != null || (ProductId != null && ProductId.Length > 0))
            {

                if (orderVM.OrderDetails.Count() > 0)
                {
                    foreach (var item in orderVM.OrderDetails)
                    {
                        OrderDetail postedOrder = new OrderDetail()
                        {
                            OrderDetailId = item.OrderDetailsId,
                            OrderId = orderVM.Order.OrderId,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            Price = db.Products.Find(item.ProductId).Price
                        };

                        if (item.Delete == true)
                        {
                            db.Entry(postedOrder).State = EntityState.Deleted;
                            continue;
                        }

                        db.Entry(postedOrder).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                }


                //Add newly added items
                if (ProductId != null && ProductId.Length > 0)
                {
                    for (int i = 0; i < ProductId.Length; i++)
                    {
                        if (ProductId[i] == 0)
                        {
                            continue;
                        }
                        OrderDetail orderDetail = new OrderDetail()
                        {
                            OrderId = orderVM.Order.OrderId,
                            ProductId = ProductId[i],
                            Quantity = Quantity[i],
                            Price = db.Products.Find(ProductId[i]).Price,
                        };

                        db.OrderDetails.Add(orderDetail); ;
                    }
                    db.SaveChanges();
                }
                return Ok("success");
            }

            return Ok("failed");
        }
        public ActionResult Delete(int id)
        {
            Order order = db.Orders.Find(id);
            if (order != null)
            {
                var orderDetail = db.OrderDetails.Where(x => x.OrderId == id).ToList();
                if (orderDetail.Count == 0)
                {
                    db.Entry(order).State = EntityState.Deleted;
                    db.SaveChanges();
                    return PartialView("_success");
                }
                if ((orderDetail != null) && (orderDetail.Count() > 0))
                {
                    foreach (var item in orderDetail)
                    {
                        db.Entry(item).State = EntityState.Deleted;
                    }
                    db.SaveChanges();

                    db.Entry(order).State = EntityState.Deleted;
                    db.SaveChanges();
                    return Ok("success");
                }
                return Ok("failed");
            }
            return Ok("failed");
        }
        public ActionResult SingleProductEntry()
        {
            ViewBag.products = new SelectList(db.Products, "ProductId", "ProductName");
            return PartialView();
        }
        public ActionResult SingleProductEditEntry()
        {
            ViewBag.customers = new SelectList(db.Customers, "CustomerId", "CustomerName");
            ViewBag.products = new SelectList(db.Products, "ProductId", "ProductName");

            return PartialView(new OrderDetail());
        }
        public JsonResult GetFee(int id)
        {
            var t = db.Products.FirstOrDefault(x => x.ProductId == id);
            return Json(t == null ? 0 : t.Price);
        }
    }
}
