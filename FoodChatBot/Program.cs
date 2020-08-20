using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.InlineQueryResults;


namespace FoodChatBot
{

    class Program
    {
        public static string Option { set; get; }
        public static string Paragraph { set; get; }
        public static string Payment { set; get; }
        public static string Time { set; get; }
        public static string Price { set; get; }
        public static string Mail { set; get; }

        static TelegramBotClient Bot;

        static void Main(string[] args)
        {
            Bot = new TelegramBotClient("1252119786:AAEXn46wlAU51P6RMzw9rnAIVbDfLhIGql8");
            var me = Bot.GetMeAsync().Result;

            Bot.OnMessage += BotOnMessageReceived;
            Bot.OnCallbackQuery += BotOnCallbackQueryReceived;
            Console.WriteLine(me.FirstName);

            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();
        }
        private async static void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs e)
        {

        }
        private async static void BotOnMessageReceived(object sender, MessageEventArgs e)
        {

            var message = e.Message;
            var replyKeyboard = new ReplyKeyboardMarkup(new[]
{
                        new[]
                        {
                            new KeyboardButton("Выбрать область!"),
                            new KeyboardButton("Нужна помощь")
                        }
                    });
            var replyKeyboardPrice = new ReplyKeyboardMarkup(new[]
                {
                        new[]
                        {
                            new KeyboardButton("Наличные"),
                            new KeyboardButton("Карта")
                        }
                    });

            string help = @"
'Что умеет этот бот?'
Возможность дистанционно заказывать еду
на ближайшей станции пути-следования.
Более подробно узнай у разработчика
mail: devbot@mail.ru
phone: 89000000000


Основные команды:
/start - запуск бота
/help - помощь";
            string dev = "Находится на этапе разработки(";


            if (message == null || message.Type != MessageType.Text)
                return;

            string name = $"{message.From.FirstName} {message.From.LastName}";
            Console.WriteLine($"{name} отправил сообщение {message.Text}");
            if (message.Text == "Волгоградская" || message.Text == "Астраханская" || message.Text == "Ланч-бокс c собой")
            {
                await Bot.SendTextMessageAsync(message.From.Id, dev);
            }
            if (message.Text == "/start")
            {

                await Bot.SendTextMessageAsync(message.From.Id, $"Привет {message.From.FirstName}, добро пожаловать в чат - бот РЖД Food!", replyMarkup: replyKeyboard);
            }
            if (message.Text == "Выбрать область!")
            {
                var replyKeyboardObl = new ReplyKeyboardMarkup(new[]
                   {
                        new[]
                        {
                            new KeyboardButton("Саратовская"),
                            new KeyboardButton("Волгоградская")
                        },
                        new[]
                        {
                            new KeyboardButton("Астраханская")
                        }
                    });
                await Bot.SendTextMessageAsync(message.From.Id, "Выбери область, где ты хотел(а) бы поесть!", replyMarkup: replyKeyboardObl);
            }
            if (message.Text == "/help" || message.Text == "Нужна помощь")
            {
                await Bot.SendTextMessageAsync(message.From.Id, help);
            }
            if (message.Text == "Саратовская")
            {
                var replyKeyboardSar = new ReplyKeyboardMarkup(new[]
                   {
                        new[]
                        {
                            new KeyboardButton("г.Саратов ул.Московская д.8"),
                            new KeyboardButton("г.Саратов ул.1-й Станционный проезд д.14")
                        },
                        new[]
                        {
                            new KeyboardButton("Анисовка"),
                            new KeyboardButton("Перелюб")
                        },
                        new[]
                        {
                            new KeyboardButton("Озинки"),
                            new KeyboardButton("Сенная")
                        }
                    });
                await Bot.SendTextMessageAsync(message.From.Id, $"Отлично {message.From.FirstName}, теперь выбери пункт приема пищи.", replyMarkup: replyKeyboardSar);
            }
            if (message.Text == "г.Саратов ул.Московская д.8" || message.Text == "г.Саратов ул.1-й Станционный проезд д.14" || message.Text == "Анисовка" || message.Text == "Перелюб" || message.Text == "Озинки" || message.Text == "Сенная")
            {
                Paragraph = message.Text;
                var replyKeyboardSarM8 = new ReplyKeyboardMarkup(new[]
                {
                        new[]
                        {
                            new KeyboardButton("Меню столовой"),
                            new KeyboardButton("Ланч-бокс c собой")
                        }
                    });
                await Bot.SendTextMessageAsync(message.From.Id, @"Выбери один из вариантов:
1. Поесть в столовой и посмотреть меню на сегодня
2. Забрать еду c собой, посмотреть варианты Ланч-бокса", replyMarkup: replyKeyboardSarM8);
            }
            if (message.Text == "Меню столовой")
            {
                var replyKeyboardSarM8SM = new ReplyKeyboardMarkup(new[]
                    {
                        new[]
                        {
                            new KeyboardButton("Вариант 1"),
                            new KeyboardButton("Вариант 2")
                        }
                    });
                await Bot.SendTextMessageAsync(message.From.Id, $"Сегодня ({DateTime.Today.ToString("d")}) в меню 2 варианта бизнес-ланча"
+ @"
" +
@"1. Борщ, куриная отбивная с картофельным пюре, чай с пирожным (250 рублей)
2. Солянка, рыбная котлета с макаронами, компот (230 рублей)", replyMarkup: replyKeyboardSarM8SM);

            }
            if (message.Text == "Вариант 1")
            {
                Option = "Бизнес-Ланч 1 (Борщ, куриная отбивная с картофельным пюре, чай с пирожным)";
                Price = "250";
                await Bot.SendTextMessageAsync(message.From.Id, $"{message.From.FirstName} выбери способ оплаты.", replyMarkup: replyKeyboardPrice);
            }
            if (message.Text == "Вариант 2")
            {
                Option = "Бизнес-ланч 2 (Солянка, рыбная котлета с макаронами, компот)";
                Price = "230";
                await Bot.SendTextMessageAsync(message.From.Id, $"{message.From.FirstName}, выбери способ оплаты.", replyMarkup: replyKeyboardPrice);
            }
            if (message.Text == "Наличные")
            {
                Payment = "Наличные";
                var ReplyMarkup = new ReplyKeyboardRemove();
                await Bot.SendTextMessageAsync(message.From.Id, $"Хорошо, теперь напиши во сколько тебя ждать, чтобы твоя еда была горячая (например 11:00)", replyMarkup: ReplyMarkup);
            }
            if (message.Text == "Карта")
            {
                Payment = "Карта";
                var ReplyMarkup = new ReplyKeyboardRemove();
                await Bot.SendTextMessageAsync(message.From.Id, $"Хорошо, теперь напиши во сколько тебя ждать, чтобы твоя еда была горачая (например 11:00)", replyMarkup: ReplyMarkup);
            }
            if (message.Text.Contains(":"))
            {
                Time = message.Text;
                Mail = $"Новый заказ! {name}, {Option}, к {Time}, к оплате {Price}, способо облаты {Payment}";
                var replyKeyboardOk = new ReplyKeyboardMarkup(new KeyboardButton("Заказать"));
                await Bot.SendTextMessageAsync(message.From.Id, $"Проверим заказ: Твой заказ {Option}, будет готов по адресу {Paragraph} к {Time}, к оплате {Price}, способ оплаты {Payment}. Если все верно нажми заказать!", replyMarkup: replyKeyboardOk);
            }
            if (message.Text == "Заказать")
            {
                MailPull();
                await Bot.SendTextMessageAsync(message.From.Id, $"Спасибо за заказ, {message.From.FirstName}, буду ждать тебя снова)", replyMarkup: replyKeyboard);
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static async void MailPull()
        {
            MailAddress from = new MailAddress("RZDFoodBot@yandex.ru", "RZDFoodBot");
            MailAddress to = new MailAddress("misha220993@gmail.com");
            MailMessage m = new MailMessage(from, to);
            m.Subject = Paragraph;
            m.Body = Mail;
            SmtpClient smtp = new SmtpClient("smtp.yandex.ru", 587);
            smtp.Credentials = new NetworkCredential("RZDFoodBot@yandex.ru", "kloun123");
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(m);
            Console.WriteLine("Письмо отправлено");
        }
    }
}
