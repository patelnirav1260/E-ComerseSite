using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Web.UI.Iservice;
using Web.UI.Models;
using Web.UI.Models.Dto;
using Web.UI.Service;

namespace Web.UI.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService productService;
        private readonly IWebHostEnvironment env;
       
        public ProductController(IProductService productService, IWebHostEnvironment env)
        {
            this.productService = productService;
            this.env = env;
        }

        public async Task<ActionResult<List<Product>>> ProductIndex()
        {
            List<Product?> products = new();

            ResponseDto response = await productService.GetAllProductAsync();

            if (response != null && response.IsSuccess)
            {
                string? result = Convert.ToString(response.Result);
                products = JsonConvert.DeserializeObject<List<Product>>(result);
                return View(products);
            }

            TempData["error"] = response.Message;
            return View(products);

        }
        [HttpGet]
        public async Task<ActionResult> CreateProduct()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateProduct(ProductDto productDto)
        {

                Product product = AddImage(productDto);

                ResponseDto responseDto = await productService.CreateProductAsync(product);

            if(responseDto.IsSuccess)
            {
                TempData["success"] = "Product is successfully added";
                return RedirectToAction("ProductIndex", "Product");

            }

            TempData["error"] = responseDto.Message;
            return View(product);
        }

        [HttpGet]
        [Route("/DeleteProduct/{ProductId}")]
        public async Task<ActionResult> DeleteProduct(int ProductId)
        {
            ResponseDto responseDto = await productService.GetProductByIdAsync(ProductId);
            if (responseDto.IsSuccess)
            {
                string? result = Convert.ToString(responseDto.Result);
                Product? product = JsonConvert.DeserializeObject<Product>(result);

                return View(product);
            }
            else
            {
                TempData["error"] = responseDto.Message;
                return View();
            }
        }

        [HttpPost()]
        [Route("/DeleteConform/{ProductId}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConform(int ProductId)
        {
            ResponseDto responseDto = await productService.DeleteProductAsync(ProductId);
            if (responseDto.IsSuccess)
            {
                DeleteImage(responseDto);
                TempData["success"] = "your Product successfully deleted";
                return RedirectToAction("ProductIndex", "Product");
            }
            else
            {
                TempData["error"] = responseDto.Message;
                return RedirectToAction("ProductIndex", "Product");
            }
        }

        [HttpGet()]
        [Route("/EditProduct/{ProductId}")]
        public async Task<ActionResult> EditProduct(int ProductId)
        {
            ResponseDto responseDto = await productService.GetProductByIdAsync(ProductId);
            if (responseDto.IsSuccess)
            {
                string? result = Convert.ToString(responseDto.Result);
                Product? product = JsonConvert.DeserializeObject<Product>(result);

                ProductDto productDto = new ProductDto()
                {
                    ProductId = product.ProductId,
                    Name = product.Name,
                    CategoryName = product.CategoryName,
                    Description = product.Description,
                    Price = product.Price,
                };

                return View(productDto);
            }
            else
            {
                TempData["error"] = responseDto.Message;
                return RedirectToAction("ProductIndex", "Product");
            }
        }


        [HttpPost("{ProductId}")]
        [Route("/EditProduct/{ProductId}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditProduct(ProductDto productDto, int ProductId)
        {
            ResponseDto response = await productService.GetProductByIdAsync(ProductId);

            if (!response.IsSuccess)
            {
                TempData["error"] = "Product not found";
                return View(productDto);
            }

            DeleteImage(response);
            Product product = AddImage(productDto);
            var responseDto = await productService.UpdateProductAsync(product,ProductId);

            if (responseDto.IsSuccess)
            {
                TempData["success"] = "Product is successfully updated";
                return RedirectToAction("ProductIndex");
            }

            TempData["error"] = responseDto.Message;
            return View(productDto);

        }






        //helper methods

        private Product AddImage(ProductDto productDto)
        {
            string fileName = "";

            string folder = Path.Combine(env.WebRootPath, "Images");
            fileName = Guid.NewGuid().ToString() + "_" + productDto.ImageFile.FileName;
            string filePath = Path.Combine(folder, fileName);
            productDto.ImageFile.CopyTo(new FileStream(filePath, FileMode.Create));

            Product product = new Product()
            {
                ProductId = productDto.ProductId,
                Name = productDto.Name,
                CategoryName = productDto.CategoryName,
                Description = productDto.Description,
                Price = productDto.Price,
                ImageUrl = fileName,
            };

            return product;
        }


        private void DeleteImage(ResponseDto response)
        {
            if (response.IsSuccess)
            {
                string? result = Convert.ToString(response.Result);
                Product? product = JsonConvert.DeserializeObject<Product>(result);

                var imagePath = Path.Combine(env.WebRootPath, "Images", product.ImageUrl);

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                } 
            }

        }

    }
}
