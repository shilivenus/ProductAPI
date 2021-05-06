using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
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

        public Task<bool> CreateOption(ProductOption productOption)
        {
            var isSuccess = false;
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (var db = new Database(connection, DatabaseType.SQLite))
                {
                    db.BeginTransaction();
                    try
                    {
                        db.Execute($"insert into ProductOptions (Id, ProductId, Name, Description) values ('{productOption.Id}', '{productOption.ProductId}', '{productOption.Name}', '{productOption.Description}')");
                    }
                    catch (Exception e)
                    {
                        db.AbortTransaction();
                        return Task.FromResult(isSuccess);
                    }

                    db.CompleteTransaction();

                    isSuccess = true;

                    return Task.FromResult(isSuccess);
                }
            }
        }

        public Task<bool> CreateProduct(Product product)
        {
            var isSuccess = false;
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
                        return Task.FromResult(isSuccess);
                    }

                    db.CompleteTransaction();

                    isSuccess = true;

                    return Task.FromResult(isSuccess);                 
                }
            }
        }

        public Task<bool> DeleteOption(Guid productOptionId)
        {
            var isSuccess = false;
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (var db = new Database(connection, DatabaseType.SQLite))
                {
                    db.BeginTransaction();
                    try
                    {
                        db.Execute($"delete from productoptions where id = '{productOptionId}' collate nocase");
                    }
                    catch (Exception e)
                    {
                        db.AbortTransaction();
                        return Task.FromResult(isSuccess);
                    }

                    db.CompleteTransaction();

                    isSuccess = true;

                    return Task.FromResult(isSuccess);
                }
            }
        }

        public Task<bool> DeleteProduct(Product product)
        {
            var isSuccess = false;
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (var db = new Database(connection, DatabaseType.SQLite))
                {
                    db.BeginTransaction();
                    try
                    {
                        if (product.ProductOptions?.Count > 0)
                        {
                            foreach (var option in product.ProductOptions)
                            {
                                db.Execute($"delete from productoptions where id = '{option.Id}' collate nocase");
                            }
                        }

                        db.Execute($"delete from Products where id = '{product.Id}' collate nocase");
                    }
                    catch (Exception e)
                    {
                        db.AbortTransaction();
                        return Task.FromResult(isSuccess);
                    }

                    db.CompleteTransaction();

                    isSuccess = true;

                    return Task.FromResult(isSuccess);
                }
            }
        }

        public Task<List<Product>> GetAllProducts()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (var db = new Database(connection, DatabaseType.SQLite))
                {
                    var products = db.FetchOneToMany<Product>(x => x.ProductOptions,
                        "select p.*, po.* from Products p left join Productoptions po on p.Id = po.ProductId order by p.Id");

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
                    var stringId = $"'{id}'";

                    var parameters = new
                    {
                        Id = stringId.ToUpper()
                    };

                    var products = db.FetchOneToMany<Product>(x => x.ProductOptions,
                        $"select p.*, po.* from Products p left join Productoptions po on p.Id = po.ProductId where p.Id = @{nameof(parameters.Id)}", parameters);

                    return Task.FromResult(products.FirstOrDefault());
                }
            }
        }

        public Task<bool> UpdateOption(ProductOption productOption)
        {
            var isSuccess = false;
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (var db = new Database(connection, DatabaseType.SQLite))
                {
                    db.BeginTransaction();
                    try
                    {
                        db.Execute($"update productoptions set name = '{productOption.Name}', description = '{productOption.Description}' where id = '{productOption.Id}' collate nocase");
                    }
                    catch (Exception e)
                    {
                        db.AbortTransaction();
                        return Task.FromResult(isSuccess);
                    }

                    db.CompleteTransaction();

                    isSuccess = true;

                    return Task.FromResult(isSuccess);
                }
            }
        }

        public Task<bool> UpdateProduct(Product product)
        {
            var isSuccess = false;
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (var db = new Database(connection, DatabaseType.SQLite))
                {
                    db.BeginTransaction();
                    try
                    {
                        db.Execute($"update Products set name = '{product.Name}', description = '{product.Description}', price = {product.Price}, deliveryprice = {product.DeliveryPrice} where id = '{product.Id}' collate nocase");
                    }
                    catch (Exception e)
                    {
                        db.AbortTransaction();
                        return Task.FromResult(isSuccess);
                    }

                    db.CompleteTransaction();

                    isSuccess = true;

                    return Task.FromResult(isSuccess);
                }
            }
        }
    }
}
