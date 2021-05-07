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

        public async Task<int> CreateOption(ProductOption productOption)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (var db = new Database(connection, DatabaseType.SQLite))
                {
                    db.BeginTransaction();
                    int result;

                    try
                    {
                        result = await db.ExecuteAsync($"insert into ProductOptions (Id, ProductId, Name, Description) values ('{productOption.Id}', '{productOption.ProductId}', '{productOption.Name}', '{productOption.Description}')");
                    }
                    catch (Exception e)
                    {
                        db.AbortTransaction();
                        throw;
                    }

                    db.CompleteTransaction();

                    return result;
                }
            }
        }

        public async Task<int> CreateProduct(Product product)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (var db = new Database(connection, DatabaseType.SQLite))
                {
                    db.BeginTransaction();
                    int result;

                    try
                    {
                        result = await db.ExecuteAsync($"insert into Products (Id, Name, Description, Price, DeliveryPrice) values ('{product.Id}', '{product.Name}', '{product.Description}', {product.Price}, {product.DeliveryPrice})");

                        if(product.ProductOptions?.Count > 0)
                        {
                            foreach(var option in product.ProductOptions)
                            {
                                result += await db.ExecuteAsync($"insert into ProductOptions (Id, ProductId, Name, Description) values ('{option.Id}', '{option.ProductId}', '{option.Name}', '{option.Description}')");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        db.AbortTransaction();
                        throw;
                    }

                    db.CompleteTransaction();

                    return result;
                }
            }
        }

        public async Task<int> DeleteOption(Guid productOptionId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (var db = new Database(connection, DatabaseType.SQLite))
                {
                    db.BeginTransaction();
                    int result;

                    try
                    {
                        result = await db.ExecuteAsync($"delete from productoptions where id = '{productOptionId}' collate nocase");
                    }
                    catch (Exception e)
                    {
                        db.AbortTransaction();
                        throw;
                    }

                    db.CompleteTransaction();

                    return result;
                }
            }
        }

        public async Task<int> DeleteProduct(Product product)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (var db = new Database(connection, DatabaseType.SQLite))
                {
                    db.BeginTransaction();
                    int result = -1;

                    try
                    {
                        if (product.ProductOptions?.Count > 0)
                        {
                            foreach (var option in product.ProductOptions)
                            {
                                result = await db.ExecuteAsync($"delete from productoptions where id = '{option.Id}' collate nocase");
                            }
                        }

                        result += await db.ExecuteAsync($"delete from Products where id = '{product.Id}' collate nocase");
                    }
                    catch (Exception e)
                    {
                        db.AbortTransaction();
                        throw;
                    }

                    db.CompleteTransaction();

                    return result;
                }
            }
        }

        public List<Product> GetAllProducts()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (var db = new Database(connection, DatabaseType.SQLite))
                {
                    return db.FetchOneToMany<Product>(x => x.ProductOptions,
                        "select p.*, po.* from Products p left join Productoptions po on p.Id = po.ProductId order by p.Id");
                }
            }
        }

        public Product GetProductById(Guid id)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (var db = new Database(connection, DatabaseType.SQLite))
                {
                    return db.FetchOneToMany<Product>(x => x.ProductOptions,
                        $"select p.*, po.* from Products p left join Productoptions po on p.Id = po.ProductId where p.Id = '{id}' collate nocase").FirstOrDefault();
                }
            }
        }

        public async Task<int> UpdateOption(ProductOption productOption)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (var db = new Database(connection, DatabaseType.SQLite))
                {
                    db.BeginTransaction();
                    int result;

                    try
                    {
                        result = await db.ExecuteAsync($"update productoptions set name = '{productOption.Name}', description = '{productOption.Description}' where id = '{productOption.Id}' collate nocase");
                    }
                    catch (Exception e)
                    {
                        db.AbortTransaction();
                        throw;
                    }

                    db.CompleteTransaction();

                    return result;
                }
            }
        }

        public async Task<int> UpdateProduct(Product product)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (var db = new Database(connection, DatabaseType.SQLite))
                {
                    db.BeginTransaction();
                    int result;

                    try
                    {
                        result = await db.ExecuteAsync($"update Products set name = '{product.Name}', description = '{product.Description}', price = {product.Price}, deliveryprice = {product.DeliveryPrice} where id = '{product.Id}' collate nocase");
                    }
                    catch (Exception e)
                    {
                        db.AbortTransaction();
                        throw;
                    }

                    db.CompleteTransaction();

                    return result;
                }
            }
        }
    }
}
