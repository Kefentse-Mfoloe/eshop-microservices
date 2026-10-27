using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services
{
    public class DiscountService(DiscountContext dbContext, ILogger<DiscountService> logger) : DiscountProtoService.DiscountProtoServiceBase
    {
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbContext.Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName);

            if(coupon == null)
            {
                coupon = new Coupon() { ProductName = "No discount", Amount = 0 , Description = "No discount available" };
            }

            logger.LogInformation($"Discount retrieved for ProductName: {coupon.ProductName}, Amount: {coupon.Amount}");
            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid create request object"));
            }

            await dbContext.Coupons.AddAsync(coupon);
            await dbContext.SaveChangesAsync();
            logger.LogInformation($"Discount created for ProductName: {coupon.ProductName}, Amount: {coupon.Amount}");

            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid update request object"));
            }

            dbContext.Update(coupon);
            await dbContext.SaveChangesAsync();

            logger.LogInformation($"Discount updated for ProductName: {coupon.ProductName}, Amount: {coupon.Amount}");
            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = dbContext.Coupons.FirstOrDefault(c => c.ProductName == request.ProductName);
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} not found. Cannot delete discount."));
            }

            dbContext.Coupons.Remove(coupon);
            await dbContext.SaveChangesAsync();

            logger.LogInformation($"Discount deleted for ProductName: {coupon.ProductName}");
            var response = new DeleteDiscountResponse { Success = true };
            return response;
        }
    }
}
