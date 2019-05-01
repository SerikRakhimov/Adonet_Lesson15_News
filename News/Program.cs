using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Dapper;
using DbUp;
using System.Data.SqlClient;
using System.Reflection;


namespace News
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionStringConfiguration = ConfigurationManager.ConnectionStrings["appConnection"];
            var connectionString = connectionStringConfiguration.ConnectionString;
            // var providerName = connectionStringConfiguration.ProviderName;

            #region Migration

            EnsureDatabase.For.SqlDatabase(connectionString); // проверяет есть ли БД, если нет - создает БД

            var upgrader = DeployChanges.To
            .SqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
            .LogToConsole()
            .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful) throw new Exception("Миграция не удалась");
            #endregion

            // Выполняем стандартные запросы Insert, Update, Select и прочие

            int i, j, number;
            string text;
            NewsRepositary _userRep = new NewsRepositary();

            i = 0;
            while (true)
            {
                Console.WriteLine("\n\t--------------------------------------------------");
                Console.WriteLine("\tСписок новостей:\n");

                i = 0;

                var news = _userRep.GetAllNews("Select * from [News]", connectionString);

                news.Sort(delegate (New new1, New new2)
                { return new1.Text.CompareTo(new2.Text); });

                foreach (var item in news)
                {
                    i++;
                    Console.WriteLine($"\t{i} - {item.Text}");
                }
                Console.WriteLine($"\n\t0 - выход");
                Console.Write($"\n\tВведите номер новости (1-{news.Count}), '+' - добавить новость = ");
                string check = Console.ReadLine();

                if (check == "+")  // добавление
                {
                    Console.Write("\n\tВведите новость = ");
                    text = Console.ReadLine();
                    New newwork = new New { Text = text };
                    if (text != "")
                    {
                        _userRep.InsertNews(@"INSERT INTO News (Id,Text) VALUES(@Id, @Text)", newwork, connectionString);
                    }
                }
                else
                {
                    try
                    {
                        number = int.Parse(check);
                        if (number == 0)  // выход с программы
                        {
                            break;
                        }
                        if (1 <= number && number <= news.Count)
                        {

                            New newwork = news[number - 1];

                            j = 0;
                            while (true)
                            {
                                Console.WriteLine("\n\t--------------------------------------------------");
                                Console.WriteLine($"\tНовость = {newwork.Text}");
                                Console.WriteLine("\n\tСписок комментариев к новости:");

                                j = 0;

                                var comments = _userRep.GetAllComments("Select * from [Comments] Where NewId = @NewId", connectionString, newwork.Id);

                                comments.Sort(delegate (Comment com1, Comment com2)
                                { return com1.Text.CompareTo(com2.Text); });

                                foreach (var item in comments)
                                {
                                    j++;
                                    Console.WriteLine($"\t{j} - {item.Text}");
                                }

                                Console.Write("\n\tВведите комментарий (пусто - выход) = ");
                                text = Console.ReadLine();

                                Comment comment = new Comment { Text = text, NewId = newwork.Id };
                                if (text != "")
                                {
                                    _userRep.InsertComments(@"INSERT INTO Comments (Id,Text,NewId) VALUES(@Id, @Text,@NewId)", comment, connectionString);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}
