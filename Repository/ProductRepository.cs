using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;


namespace adonet
{
    // The class must implement this interface, do not change.
    public class ProductRepository : IProductRepository
    {
        private readonly string connectionString;

        // Keep the constructor and the member as-is.
        public ProductRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IReadOnlyList<Product> Search(string name)
        {
            var result = new List<Product>();
            SqlCommand command;

            using (var conn = new SqlConnection(connectionString))

            {
                if (name == null)
                {
                    command = new SqlCommand("SELECT Product.ID,Product.Name,Product.Price,Product.Stock,VAT.Percentage,Category.Name as catName" +
                       " FROM(( Product " +
                       "JOIN VAT ON Product.VATID = VAT.ID ) " +
                       "JOIN Category ON Product.CATEGORYID = Category.ID)", conn);
                }
                else
                {
                    command = new SqlCommand("SELECT Product.ID,Product.Name,Product.Price,Product.Stock,VAT.Percentage,Category.Name as catName" +
                   " FROM(( Product " +
                   "JOIN VAT ON Product.VATID = VAT.ID ) " +
                   "JOIN Category ON Product.CATEGORYID = Category.ID)" +
                   "WHERE lower(Product.Name) LIKE @NAME", conn);
                    command.Parameters.Add("@NAME", SqlDbType.NVarChar);
                    command.Parameters["@NAME"].Value = '%' + name.ToLower() + '%';

                }
                conn.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Product NewProduct = new Product((int)reader["ID"], (String)reader["Name"], (Double)reader["Price"], (int)reader["Stock"], (int)reader["Percentage"], (string)reader["catName"]);
                        result.Add(NewProduct);
                    }
                }
                return result;
            }
        }

        public Product FindById(int id)
        {
            Product p;
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT Product.ID,Product.Name,Product.Price,Product.Stock, VAT.Percentage, Category.Name FROM ((Product INNER JOIN VAT ON Product.VATID = VAT.ID) INNER JOIN Category ON Product.CategoryID = Category.ID) where Product.ID ="+id+"", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                             p = new Product(reader.GetInt32(0), reader.GetString(1), reader.GetDouble(2), reader.GetInt32(3), reader.GetInt32(4), reader.GetString(5));
                            return p;
                        }
                    }
                    else
                    {
                        return null;
                    }

                    
                }
               
            }
            


            throw new NotImplementedException();
        }

        public void Update(Product p)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                
                var command = new SqlCommand("Update Product SET Name = @Name, Price = @Price ,Stock = @Stock WHERE Product.ID = "+p.ID+"", conn);
                command.Parameters.Add("@Name", SqlDbType.NVarChar);
                command.Parameters.Add("@Price", SqlDbType.Float);
                command.Parameters.Add("@Stock", SqlDbType.Int);
                command.Parameters.Add("@ID", SqlDbType.Int);
                command.Parameters["@ID"].Value = p.ID;
                command.Parameters["@Name"].Value = p.Name;
                command.Parameters["@Price"].Value = p.Price;
                command.Parameters["@Stock"].Value = p.Stock;
                conn.Open();
                command.ExecuteNonQuery();
            }

            
        }

        public bool Delete(int id)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var command = new SqlCommand("DELETE FROM Product WHERE Product.ID = @ID", conn);
                command.Parameters.Add("@ID", SqlDbType.Int);
                command.Parameters["@ID"].Value = id;


                int result = command.ExecuteNonQuery();
                if (result == 1)
                    return true;
                else
                    return false;
            }
        }

        public bool UpdateWithConcurrencyCheck(Product p)
        {
            //will save retrieved data from database to both parameters created in product class
            //and set them to same value after making sure theyre not changes
            using (var conn = new SqlConnection(connectionString))
            {
                var command = new SqlCommand("Update Product SET Name = @Name, Price = @Price , Stock = @Stock  WHERE Product.ID = @ID AND Product.Name = @p_Name AND Product.Price = @p_Price AND Product.Stock = @p_stock", conn);
                conn.Open();

                //changing from sql data reader type to c# object
                command.Parameters.Add("@Name", SqlDbType.NVarChar);
                command.Parameters.Add("@Price", SqlDbType.Float);
                command.Parameters.Add("@Stock", SqlDbType.Int);
                command.Parameters.Add("@ID", SqlDbType.Int);
                //make changes for second parametres
                command.Parameters.Add("@p_Name", SqlDbType.NVarChar);
                command.Parameters.Add("@p_Price", SqlDbType.Float);
                command.Parameters.Add("@p_stock", SqlDbType.Int);

                //saving values to both parametres to check later for similarity
                //
                command.Parameters["@ID"].Value = p.ID;
                command.Parameters["@Name"].Value = p.Name;
                command.Parameters["@Price"].Value = p.Price;
                command.Parameters["@Stock"].Value = p.Stock;
                command.Parameters["@p_Name"].Value = p.p_Name;
                command.Parameters["@p_Price"].Value = p.p_Price;
                command.Parameters["@p_stock"].Value = p.p_stock;

                int rowsAffected = command.ExecuteNonQuery();
                if(rowsAffected != 0)
                {
                    //after succesull implementation and afffected row update product variables
                    p.p_Name = p.Name;
                    p.p_Price = p.Price;
                    p.p_stock = p.Stock;
                    return true;
                }
                else
                {
                    return false;
                }

            }
                throw new NotImplementedException();
        }
    }

    
}

