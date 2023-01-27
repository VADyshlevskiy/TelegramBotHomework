using BotTelegramHomework.Repository;
using BotTelegramHomework.Sourse;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace BotTelegramHomework
{
    public class Program
    {
        private List<User> _users = new List<User>();
        private static Appoint _appoint;

        private static async Task Main(string[] args)
        {

            var bot = new TelegramBotClient(Config._token);                                                                         // создаем подключение к боту
            var me = await bot.GetMeAsync();                                                                                        // запрашиваем информацию о боте

            Console.WriteLine($"My bot: {me.Username}, {me.Id}");

            bot.StartReceiving(HandleUpdateAsync, HandleErrorAsync,
                new Telegram.Bot.Polling.ReceiverOptions
                {
                    AllowedUpdates = Array.Empty<UpdateType>(),
                });

            Console.ReadKey();
        }

        private static Task HandleErrorAsync(ITelegramBotClient bot, Exception exception, CancellationToken arg3)                   // обрабатываем ошибки
        {
            var ErrorMessage = exception.Message;
            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        private static async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken arg3)                  // обрабатываем получение сообщений
        {
            try
            {
                switch (update.Type)
                {
                    case UpdateType.Message:
                        await BotOnMessageReceived(bot, update.Message);
                        break;
                    case UpdateType.CallbackQuery:
                        if (_appoint == null)
                            return;
                        await _appoint.OnAnswer(update.CallbackQuery);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(bot, exception, arg3);
            }
        }

        private static async Task BotOnMessageReceived(ITelegramBotClient bot, Message message)
        {
            Console.WriteLine($"received message: {message.Type}");
            Console.WriteLine($"received message: {message.Text}\t{message.From.Id}\t{DateTime.Now}");

            if (message.Type != MessageType.Text) return;

            var action = message.Text;

            switch (action)
            {
                case "/start":
                    var userName = $"{message.From.LastName} {message.From.FirstName}";
                    await bot.SendTextMessageAsync(message.Chat.Id, $"Привет {userName}");
                    _appoint = new Appoint(bot, message.Chat);
                    await _appoint.StartAsync();
                    break;
                case "/help":
                    userName = $"{message.From.LastName} {message.From.FirstName}";
                    await bot.SendTextMessageAsync(message.Chat.Id, $"{userName}, чем я могу Вам помочь?");
                    break;
                default:
                    await bot.SendTextMessageAsync(message.Chat.Id, "Простите, но я Вас не понимаю! :(");
                    break;
            }

        }
    }
}