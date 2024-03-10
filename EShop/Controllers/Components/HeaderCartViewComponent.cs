using EShop.Extension;
using EShop.ModelViews;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Controllers.Components
{
    public class HeaderCartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            return View(cart);
        }
    }
}