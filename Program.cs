using System;
using BaltaDataAccess.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BaltaDataAccess
{
    class Program
    {
        static void Main(string[] args)
        {
            const string connectionString
            = "Server=localhost,1433;Database=balta;User ID=sa;Password=1q2w3e4r@#$;TrustServerCertificate=True";


            using (var connection = new SqlConnection(connectionString))
            {
                DeleteCategory(connection);

                ListCategories(connection);
            }

            static void ListCategories(SqlConnection connection)
            {
                var categories = connection.Query<Category>("SELECT [Id], [Title] FROM [Category] ORDER BY [Title]");

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
                category.Description = "Categoria destinada a serviços do AWS";
                category.Order = 8;
                category.Summary = "AWS Cloud";
                category.Featured = false;

                var insertSql = @"INSERT INTO
                        [Category]
                    VALUES(
                        @Id,
                        @Title,
                        @Url,
                        @Description,
                        @Order,
                        @Summary,
                        @Featured)";

                var rows = connection.Execute(insertSql, new
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
                var rows = connection.Execute(updateQuery, new
                {
                    id = new Guid("4e07c597-711a-43f0-8a6f-a4463a515022"),
                    title = "Docker"
                });

                Console.WriteLine($"{rows} registro(s) atualizado(s)");
            }

            static void DeleteCategory(SqlConnection connection)
            {
                var deleteQuery = "DELETE [Category] WHERE [Id]=@id";
                var rows = connection.Execute(deleteQuery, new
                {
                    id = new Guid("4e07c597-711a-43f0-8a6f-a4463a515022")
                });

                Console.WriteLine($"{rows} registro(s) deletado(s)");
            }
        }
    }
}