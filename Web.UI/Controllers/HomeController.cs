using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using Web.UI.Iservice;
using Web.UI.Models;
using Web.UI.Models.Dto;

namespace Web.UI.Controllers
{
    public class HomeController : Controller
    {
    
        private readonly IProductService productService;
        private readonly ICartService cartService;

        public HomeController(IProductService productService, ICartService cartService)
        {
            this.productService = productService;
            this.cartService = cartService;
        }

        public async Task<ActionResult<List<Product>>> Index()
        {
            List<Product?> products = new();

            var response = await productService.GetAllProductAsync();

            if (response != null && response.IsSuccess)
            {
                string? result = Convert.ToString(response.Result);
                products = JsonConvert.DeserializeObject<List<Product>>(result);
                return View(products);
            }

            TempData["error"] = response.Message;
            return View(products);

        }


        [HttpGet("ProductDetails/{ProductId}")]
        [Authorize]
        public async Task<ActionResult<ProductDto>> ProductDetails(int ProductId)
        {
            ProductDto productDto = new();

            var response = await productService.GetProductByIdAsync(ProductId);

            if (response != null && response.IsSuccess)
            {
                string? result = Convert.ToString(response.Result);
                var product = JsonConvert.DeserializeObject<Product>(result);

                productDto.Name = product.Name;
                productDto.Description = product.Description;
                productDto.Price = product.Price;
                productDto.ImageUrl = product.ImageUrl;
                productDto.CategoryName = product.CategoryName;
    
                return View(productDto);
            }

            TempData["error"] = response.Message;
            return View(productDto);

        }


        [HttpPost("AddToCart")]
        [Authorize]
        public async Task<ActionResult<ProductDto>> AddToCart(ProductDto productDto)
        {

            CartDto cartDto = new CartDto()
            {
                CartHeader = new CartHeader()
                {
                    UserId = User.Claims.Where(x => x.Type == JwtClaimTypes.Subject)?.FirstOrDefault()?.Value
                }
            };

            CartDetails cartDetails = new CartDetails()
            {
                Count = (int)productDto.Count,
                ProductId = productDto.ProductId,

            };

            List<CartDetails> cartDetailsList = new() { cartDetails };
            cartDto.CartDetails = cartDetailsList;
            var response = await cartService.AddToCartAsync(cartDto);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Item added to a shoping cart";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = response.Message;
            return View(productDto);

        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}