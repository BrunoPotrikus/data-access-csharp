﻿using Dapper;
using DataAccess.Models;
using Microsoft.Data.SqlClient;
using System.Data;

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
            //UpdateCategory(connection);
            //CreateManyCategory(connection);
            //ListCategories(connection);
            // CreateCategory(connection);
            //ExecuteProcedure(connection);
            //ExecuteReadProcedure(connection);
            //ExecuteScalar(connection);
            ReadView(connection); 
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

    static void CreateManyCategory(SqlConnection connection)
    {
        var category = new Category();
        category.Id = Guid.NewGuid();
        category.Title = "Amazon AWS";
        category.Url = "amazon";
        category.Summary = "Categoria destinada a serviços do AWS";
        category.Order = 8;
        category.Description = "AWS Cloud";
        category.Featured = false;

        var category2 = new Category();
        category2.Id = Guid.NewGuid();
        category2.Title = "Nova Categoria Teste";
        category2.Url = "nova-categoria-teste";
        category2.Summary = "Categoria destinada para teste";
        category2.Order = 9;
        category2.Description = "Testando categoria";
        category2.Featured = true;

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

        var rows = connection.Execute(insertSqlCategory, new[]
        {
            new
            {
                category.Id,
                category.Title,
                category.Url,
                category.Summary,
                category.Order,
                category.Description,
                category.Featured
            },
            new
            {
                category2.Id,
                category2.Title,
                category2.Url,
                category2.Summary,
                category2.Order,
                category2.Description,
                category2.Featured
            }
        });
        Console.WriteLine($"{rows} linha(s) inserida(s)");
    }

    static void ExecuteProcedure(SqlConnection connection)
    {
        var sqlProcedure = "[spDeleteStudent]";
        var parameters = new { StudentId = "a21e82eb-2e66-40a6-87cc-f40ce617b800" };
        var affectedRows = connection.Execute(
            sqlProcedure, 
            parameters,
            commandType: CommandType.StoredProcedure
        );

        Console.WriteLine($"{affectedRows} linhas afetadas");
    }

    static void ExecuteReadProcedure(SqlConnection connection)
    {
        var sqlProcedure = "[spGetCourseByCategory]";
        var parameters = new { CategoryId = "09ce0b7b-cfca-497b-92c0-3290ad9d5142" };
        var readRows = connection.Query(
            sqlProcedure,
            parameters,
            commandType: CommandType.StoredProcedure
        );

        foreach(var item in readRows)
        {
            Console.WriteLine($"{item.Id} - {item.Title}");
        }
    }

    static void ExecuteScalar(SqlConnection connection)
    {
        var category = new Category();
        category.Title = "Amazon AWS";
        category.Url = "amazon";
        category.Summary = "Categoria destinada a serviços do AWS";
        category.Order = 8;
        category.Description = "AWS Cloud";
        category.Featured = false;

        var insertSqlCategory = "INSERT INTO " +
                                    "[Category]" +
                                "OUTPUT inserted.[Id]" +
                                "VALUES(" +
                                    "NEWID()," +
                                    "@Title," +
                                    "@Url," +
                                    "@Summary," +
                                    "@Order," +
                                    "@Description," +
                                    "@Featured)";

        var rowId = connection.ExecuteScalar<Guid>(insertSqlCategory, new
        {
            category.Title,
            category.Url,
            category.Summary,
            category.Order,
            category.Description,
            category.Featured
        });
        Console.WriteLine($"Categoria inserida: {rowId}");
    }

    static void ReadView(SqlConnection connection)
    {
        var sqlView = "SELECT * FROM vwListaCursos";
        var courses = connection.Query(sqlView);

        foreach (var item in courses)
        {
            Console.WriteLine($"{item.Id} - {item.Title}");
        }
    }

    static void OneToOne(SqlConnection connection)
    {
        var sql = "SELECT * FROM" +
                    "[CareerItem]" +
                  "INNER JOIN" +
                    "[Course] ON [CareerItem].[CourseId] = [Course].[Id]";

        var itens = connection.Query(sql);


        foreach (var item in itens)
        {
            Console.WriteLine($"{item.Id} - {item.Title}");
        }
    }
}
