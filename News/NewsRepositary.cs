using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News
{
    public class NewsRepositary
    {
        public void InsertNews(string query, New newwork, string connectionString)
        {
            using (var sql = new SqlConnection(connectionString))
            {
                var result = sql.Execute(query, newwork);  // количество добавленных записей
                if (result < 1) throw new Exception("Ошибка при вставке записи");

            }
            Console.WriteLine("\tВставка произошла успешно.");
        }

        public void InsertComments(string query, Comment comment, string connectionString)
        {
            using (var sql = new SqlConnection(connectionString))
            {
                var result = sql.Execute(query, comment);  // количество добавленных записей
                if (result < 1) throw new Exception("Ошибка при вставке записи");

            }
            Console.WriteLine("\tВставка произошла успешно.");
        }

        public List<New> GetAllNews(string query, string connectionString)
        {
            using (var sql = new SqlConnection(connectionString))
            {
                return sql.Query<New>(query).ToList();
            }
        }

        public List<Comment> GetAllComments(string query, string connectionString, Guid newId)
        {
            using (var sql = new SqlConnection(connectionString))
            {
                return sql.Query<Comment>(query,new { newId = newId}).ToList();
            }
        }
    }
}
