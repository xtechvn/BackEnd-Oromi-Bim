using Entities.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels.Products
{
    public class OrderDetailMongoDbModel
    {
        [BsonElement("_id")]
        public string _id { get; set; }
        public void GenID()
        {
            _id = ObjectId.GenerateNewId(DateTime.Now).ToString();
        }
        public long account_client_id { get; set; }
        public int payment_type { get; set; }
        public int delivery_type { get; set; }
        public long order_id { get; set; }
        public string order_no { get; set; }

        public double total_amount { get; set; }
        public double? total_price { get; set; }
        public double? total_profit { get; set; }
        public double? total_discount { get; set; }
        public List<CartItemMongoDbModel> carts { get; set; }
        public string utm_source { get; set; }
        public string utm_medium { get; set; }

        public int voucher_id { get; set; }
        public string receivername { get; set; }

        public string phone { get; set; }

        public string? provinceid { get; set; }

        public string? districtid { get; set; }

        public string? wardid { get; set; }

        public string address { get; set; }
        public long address_id { get; set; }

        public Order order { get; set; }
        public List<OrderDetail> order_detail { get; set; }
        public string note { get; set; }

    }
    public class CartItemMongoDbModel
    {
        [BsonElement("_id")]
        public string _id { get; set; }
        public void GenID()
        {
            _id = ObjectId.GenerateNewId(DateTime.Now).ToString();
        }
        public long account_client_id { get; set; }
        public int quanity { get; set; }
        public double? total_price { get; set; }
        public double? total_profit { get; set; }
        public double? total_discount { get; set; }
        public double total_amount { get; set; }
        public DateTime created_date { get; set; }
        public ProductMongoDbModel product { get; set; }
    }
    public partial class OrderDetail
    {
        public long OrderDetailId { get; set; }

        public long OrderId { get; set; }

        public string ProductId { get; set; } = null!;

        public string? ProductCode { get; set; }

        public double? Amount { get; set; }

        public double? Price { get; set; }

        public double? Profit { get; set; }

        public double? Discount { get; set; }

        public int? Quantity { get; set; }

        public double? TotalPrice { get; set; }

        public double? TotalProfit { get; set; }

        public double? TotalDiscount { get; set; }

        public double? TotalAmount { get; set; }

        public string? ProductLink { get; set; }

        public int? UserCreate { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? UserUpdated { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
