using Entities.ViewModels.Products;
using MongoDB.Driver;

namespace Web.CMS.Service.Product
{
    public class OrderMongoAccess
    {
        private readonly IConfiguration _configuration;
        private IMongoCollection<OrderDetailMongoDbModel> _OrderDetailCollection;

        public OrderMongoAccess(IConfiguration configuration)
        {
            _configuration = configuration;
            //mongodb://adavigolog_writer:adavigolog_2022@103.163.216.42:27017/?authSource=HoanBds
            string url = "mongodb://" + configuration["DataBaseConfig:MongoServer:user"] +
                ":" + configuration["DataBaseConfig:MongoServer:pwd"] +
                "@" + configuration["DataBaseConfig:MongoServer:Host"] +
                ":" + configuration["DataBaseConfig:MongoServer:Port"] +
                "/?authSource=" + configuration["DataBaseConfig:MongoServer:catalog"] + "";

            var client = new MongoClient(url);
            IMongoDatabase db = client.GetDatabase(configuration["DataBaseConfig:MongoServer:catalog"]);
            _OrderDetailCollection = db.GetCollection<OrderDetailMongoDbModel>("Orders");
        }
        public async Task<OrderDetailMongoDbModel> GetByOrderNo(string order_no)
        {
            try
            {
                var filter = Builders<OrderDetailMongoDbModel>.Filter;
                var filterDefinition = filter.Empty;
                filterDefinition &= Builders<OrderDetailMongoDbModel>.Filter.Eq(x => x.order_no, order_no); ;
                var model = await _OrderDetailCollection.Find(filterDefinition).FirstOrDefaultAsync();
                return model;
            }
            catch (Exception ex)
            {
                Utilities.LogHelper.InsertLogTelegram("ProductDetailMongoAccess - GetByID Error: " + ex);
                return null;
            }
        }
    }
}
