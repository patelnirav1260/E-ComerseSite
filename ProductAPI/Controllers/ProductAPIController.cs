using AutoMapper;
using ProductAPI.Data;
using ProductAPI.Models;
using ProductAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Server.IIS.Core;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ResponseDto responseDto;

        public ProductAPIController(AppDbContext dbContext, IMapper mapper, ResponseDto responseDto)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.responseDto = responseDto;
        }

        [HttpGet]
        public async Task<ResponseDto> Get()
        {
            if(dbContext.products == null)
            {
                responseDto.IsSuccess = true;
                responseDto.Message = "No Product is available";
                return responseDto;
            }

            var products = await dbContext.products.ToListAsync();
            List<ProductDto> productDtos = mapper.Map<List<ProductDto>>(products);
            responseDto.Result = productDtos;
            return responseDto;
        }
    

        [HttpGet("{id}")]
        public async Task<ResponseDto> Get(int id)
        {
            var product = await dbContext.products.FirstOrDefaultAsync(x => x.ProductId == id);
            if(product == null)
            {
                 throw new Exception("No Product available for your requested id");
            }
            ProductDto productDto = mapper.Map<ProductDto>(product);
            responseDto.Result = productDto;
            return responseDto;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ResponseDto> Post(ProductDto productDto)
        {

            try
            {
                Product product = mapper.Map<Product>(productDto);
                await dbContext.products.AddAsync(product);
                await dbContext.SaveChangesAsync();

                responseDto.Result = product;
                return responseDto;
            }
            catch (Exception ex)
            {
                responseDto.Message = ex.Message;
                responseDto.IsSuccess = false;
            }
            return responseDto;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ResponseDto> Put(ProductDto productDto, int id) 
        {

            try
            {
                if (id != productDto.ProductId)
                {
                    responseDto.IsSuccess = false;
                    var msg = "bad request";
                     throw new Exception(msg);
                }

                var cp = await dbContext.products.FindAsync(id);
                if (cp == null)
                {
                    throw new Exception("your request to update is not found");
                }
                Product product = mapper.Map<Product>(productDto);
                dbContext.Entry(cp).CurrentValues.SetValues(product);
                await dbContext.SaveChangesAsync();

                responseDto.Result = product;

                return responseDto;
            }
            catch (Exception ex)
            {
                responseDto.Message = ex.Message;
                responseDto.IsSuccess = false;
            }
            return responseDto;
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ResponseDto> Delete(int id)
        {
            try
            {
                var product = await dbContext.products.FindAsync(id);

                if (product == null)
                {
                    throw new Exception("your request to delete coupon is not available");
                }

                dbContext.products.Remove(product);
                await dbContext.SaveChangesAsync();
                responseDto.Result = product;
                return responseDto;
            }
            catch (Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.Message = ex.Message;
            }
            return responseDto;
        }
    }
}
