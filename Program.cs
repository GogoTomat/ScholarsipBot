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

double money = 0;
double booster1 = 0;
double Bn1 = 0;
double booster2 = 0;
double Bn2 = 0;
double booster3 = 0;
double Bn3 = 0;
double booster4 = 0;
double Bn4 = 0;
double booster5 = 0;
double Bn5 = 0;

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
            await botClient.AnswerCallbackQueryAsync(callbackQueryId: update.CallbackQuery.Id, text: "+points");
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

        //обработка магазина, надо дописать кнопки 
        else if(messageText == "/shop")
    {
        Message sentShop = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "1. Купить шаурму за 10 очков(+0.01 за клик) " + '\n' +
            "2. Выпить энергетик Nurb за 50 очков(+0.05 за клик) " + '\n' +
            "3. Сходить на консультацию за 100 очков(+0.5 за клик)" + '\n' +
           " 4. Сходить на лекция за 500 очков(+1 за клик) " + '\n' +
            "5. Сходить на экзамен за 1000 очков(+5 за клик)",
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
