using BotTelegramHomework.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotTelegramHomework
{
    public class Appoint
    {
        private ITelegramBotClient _botClient;
        private Chat _chat;

        public Appoint(ITelegramBotClient botClient, Chat chat)
        {
            _botClient = botClient;
            _chat = chat;
        }

        public async Task StartAsync()
        {
            InlineKeyboardMarkup inlineKeyboard = new(
                new[] {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Тестировщик", "QAEngineer")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Разработчик", "Developer")
                    }
                }
                );

            await _botClient.SendTextMessageAsync(_chat.Id,
                    $"Укажите кем вы являетесь",
                    replyMarkup: inlineKeyboard);
        }

        internal async Task OnAnswer(CallbackQuery callbackQuery)
        {
            var users = UserRepository.GetUser();
            
            switch (callbackQuery.Data)
            {
                case "QAEngineer":
                    UserRepository.Add(users, callbackQuery, "QAEngineer");

                    InlineKeyboardMarkup inlineKeyboard = new(
                        new[] {
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Запустить новое тестирование", "NewTestRun")
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Продолжить текущее тестирование", "TestRun")
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Перейти к активным дефектам", "Defects"),
                                InlineKeyboardButton.WithCallbackData("Посмотреть текущий прогресс", "Progress")
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Сформировать полный отчет", "Report")
                            }
                        }
                    );

                    await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "");
                    await _botClient.SendTextMessageAsync(_chat.Id,
                            $"Выберите действие:",
                            replyMarkup: inlineKeyboard);
                    break;

                case "Developer":
                    UserRepository.Add(users, callbackQuery, "Developer");

                    InlineKeyboardMarkup keysStep2 = new(
                        new[] {
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Посмотреть текущий прогресс", "Progress")
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Перейти к активным дефектам", "Defects")
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Открыть замечания", "Remarks")
                            }
                        }
                    );

                    await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "");
                    await _botClient.SendTextMessageAsync(_chat.Id,
                            $"Выберите действие:",
                            replyMarkup: keysStep2);
                    break;
            }
        }
    }
}
