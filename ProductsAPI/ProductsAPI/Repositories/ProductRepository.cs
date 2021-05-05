using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NPoco;
using ProductsAPI.Interface;
using ProductsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ProductDB");
        }

        public Task CreateProduct(Product product)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (var db = new Database(connection, DatabaseType.SQLite))
                {
                    db.BeginTransaction();
                    try
                    {
                        db.Execute($"insert into Products (Id, Name, Description, Price, DeliveryPrice) values ('{product.Id}', '{product.Name}', '{product.Description}', {product.Price}, {product.DeliveryPrice})");

                        if(product.ProductOptions?.Count > 0)
                        {
                            foreach(var option in product.ProductOptions)
                            {
                                db.Execute($"insert into ProductOptions (Id, ProductId, Name, Description) values ('{option.Id}', '{option.ProductId}', '{option.Name}', '{option.Description}')");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        db.AbortTransaction();
                        return Task.FromResult(product);
                    }

                    db.CompleteTransaction();

                    return Task.FromResult(product);                 
                }
            }
        }

        public Task DeleteProduct(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Product>> GetAllProducts()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (var db = new Database(connection, DatabaseType.SQLite))
                {
                    var products = db.FetchOneToMany<Product>(x => x.ProductOptions,
                        "select p.*, po.* from Products p inner join Productoptions po on p.Id = po.ProductId order by p.Id");

                    return Task.FromResult(products);
                }
            }
        }

        public Task<Product> GetProductById(Guid id)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (var db = new Database(connection, DatabaseType.SQLite))
                {
                    var products = db.FetchOneToMany<Product>(x => x.ProductOptions,
                        $"select p.*, po.* from Products p inner join Productoptions po on p.Id = po.ProductId where p.Id = '{id}' collate nocase");

                    return Task.FromResult(products.FirstOrDefault());
                }
            }
        }

        public Task UpdateProduct(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
