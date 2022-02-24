﻿using Telegram.Bot;
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
    //начало
    if (update.Type != UpdateType.Message)
        return;
    if (update.Message!.Type != MessageType.Text)
        return;

    var chatId = update.Message.Chat.Id;
    var messageText = update.Message.Text;

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
        //создание кнопки, отвечающей за набор очков. Не работает, не понятно, как отслеживать нажатие и прибавлять очки
        InlineKeyboardMarkup farm = new(new[]
    {
        new []
        {
            
            InlineKeyboardButton.WithCallbackData(text: "farm", callbackData: "1"),    
        },   
 
    });
               

        Message sentYes = await botClient.SendTextMessageAsync(
                chatId: chatId, text: "Начинаем. В нашем вузе ты теряешь не всю стипендию, а только ее часть, ты должен сам ее зарабатывать. За каждое выполненное задание изначально ты получаешь 0.01 рублей. Так как я гений учебы, то тебе надо просто нажимать на кнопку, чтобы заработать деньги. В магазине ты можешь купить бустеры, чтобы зарабатывать быстрее.", 
                replyMarkup: farm,
                cancellationToken: cancellationToken);
        }
    //обработка второго ответа
        else if (messageText == "нет" || messageText == "Нет")
    {
        Message sentNo = await botClient.SendTextMessageAsync(
                chatId: chatId, text: "Мне нечего кушать, ты должен быть готов.", cancellationToken: cancellationToken);
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