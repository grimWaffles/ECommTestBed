using Google.Protobuf.WellKnownTypes;
using OrderServiceGrpc.Models;
using OrderServiceGrpc.Protos;

namespace OrderServiceGrpc.Helpers.cs
{
    public static class OrderItemMapper
    {
        public static OrderItemModel ToModel(OrderItem proto)
        {
            return new OrderItemModel
            {
                Id = proto.Id,
                OrderId = proto.OrderId,
                ProductId = proto.ProductId,
                Quatity = proto.Quantity,
                GrossAmount = (decimal)proto.GrossAmount,
                Status = proto.Status,
                CreatedBy = proto.CreatedBy,
                CreatedDate = proto.CreatedDate.ToDateTime(),
                ModifiedBy = proto.ModifiedBy,
                ModifiedDate = proto.ModifiedDate.ToDateTime(),
                IsDeleted = proto.IsDeleted
            };
        }

        public static OrderItem ToProto(OrderItemModel model)
        {
            return new OrderItem
            {
                Id = model.Id,
                OrderId = model.OrderId,
                ProductId = model.ProductId,
                Quantity = model.Quatity,
                GrossAmount = (double)model.GrossAmount,
                Status = model.Status ?? string.Empty,
                CreatedBy = model.CreatedBy,
                CreatedDate = Timestamp.FromDateTime(model.CreatedDate.ToUniversalTime()),
                ModifiedBy = model.ModifiedBy,
                ModifiedDate = Timestamp.FromDateTime(model.ModifiedDate.ToUniversalTime()),
                IsDeleted = model.IsDeleted
            };
        }
    }
}
