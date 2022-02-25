using ScholarshipBot;
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

    if (update.Type == UpdateType.CallbackQuery)
    {
        //там ниже кнопка, у нее CallBackData = farm, значит мы по колбек дате отслеживаем нажатие именно этой кнопки
        if (update.CallbackQuery.Data == "farm")
        {
            Info.money += Info.acc;
            await botClient.AnswerCallbackQueryAsync(callbackQueryId: update.CallbackQuery.Id, text: '+' + Info.acc.ToString());
            Console.WriteLine(Info.money);
        }
        //отслеживание кнопок магазина
        
        if (update.CallbackQuery.Data == "shaurma")
        {
            if (Info.money >= 100)
            {
                Info.b1 += 1;
                Info.n1 += 1;
                Info.money -= 100;
                await botClient.AnswerCallbackQueryAsync(callbackQueryId: update.CallbackQuery.Id, text: "Куплена 1 шаурма!");
            }
            else {
                await botClient.AnswerCallbackQueryAsync(callbackQueryId: update.CallbackQuery.Id, text: "Недостаточно денег");
            }
        }
       
        if (update.CallbackQuery.Data == "Nurb")
        {
            if (Info.money >= 500)
            {
                Info.b2 += 5;
                Info.n2 += 1;
                Info.money -= 500;
                await botClient.AnswerCallbackQueryAsync(callbackQueryId: update.CallbackQuery.Id, text: "Куплен 1 энергетик!");
            }
            else
            {
                await botClient.AnswerCallbackQueryAsync(callbackQueryId: update.CallbackQuery.Id, text: "Недостаточно денег");
            }
        }

        if (update.CallbackQuery.Data == "cons")
        {
            if (Info.money >= 1500)
            {
                Info.b3 += 15;
                Info.n3 += 1;
                Info.money -= 1500;
                await botClient.AnswerCallbackQueryAsync(callbackQueryId: update.CallbackQuery.Id, text: "Вы успешно сходили на консультацию!");
            }
            else
            {
                await botClient.AnswerCallbackQueryAsync(callbackQueryId: update.CallbackQuery.Id, text: "Недостаточно денег");
            }
        }

        if (update.CallbackQuery.Data == "lec")
        {
            if (Info.money >= 10000)
            {
                Info.b4 += 100;
                Info.n4 += 1;
                Info.money -= 10000;
                await botClient.AnswerCallbackQueryAsync(callbackQueryId: update.CallbackQuery.Id, text: "Вы успешно сходили на лекцию!");
            }
            else
            {
                await botClient.AnswerCallbackQueryAsync(callbackQueryId: update.CallbackQuery.Id, text: "Недостаточно денег");
            }
        }

        if (update.CallbackQuery.Data == "ex")
        {
            if (Info.money >= 500000)
            {
                Info.b5 += 1000;
                Info.n5 += 1;
                Info.money -= 500000;
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

    //создание кнопки, отвечающей за набор очков.
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
        
        else if(messageText == "/money")
    {
        Message sentMoney = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Количество денег:" + Info.money.ToString(),
            cancellationToken: cancellationToken);
    }

        else if(messageText == "/play")
    {
        Message sentPlay = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Кликай!!!",
            replyMarkup: farm,
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
