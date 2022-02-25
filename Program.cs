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

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    if (update.Type == UpdateType.CallbackQuery)
    {        if (update.CallbackQuery.Data == "farm")
        {
            Info.money += Info.acc;
            await botClient.AnswerCallbackQueryAsync(callbackQueryId: update.CallbackQuery.Id, text: '+' + Info.acc.ToString());
            Console.WriteLine(Info.money);
        }
        
        if (update.CallbackQuery.Data == "shaurma")
        {
            if (Info.money >= 100)
            {
                Info.n1 += 1;
                Info.money -= 100;
                Info.acc += 1;
                await botClient.AnswerCallbackQueryAsync(callbackQueryId: update.CallbackQuery.Id, text: "Куплена 1 шаурма!");            }
            else {
                await botClient.AnswerCallbackQueryAsync(callbackQueryId: update.CallbackQuery.Id, text: "Недостаточно денег");
            }
        }
       
        if (update.CallbackQuery.Data == "Nurb")
        {
            if (Info.money >= 500)
            {
                Info.n2 += 1;
                Info.money -= 500;
                Info.acc += 5;
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
                Info.n3 += 1;
                Info.money -= 1500;
                Info.acc += 15;
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
                Info.n4 += 1;
                Info.money -= 10000;
                Info.acc += 100;
                await botClient.AnswerCallbackQueryAsync(callbackQueryId: update.CallbackQuery.Id, text: "Вы успешно сходили на лекцию!");
            }
            else
            {
                await botClient.AnswerCallbackQueryAsync(callbackQueryId: update.CallbackQuery.Id, text: "Недостаточно денег");
            }
        }

        if (update.CallbackQuery.Data == "ex")
        {
            if (Info.money >= 50000)
            {
                Info.n5 += 1;
                Info.money -= 50000;
                Info.acc += 500;
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

    if (messageText == "/start")
    {
        Message sentMessage = await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: "Привет, меня зовут Кадим Вадырин. Я студент ГурСу. Я прилежный ученик, но к сожалению, один предмет так мне и не дался. Все дело в физике. Из-за физики я лишился стипендии. Помоги мне вернуть ее, накликай мне деньги. Ты готов?",
        cancellationToken: cancellationToken);
    }
  
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
            text: "1. Купить шаурму за 100 очков(+1 за клик) " + '\n' +
            "2. Выпить энергетик Nurb за 500 очков(+5 за клик) " + '\n' +
            "3. Сходить на консультацию за 1500 очков(+15 за клик)" + '\n' +
           " 4. Сходить на лекция за 10000 очков(+100 за клик) " + '\n' +
            "5. Сходить на экзамен за 50000 очков(+500 за клик)",
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
        else if(messageText == "/amount")
    {
        Message sentAmount = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Количество шаурмы:" + " " + Info.n1.ToString() + '\n' +
                  "Количество энергетиков" + " " + Info.n2.ToString() + '\n' +
                  "Количество консультаций" + " " + Info.n3.ToString() + '\n' +
                  "Количсетво практик" + " " + Info.n4.ToString() + '\n' +
                  "Количество экзаменов" + " " + Info.n5.ToString() + '\n',
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
