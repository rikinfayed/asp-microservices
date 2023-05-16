using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data;

public class CatalogContext : ICatalogContext
{
    CatalogContext(IConfiguration configuration) {
        var client = new MongoClient(configuration.GetValue<String>("DatabaseSetting:ConnectionString"));

        var database = client.GetDatabase(configuration.GetValue<String>("DatabaseSetting:DatabaseName"));

        Products = database.GetCollection<Product>(configuration.GetValue<String>("DatabaseSetting:CollectionName"));

        CatalogContextSeed.SeedData(Products);
    }
    public IMongoCollection<Product> Products { get; }
}