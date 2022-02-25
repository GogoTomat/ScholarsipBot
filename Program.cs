using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

var botClient = new TelegramBotClient("5133602661:AAGlVmRW8en513lPgcjaaAflDxaVdywTH4A");

using var cts = new CancellationTokenSource();

var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = { } 
};
botClient.StartReceiving(
    HandleUpdateAsync,
    HandleErrorAsync,
    receiverOptions,
    cancellationToken: cts.Token);

var me = await botClient.GetMeAsync();



Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();

cts.Cancel();

//вот эта штука отвечает за отслеживания нажажатия кнопки 
async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    double money = 10;

    double booster1 = 0;
    double Bn1 = 0;
    double booster2 = 0;
    double Bn2 = 0;
    double booster3 = 0;
    double Bn3 = 0;
    double booster4 = 0;
    double Bn4 = 0;
    double booster5 = 1;
    double Bn5 = 1;

    if (update.Type == UpdateType.CallbackQuery)
    {
        //там ниже кнопка, у нее CallBackData = farm, значит мы по колбек дате отслеживаем нажатие именно этой кнопки, не работает изменение переменной
        if (update.CallbackQuery.Data == "farm")
        {
            money += 0.01 + (booster1 * Bn1 + booster2 * Bn2 + booster3 * Bn3 + booster4 * Bn4 + booster5 * Bn5);
            await botClient.AnswerCallbackQueryAsync(callbackQueryId: update.CallbackQuery.Id, text: "1");
            Console.WriteLine(money);
        }
        //отслеживание кнопок магазина, толком не работает изменение переменных
        if (update.CallbackQuery.Data == "shaurma")
        {
            if (money >= 10)
            {
                booster1 += 0.01;
                Bn1 += 1;
                money -= 10;
                await botClient.AnswerCallbackQueryAsync(callbackQueryId: update.CallbackQuery.Id, text: "Куплена 1 шаурма!");
                //Console.WriteLine(booster1);
                //Console.WriteLine(Bn1);
                //Console.WriteLine(money);
            }
            else {
                await botClient.AnswerCallbackQueryAsync(callbackQueryId: update.CallbackQuery.Id, text: "Недостаточно денег");
            }
        }
       
        if (update.CallbackQuery.Data == "Nurb")
        {
            if (money >= 50)
            {
                booster2 += 0.05;
                Bn2 += 1;
                money -= 50;
                await botClient.AnswerCallbackQueryAsync(callbackQueryId: update.CallbackQuery.Id, text: "Куплен 1 энергетик!");
            }
            else
            {
                await botClient.AnswerCallbackQueryAsync(callbackQueryId: update.CallbackQuery.Id, text: "Недостаточно денег");
            }
        }

        if (update.CallbackQuery.Data == "cons")
        {
            if (money >= 100)
            {
                booster3 += 0.5;
                Bn3 += 1;
                money -= 100;
                await botClient.AnswerCallbackQueryAsync(callbackQueryId: update.CallbackQuery.Id, text: "Вы успешно сходили на консультацию!");
            }
            else
            {
                await botClient.AnswerCallbackQueryAsync(callbackQueryId: update.CallbackQuery.Id, text: "Недостаточно денег");
            }
        }

        if (update.CallbackQuery.Data == "lec")
        {
            if (money >= 500)
            {
                booster4 += 1;
                Bn4 += 1;
                money -= 500;
                await botClient.AnswerCallbackQueryAsync(callbackQueryId: update.CallbackQuery.Id, text: "Вы успешно сходили на лекцию!");
            }
            else
            {
                await botClient.AnswerCallbackQueryAsync(callbackQueryId: update.CallbackQuery.Id, text: "Недостаточно денег");
            }
        }

        if (update.CallbackQuery.Data == "ex")
        {
            if (money >= 1000)
            {
                booster5 += 5;
                Bn5 += 1;
                money -= 1000;
                await botClient.AnswerCallbackQueryAsync(callbackQueryId: update.CallbackQuery.Id, text: "Вы успешно сдали экзамен!");
            }
            else
            {
                await botClient.AnswerCallbackQueryAsync(callbackQueryId: update.CallbackQuery.Id, text: "Недостаточно денег");
            }
        }
        return;
    }

    if (update.Type != UpdateType.Message)
        return;
    if (update.Message!.Type != MessageType.Text)
        return;

    var chatId = update.Message.Chat.Id;
    var messageText = update.Message.Text;

    //создание кнопки, отвечающей за набор очков. Не работает, не понятно, как отслеживать нажатие и прибавлять очки
    InlineKeyboardMarkup farm = new(new[]
 {
        new []
        {
            InlineKeyboardButton.WithCallbackData(text: "CLICK", callbackData: "farm")
        },

    });

    InlineKeyboardMarkup shop = new(new[]
{
        new []
        {
            InlineKeyboardButton.WithCallbackData(text: "1", callbackData: "shaurma"),
            InlineKeyboardButton.WithCallbackData(text: "2", callbackData: "Nurb"),
            InlineKeyboardButton.WithCallbackData(text: "3", callbackData: "cons"),
            InlineKeyboardButton.WithCallbackData(text: "4", callbackData: "lec"),
            InlineKeyboardButton.WithCallbackData(text: "5", callbackData: "ex"),
        },

    });


    //начало
    if (messageText == "/start")
    {
        Message sentMessage = await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: "Привет, меня зовут Кадим Вадырин. Я студент ГурСу. Я прилежный ученик, но к сожалению, один предмет так мне и не дался. Все дело в физике. Из-за физики я лишился стипендии. Помоги мне вернуть ее, накликай мне деньги. Ты готов?",
        cancellationToken: cancellationToken);
    }
  
        //Обработка первого ответа
        else if (messageText == "Да" || messageText == "да")   
        {
        Message sentYes = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Отлично, просто нажимай на эту кнопку и получай очки. Командой '/money' можно узнать сколько денег у тебе. Прописав '/shop' можно увидеть список бустеров. Удачи!",
            replyMarkup: farm,
            cancellationToken: cancellationToken);
    }

        //обработка второго ответа
        else if (messageText == "нет" || messageText == "Нет")
    {
        Message sentNo = await botClient.SendTextMessageAsync(
                chatId: chatId, text: "Мне нечего кушать, ты должен быть готов.", cancellationToken: cancellationToken);
    }

        //обработка магазина
        else if (messageText == "/shop")
    {
        Message sentShop = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "1. Купить шаурму за 10 очков(+0.01 за клик) " + '\n' +
            "2. Выпить энергетик Nurb за 50 очков(+0.05 за клик) " + '\n' +
            "3. Сходить на консультацию за 100 очков(+0.5 за клик)" + '\n' +
           " 4. Сходить на лекция за 500 очков(+1 за клик) " + '\n' +
            "5. Сходить на экзамен за 1000 очков(+5 за клик)",
            replyMarkup: shop,
            cancellationToken: cancellationToken
            );
    }

      
    Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");
 
}

Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}
