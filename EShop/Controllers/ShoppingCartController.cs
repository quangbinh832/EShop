using AspNetCoreHero.ToastNotification.Abstractions;
using EShop.Extension;
using EShop.Models;
using EShop.ModelViews;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiChoSaiGon.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly dbEShopContext _context;
        public INotyfService _notyfService { get; }

        public ShoppingCartController(dbEShopContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        //Tạo giỏ hàng rỗng
        public List<CartItem> GioHang
        {
            get
            {
                var gh = HttpContext.Session.Get<List<CartItem>>("GioHang");
                if (gh == default(List<CartItem>))
                {
                    gh = new List<CartItem>();
                }
                return gh;
            }
        }

        /*
                1. Thêm mới sản phẩm vào giỏ hàng
                2. Cập nhật lại số lượng sản phẩm trong giỏ hàng
                3. Xóa sản phẩm khỏi giỏ hàng
                4. Xóa luôn giỏ hàng

         */

        //thêm mới sản phẩm vào trong giỏ hàng
        [HttpPost]
        [Route("api/cart/add")]
        public IActionResult AddToCart(int productID, int? amount)
        {
            List<CartItem> cart = GioHang;

            try
            {
                //Thêm sản phẩm vào giỏ hàng
                CartItem item = GioHang.SingleOrDefault(p => p.product.ProductId == productID);
                if (item != null) //đã có --> cập nhật số lượng
                {
                    item.amount = item.amount + amount.Value;
                    //Lưu lại session
                    HttpContext.Session.Set<List<CartItem>>("GioHang", cart);
                }
                else
                {
                    Product hh = _context.Products.SingleOrDefault(p => p.ProductId == productID);
                    item = new CartItem
                    {
                        amount = amount.HasValue ? amount.Value : 1,
                        product = hh
                    };
                    cart.Add(item); // thêm vào giỏ
                }
                //lưu lại session
                HttpContext.Session.Set<List<CartItem>>("GioHang", cart);
                _notyfService.Success("Thêm sản phẩm thành công");
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        //cập nhật lại số lượng sản phẩm trong giỏ hàng
        [HttpPost]
        [Route("api/cart/update")]
        public IActionResult UpdateCart(int productID, int? amount)
        {
            //lấy giỏ hàng ra để xử lý
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            try
            {
                if (cart != null)
                {
                    CartItem item = cart.SingleOrDefault(p => p.product.ProductId == productID);
                    if (item != null && amount.HasValue) //đã có --> cập nhật số lượng
                    {
                        item.amount = amount.Value;
                    }
                    //lưu lại session
                    HttpContext.Session.Set<List<CartItem>>("GioHang", cart);
                }
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        //xóa sản phẩm khỏi giỏ hàng
        [HttpPost]
        [Route("api/cart/remove")]
        public ActionResult Remove(int productID)
        {
            try
            {
                List<CartItem> gioHang = GioHang;
                CartItem item = gioHang.SingleOrDefault(p => p.product.ProductId == productID);
                if (item != null)
                {
                    gioHang.Remove(item);
                }
                //lưu lại session
                HttpContext.Session.Set<List<CartItem>>("GioHang", gioHang);
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        [Route("cart.html", Name = "cart")]
        public IActionResult Index()
        {
            return View(GioHang);
        }
    }
}