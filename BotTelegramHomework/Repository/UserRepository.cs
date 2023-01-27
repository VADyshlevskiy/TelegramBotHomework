using BotTelegramHomework.Sourse;
using BotTelegramHomework.Tables;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotTelegramHomework.Repository
{
    public class UserRepository
    {
        public static void Add(List<Users> users, Telegram.Bot.Types.CallbackQuery message, string role)            //Добавление нового пользователя
        {
            var user = new Users();
            {
                user.Role = role;
                user.FirstName = message.From.FirstName;
                user.LastName = message.From.LastName;
                user.Chat_Id = message.From.Id;
            }

            List<long> ids = new List<long>();
            foreach (var item in users)
            {
                ids.Add(item.Chat_Id);
            }

            if (!ids.Contains(user.Chat_Id))
            {
                using (var connection = new NpgsqlConnection(Config.SqlConnectionString))
                {
                    string sql = $"insert into Users (firstname, lastname, role, chat_id) values (@firstname, @lastname, @role, @chat_id) ";
                    connection.Execute(sql, user);
                }
            }
        }

        public static List<Users> GetUser()                                                                         // Метод получения списка пользователей
        {                                                                                                           // с базы данных
            using (var connection = new NpgsqlConnection(Config.SqlConnectionString))
            {
                string sql = $"select firstname, lastname, role, chat_id from users";
                return connection.Query<Users>(sql).ToList();
            }
        }
    }
}