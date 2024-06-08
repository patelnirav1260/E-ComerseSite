using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Web.UI.Iservice;
using Web.UI.Models;
using Web.UI.Models.Dto;

namespace Web.UI.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService couponService;

        public CouponController(ICouponService couponService)
        {
            this.couponService = couponService;
        }

        public async Task<ActionResult<List<Coupon>>> CouponIndex()
        {
            List<Coupon?> coupons = new();

            ResponseDto response = await couponService.GetAllCouponAsync();

            if (response != null && response.IsSuccess)
            {
                string? result = Convert.ToString(response.Result);
                coupons = JsonConvert.DeserializeObject<List<Coupon>>(result);
                return View(coupons);
            }

            TempData["error"] = response.Message;
            return View(coupons);

        }
        [HttpGet]
        public async Task<ActionResult> CreateCoupon()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateCoupon(Coupon coupon)
        {
            ResponseDto responseDto = await couponService.CreateCouponAsync(coupon);

            if(responseDto.IsSuccess)
            {
                TempData["success"] = "Coupon is successfully added";
                return RedirectToAction("CouponIndex", "Coupon");

            }

            TempData["error"] = responseDto.Message;
            return View(coupon);
        }

        [HttpGet("{CouponId}")]
        public async Task<ActionResult> DeleteCoupon(int CouponId)
        {
            ResponseDto responseDto = await couponService.GetCouponByIdAsync(CouponId);
            if (responseDto.IsSuccess)
            {
                string? result = Convert.ToString(responseDto.Result);
                Coupon? coupon = JsonConvert.DeserializeObject<Coupon>(result);

                return View(coupon);
            }
            else
            {
                TempData["error"] = responseDto.Message;
                return View();
            }
        }

        [HttpPost("{CouponId}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConform(int Couponid)
        {
            ResponseDto responseDto = await couponService.DeleteCouponAsync(Couponid);
            if (responseDto.IsSuccess)
            {
                TempData["success"] = "your coupon successfully deleted";
                return RedirectToAction("CouponIndex", "Coupon");
            }
            else
            {
                TempData["error"] = responseDto.Message;
                return RedirectToAction("CouponIndex", "Coupon");
            }
        }


    }
}
