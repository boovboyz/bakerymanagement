using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using bakerymanagement.Models;

namespace bakerymanagement.Data
{
    public class BakeryContext
    {
        private readonly string _connectionString;

        public BakeryContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// Retrieves the current inventory from the database.
        /// </summary>
        /// <returns>A list of products with their quantities.</returns>
        public List<Product> GetInventory()
        {
            var products = new List<Product>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetInventory", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        ProductId = (int)reader["ProductId"],
                        ProductName = (string)reader["ProductName"],
                        Quantity = (int)reader["Quantity"]
                    });
                }
            }

            return products;
        }

        /// <summary>
        /// Updates the inventory by changing the quantity of a specific product.
        /// </summary>
        /// <param name="userName">The name of the user making the change.</param>
        /// <param name="productId">The ID of the product to update.</param>
        /// <param name="quantityChanged">The amount by which to change the quantity.</param>
        public void UpdateInventory(string userName, int productId, int quantityChanged)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("UpdateInventory", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@UserName", userName);
                cmd.Parameters.AddWithValue("@ProductId", productId);
                cmd.Parameters.AddWithValue("@QuantityChanged", quantityChanged);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Retrieves the list of all transactions from the database.
        /// </summary>
        /// <returns>A list of transactions.</returns>
        public List<Transaction> GetTransactions()
        {
            var transactions = new List<Transaction>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetTransactions", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    transactions.Add(new Transaction
                    {
                        TransactionId = (int)reader["TransactionId"],
                        User = new User { UserName = (string)reader["UserName"] },
                        Product = new Product { ProductName = (string)reader["ProductName"] },
                        QuantityChanged = (int)reader["QuantityChanged"],
                        TransactionDate = (DateTime)reader["TransactionDate"]
                    });
                }
            }

            return transactions;
        }

        /// <summary>
        /// Retrieves the leaderboard data from the database.
        /// </summary>
        /// <returns>A list of users with their total added and removed quantities.</returns>
        public List<UserLeaderboard> GetLeaderboard()
        {
            var leaderboard = new List<UserLeaderboard>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetLeaderboard", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    leaderboard.Add(new UserLeaderboard
                    {
                        UserName = (string)reader["UserName"],
                        TotalAdded = (int)reader["TotalAdded"],
                        TotalRemoved = (int)reader["TotalRemoved"]
                    });
                }
            }

            return leaderboard;
        }

        /// <summary>
        /// Resets the inventory and transaction history for the day.
        /// </summary>
        public void ResetInventory()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("ResetInventory", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
