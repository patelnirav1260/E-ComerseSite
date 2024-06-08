using AutoMapper;
using CoupenAPI.Data;
using CoupenAPI.Models;
using CoupenAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoupenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ResponseDto responseDto;

        public CouponAPIController(AppDbContext dbContext, IMapper mapper, ResponseDto responseDto)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.responseDto = responseDto;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto>> Get()
        {
            if(dbContext.coupons == null)
            {
                responseDto.IsSuccess = true;
                responseDto.Message = "No Coupon is available";
                return Ok(responseDto);
            }

            var coupons = await dbContext.coupons.ToListAsync();
            List<CouponDto> couponDtos = mapper.Map<List<CouponDto>>(coupons);
            responseDto.Result = couponDtos;
            return Ok(responseDto);
        }
    

        [HttpGet("code/{code}")]
     
        public async Task<ActionResult<ResponseDto>> GetByCode(string code)
        {
            if (dbContext.coupons == null || code == null)
            {
                return NoContent();
            }

            var coupons = await dbContext.coupons.FirstOrDefaultAsync(x => x.CouponCode.ToLower() == code.ToLower());
            CouponDto couponDtos = mapper.Map<CouponDto>(coupons);
            responseDto.Result = couponDtos;
            return Ok(responseDto);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseDto>> Get(int id)
        {
            var coupon = await dbContext.coupons.FirstOrDefaultAsync(x => x.CouponId == id);
            if(coupon == null)
            {
                return NotFound();
            }
            CouponDto couponDto = mapper.Map<CouponDto>(coupon);
            responseDto.Result = couponDto;
            return Ok(responseDto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseDto>> Post(CouponDto couponDto)
        {

            try
            {
                if (couponDto == null)
                {
                    return BadRequest();
                }
                Coupon coupon = mapper.Map<Coupon>(couponDto);
                await dbContext.coupons.AddAsync(coupon);
                await dbContext.SaveChangesAsync();

                responseDto.Result = coupon;
                return Ok(responseDto);
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
        public async Task<ActionResult<ResponseDto>> Put(CouponDto couponDto, int id) 
        {

            try
            {
                if (id != couponDto.CouponId)
                {
                    return BadRequest();
                }

                var cp = await dbContext.coupons.FindAsync(id);
                if (cp == null)
                {
                    return NotFound(couponDto.CouponId);
                }
                Coupon coupon = mapper.Map<Coupon>(couponDto);
                dbContext.Entry(cp).CurrentValues.SetValues(coupon);
                await dbContext.SaveChangesAsync();

                responseDto.Result = coupon;

                return Ok(responseDto);
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
        public async Task<ActionResult<ResponseDto>> Delete(int id)
        {
            try
            {
                var coupon = await dbContext.coupons.FindAsync(id);

                if (coupon == null)
                {
                    return BadRequest();
                }

                dbContext.coupons.Remove(coupon);
                await dbContext.SaveChangesAsync();
                responseDto.Result = coupon;
                return Ok(responseDto);
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
