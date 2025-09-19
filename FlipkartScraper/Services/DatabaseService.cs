using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using FlipkartScraper.Models;

namespace FlipkartScraper.Services
{
    /// <summary>
    /// Service class for database operations using ADO.NET
    /// </summary>
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// Test database connection      
        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    return connection.State == ConnectionState.Open;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database connection error: {ex.Message}");
                return false;
            }
        }

        /// Insert product data using stored procedure
        public async Task<int> InsertProductAsync(Product product)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("InsertProductData", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters
                        command.Parameters.AddWithValue("@Name", product.Name ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Price", product.Price ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Rating", product.Rating ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Description", product.Description ?? (object)DBNull.Value);

                        // Execute and get the new ProductID
                        var result = await command.ExecuteScalarAsync();
                        return Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting product: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Get all products using stored procedure
        /// </summary>
        public async Task<List<Product>> GetAllProductsAsync()
        {
            var products = new List<Product>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("GetAllProducts", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                products.Add(new Product
                                {
                                    ProductID = reader.GetInt32("ProductID"),
                                    Name = reader.IsDBNull("Name") ? string.Empty : reader.GetString("Name"),
                                    Price = reader.IsDBNull("Price") ? string.Empty : reader.GetString("Price"),
                                    Rating = reader.IsDBNull("Rating") ? string.Empty : reader.GetString("Rating"),
                                    Description = reader.IsDBNull("Description") ? string.Empty : reader.GetString("Description"),
                                    CreatedDate = reader.GetDateTime("CreatedDate"),
                                    UpdatedDate = reader.GetDateTime("UpdatedDate")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving products: {ex.Message}");
                throw;
            }

            return products;
        }

        /// <summary>
        /// Get product by ID using stored procedure
        /// </summary>
        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("GetProductById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ProductID", productId);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new Product
                                {
                                    ProductID = reader.GetInt32("ProductID"),
                                    Name = reader.IsDBNull("Name") ? string.Empty : reader.GetString("Name"),
                                    Price = reader.IsDBNull("Price") ? string.Empty : reader.GetString("Price"),
                                    Rating = reader.IsDBNull("Rating") ? string.Empty : reader.GetString("Rating"),
                                    Description = reader.IsDBNull("Description") ? string.Empty : reader.GetString("Description"),
                                    CreatedDate = reader.GetDateTime("CreatedDate"),
                                    UpdatedDate = reader.GetDateTime("UpdatedDate")
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving product by ID: {ex.Message}");
                throw;
            }

            return null;
        }
    }
}
