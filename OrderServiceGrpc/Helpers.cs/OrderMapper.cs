using Google.Protobuf.WellKnownTypes;
using Microsoft.IdentityModel.Tokens;
using OrderServiceGrpc.Models;
using OrderServiceGrpc.Protos;

namespace OrderServiceGrpc.Helpers.cs
{
    public static class OrderMapper
    {
        public static OrderModel ToModel(Order proto)
        {
            return new OrderModel
            {
                Id = proto.Id,
                OrderDate = proto.OrderDate.ToDateTime(),
                OrderCounter = proto.OrderCounter,
                UserId = proto.UserId,
                Status = proto.Status,
                NetAmount = (decimal)proto.NetAmount,
                CreatedBy = proto.CreatedBy,
                CreatedDate = proto.CreatedDate.ToDateTime(),
                ModifiedBy = proto.ModifiedBy,
                ModifiedDate = proto.ModifiedDate.ToDateTime(),
                IsDeleted = proto.IsDeleted,
                OrderItems = !proto.Items.IsNullOrEmpty() ? new List<OrderItemModel>() : proto.Items.Select(OrderItemMapper.ToModel).ToList()
            };
        }

        public static Order ToProto(OrderModel model)
        {
            var proto = new Order
            {
                Id = model.Id,
                OrderDate = Timestamp.FromDateTime(model.OrderDate.ToUniversalTime()),
                OrderCounter = model.OrderCounter,
                UserId = model.UserId,
                Status = model.Status ?? string.Empty,
                NetAmount = (double)model.NetAmount,
                CreatedBy = model.CreatedBy,
                CreatedDate = Timestamp.FromDateTime(model.CreatedDate.ToUniversalTime()),
                ModifiedBy = model.ModifiedBy,
                ModifiedDate = Timestamp.FromDateTime(model.ModifiedDate.ToUniversalTime()),
                IsDeleted = model.IsDeleted,
            };

            if (!model.OrderItems.IsNullOrEmpty())
            {
                proto.Items.AddRange(model.OrderItems.Select(OrderItemMapper.ToProto));
            }

            return proto;
        }
    }
}
