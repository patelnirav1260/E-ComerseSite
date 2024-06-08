using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopingCartAPI.Data;
using ShopingCartAPI.Models.Dto;
using ShopingCartAPI.Models;
using ShopingCartAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;
using ShopingCartAPI.Iservices;

namespace ShopingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartApiController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ResponseDto responseDto;
        private readonly IProductService productService;
        private readonly ICouponService couponService;

        public CartApiController(AppDbContext dbContext, IMapper mapper, ResponseDto responseDto, IProductService productService, ICouponService couponService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.responseDto = responseDto;
            this.productService = productService;
            this.couponService = couponService;
        }



        [HttpGet("{userId}")]
        public async Task<ResponseDto> GetCart(string userId)
        {
            try
            {
                Cart cart = new Cart()
                {
                    CartHeader = mapper.Map<CartHeaderDto>(dbContext.cartHeaders.First(x => x.UserId == userId))
                };

                cart.CartDetails = mapper.Map<IEnumerable<CartDetailsDto>>(dbContext.cartDetails.Where(x => x.CartHeaderId == cart.CartHeader.CartHeaderId));

                IEnumerable<ProductDto> products = await productService.GetProductsAsync();

                foreach(var item in cart.CartDetails)
                {
                    item.product = products.FirstOrDefault(x => x.ProductId == item.ProductId);
                    cart.CartHeader.CartTotal += (item.Count * item.product.Price); 
                }

                //apply coupon if any
                if(!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
                {
                    CouponDto coupon = await couponService.GetCoupon(cart.CartHeader.CouponCode);
                    if(coupon != null && cart.CartHeader.CartTotal > coupon.MinAmount)
                    {
                        cart.CartHeader.CartTotal -= coupon.DiscountAmount;
                        cart.CartHeader.Discount = coupon.DiscountAmount;
                    }
                }

                responseDto.Result = cart;
            }
            catch (Exception ex)
            {
                responseDto.Message = ex.Message;
                responseDto.IsSuccess = false;
            }
            return responseDto;
        }



        [HttpPost("ApplyCoupon")]
        public async Task<ResponseDto> ApplyCoupon(Cart cart)
        {
            try
            {
                var cartFromDb = await dbContext.cartHeaders.FirstAsync(x => x.UserId == cart.CartHeader.UserId);

                cartFromDb.CouponCode = cart.CartHeader.CouponCode;

                dbContext.cartHeaders.Update(cartFromDb);

                await dbContext.SaveChangesAsync();

                responseDto.Result = true;
            }
            catch(Exception ex)
            {
                responseDto.Message = ex.Message;
                responseDto.IsSuccess = false;
            }
            return responseDto;
        }



        [HttpPost("RemoveCoupon")]
        public async Task<ResponseDto> RemoveCoupon(Cart cart)
        {
            try
            {
                var cartFromDb = await dbContext.cartHeaders.FirstAsync(x => x.UserId == cart.CartHeader.UserId);

                cartFromDb.CouponCode = "";

                dbContext.cartHeaders.Update(cartFromDb);

                await dbContext.SaveChangesAsync();

                responseDto.Result = true;
            }
            catch (Exception ex)
            {
                responseDto.Message = ex.ToString();
                responseDto.IsSuccess = false;
            }
            return responseDto;
        }


        [HttpPost("CartUpsert")]
        public async Task<ResponseDto> CartUpsert(Cart cart)
        {
            try
            {
                var cartHeaderFromDb = dbContext.cartHeaders.FirstOrDefault(x => x.UserId == cart.CartHeader.UserId);
                if (cartHeaderFromDb == null)
                {
                    //create header and details
                    CartHeader cartHeader = mapper.Map<CartHeader>(cart.CartHeader);
                    dbContext.cartHeaders.Add(cartHeader);
                    await dbContext.SaveChangesAsync();

                    cart.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                    dbContext.cartDetails.Add(mapper.Map<CartDetails>(cart.CartDetails.First()));
                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    //if header is not null
                    //check if details have same product
                    var cartDetailsFromDb = await dbContext.cartDetails.AsNoTracking().FirstOrDefaultAsync(x => x.ProductId == cart.CartDetails.First().ProductId
                        && x.CartHeaderId == cartHeaderFromDb.CartHeaderId);

                    if (cartDetailsFromDb == null)
                    {
                        //create card details
                        cart.CartDetails.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                        dbContext.cartDetails.Add(mapper.Map<CartDetails>(cart.CartDetails.First()));

                        await dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        //update count in card details
                        cart.CartDetails.First().Count += cartDetailsFromDb.Count;
                        cart.CartDetails.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                        cart.CartDetails.First().CartDetailsId = cartDetailsFromDb.CartDetailsId ;

                        dbContext.cartDetails.Update(mapper.Map<CartDetails>(cart.CartDetails.First()));
                        await dbContext.SaveChangesAsync();

                    }
                }
                responseDto.Result = cart;
            }
            catch (Exception ex)
            {
                responseDto.Message = ex.Message;
                responseDto.IsSuccess = false;
            }

            return responseDto;
        }



        [HttpPost("RemoveCart")]
        public async Task<ResponseDto> RemoveCart(int CartDetailsId)
        {
            try
            {
                var cartDetails = dbContext.cartDetails.First(x => x.CartDetailsId == CartDetailsId);

                int totalCountOfCartItem = dbContext.cartHeaders.Where(x => x.CartHeaderId == cartDetails.CartHeaderId).Count();
                dbContext.cartDetails.Remove(cartDetails);
                if(totalCountOfCartItem == 1)
                {
                    var cartHeaderToRemove = await dbContext.cartHeaders.FirstOrDefaultAsync(x => x.CartHeaderId == cartDetails.CartHeaderId);
                    dbContext.cartHeaders.Remove(cartHeaderToRemove);
                }

                await dbContext.SaveChangesAsync();

                responseDto.Result = true;
                
            }
            catch (Exception ex)
            {
                responseDto.Message = ex.Message;
                responseDto.IsSuccess = false;
            }

            return responseDto;
        }
    }
}
