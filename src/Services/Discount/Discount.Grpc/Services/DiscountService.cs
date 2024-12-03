using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services
{
    public class DiscountService(DiscountContext discountContext,ILogger <DiscountService> logger)
        :DiscountProtoService.DiscountProtoServiceBase
    {
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon=await discountContext.Coupons.FirstOrDefaultAsync(x=>x.ProductName == request.ProductName);
            if (coupon == null) { 
                coupon=new Coupon { ProductName = "No Discount",Amount=0,Description="No Discount" };
            }

            logger.LogInformation("Discount is retrieved for productNme : {productname} ,Amount: {Amount}",coupon.ProductName,coupon.Amount);
            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();
            if (coupon == null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));
           discountContext.Coupons.Add(coupon);
            await discountContext.SaveChangesAsync();
            logger.LogInformation("Product successfully created productName : {productName}", coupon.ProductName);
            var couponModel=coupon.Adapt<CouponModel>();    

            return couponModel; 
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = await discountContext.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

            if (coupon == null)
                throw new RpcException(new Status(StatusCode.NotFound, $"product not found :{request.ProductName}"));
            discountContext.Coupons.Remove(coupon);
            await discountContext.SaveChangesAsync();
            logger.LogInformation("Product successfully deleted productName : {productName}", coupon.ProductName);
          
            return new DeleteDiscountResponse { Success = true };
        }

        public override  async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();
            if (coupon == null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));
            discountContext.Coupons.Update(coupon);
            await discountContext.SaveChangesAsync();
            logger.LogInformation("Product successfully updated productName : {productName}", coupon.ProductName);
            var couponModel = coupon.Adapt<CouponModel>();

            return couponModel;
        }
    }
}
