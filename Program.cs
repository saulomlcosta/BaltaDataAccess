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
                ExecuteProcedure(connection, new Guid("e1bc6798-9941-41ff-bea6-6e204ebfee31"));
                // StudentBirthdayFunction(connection, "1994-12-01", "1994-12-30");
            }

            static void ListCategories(SqlConnection connection)
            {
                var categories = connection.Query<Category>("SELECT [Id], [Title] FROM [Category] ORDER BY [Title]");

                foreach (var item in categories)
                {
                    Console.WriteLine($"{item.Id} - {item.Title}");
                }
            }

            static void GetCategory(SqlConnection connection)
            {
                var category = connection
                    .QueryFirstOrDefault<Category>(
                        "SELECT TOP 1 [Id], [Title] FROM [Category] WHERE [Id] = @id",
                    new
                    {
                        id = new Guid("09ce0b7b-cfca-497b-92c0-3290ad9d5142")
                    }
                );

                Console.WriteLine($"{category.Id} - {category.Title}");
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

            static void ExecuteProcedure(SqlConnection connection, Guid studentId)
            {
                var procedure = "[spDeleteStudent]";
                var pars = new { StudentId = studentId };
                var affectedRows = connection.Execute(
                    procedure,
                    pars,
                    commandType: System.Data.CommandType.StoredProcedure);

                Console.WriteLine($"{affectedRows} linhas afetadas");
            }

            static void StudentBirthdayFunction(SqlConnection connection, String min, String max)
            {
                var student = connection.
                Query("SELECT * FROM DateStudent(@min, @max)",
                  new
                  {
                      min = min,
                      max = max
                  }
                );

                foreach (var item in student)
                {
                    Console.WriteLine($"{item.Name} - {item.Id}");
                }

            }
        }
    }
}