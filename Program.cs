using Dapper;
using DataAccess.Models;
using Microsoft.Data.SqlClient;

public class Program
{
    public static void Main(string[] args)
    {
        const string connectionString = "Server=localhost,1433;" +
                                        "Database=balta;" +
                                        "User ID=sa;" +
                                        "Password=H2g@5dT$;" +
                                        "Trusted_Connection=False;" +
                                        "TrustServerCertificate=True";

        // Microsoft.Data.SqlClient (NUGET)
        //var connection = new SqlConnection()
        //connection.Open();

        //connection.Close();

        // Conexão com ADO.NET
        //using (var connection = new SqlConnection(connectionString))
        //{
        //    Console.WriteLine("Conectado");
        //    connection.Open();

        //    using (var command = new SqlCommand())
        //    {
        //        command.Connection = connection;
        //        command.CommandType = System.Data.CommandType.Text;
        //        command.CommandText = "SELECT [Id], [Title] FROM [Category]";

        //        var reader = command.ExecuteReader();
        //        while(reader.Read())
        //        {
        //            Console.WriteLine($"{reader.GetGuid(0)} - {reader.GetString(1)}");
        //        }
        //    }
        //}

        

        // Conexão com Dapper
        using (var connection = new SqlConnection(connectionString))
        {
            UpdateCategory(connection);
            ListCategories(connection);
            // CreateCategory(connection);
        }
    }

    static void ListCategories(SqlConnection connection)
    {
        var categories = connection.Query<Category>("SELECT [Id], [Title] FROM [Category]");
        foreach (var item in categories)
        {
            Console.WriteLine($"{item.Id} - {item.Title}");
        }
    }

    static void CreateCategory(SqlConnection connection)
    {
        var category = new Category();
        category.Id = Guid.NewGuid();
        category.Title = "Amazon AWS";
        category.Url = "amazon";
        category.Summary = "Categoria destinada a serviços do AWS";
        category.Order = 8;
        category.Description = "AWS Cloud";
        category.Featured = false;

        var insertSqlCategory = "INSERT INTO " +
                                    "[Category]" +
                                "VALUES(" +
                                    "@Id," +
                                    "@Title," +
                                    "@Url," +
                                    "@Summary," +
                                    "@Order," +
                                    "@Description," +
                                    "@Featured)";

        var rows = connection.Execute(insertSqlCategory, new
        {
            category.Id,
            category.Title,
            category.Url,
            category.Summary,
            category.Order,
            category.Description,
            category.Featured
        });
        Console.WriteLine($"{rows} linha(s) inserida(s)");
    }

    static void UpdateCategory(SqlConnection connection)
    {
        var updateQuery = "UPDATE [Category] SET [Title]=@title WHERE [Id]=@id";
        var rows = connection.Execute(updateQuery, new { 
            id = new Guid("af3407aa-11ae-4621-a2ef-2028b85507c4"),
            title = "Frontend 2023"
        });
        Console.WriteLine($"{rows} registro(s) atualizado(s)");
    }
}
