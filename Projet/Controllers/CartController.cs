// Controllers/CartController.cs
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Projet.Models;
using Microsoft.AspNetCore.Authorization;


[Authorize]
public class CartController : Controller
{
    private readonly ProductContext _context;

    public CartController(ProductContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var cart = GetCart();
        ViewBag.Products = _context.Products.ToList();
        return View(cart);
    }

    public IActionResult AddToCart(int id)
    {
        var product = _context.Products.SingleOrDefault(p => p.ProductId == id);
        if (product == null)
        {
            return NotFound();
        }

        var cart = GetCart();
        var cartItem = cart.SingleOrDefault(c => c.ProductId == id);
        if (cartItem == null)
        {
            cart.Add(new CartItem { ProductId = id, Quantity = 1 });
        }
        else
        {
            cartItem.Quantity++;
        }

        SaveCart(cart);

        return RedirectToAction("Index", "Cart");
    }

    private List<CartItem> GetCart()
    {
        var cart = HttpContext.Session.GetString("Cart");
        if (cart == null)
        {
            return new List<CartItem>();
        }
        return JsonConvert.DeserializeObject<List<CartItem>>(cart);
    }

    private void SaveCart(List<CartItem> cart)
    {
        HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
    }
}
