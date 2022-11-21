namespace Thabe.Bot;


public static class Log
{
    public static void Write(MessageChain chain, LogLevel level = LogLevel.Console)
    {
        StringBuilder sb = new();
        foreach(var i in chain)
        {
            if(i.Type == Messages.Plain)
            {
                sb.Append((i as PlainMessage)!.Text);
            }
            else sb.Append($"{i.Type}");
        }

        Write(sb.ToString(), level);
    }

    public static void Write(Exception e, LogLevel level = LogLevel.ConsoleAndMaster)
    {
        if ((level & LogLevel.Console) == LogLevel.Console)
        {
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]\n{e}");
        }
        if ((level & LogLevel.ConsoleAndMaster) == LogLevel.ConsoleAndMaster)
        {
            MessageManager.SendFriendMessageAsync(QQBot.Master, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]\n{e.Message}");
        }
    }

    public static void Write(string msg, LogLevel level = LogLevel.Console)
    {
        var log_text = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {msg}";

        if ((level & LogLevel.Console) == LogLevel.Console)
        {
            Console.WriteLine(log_text);
        }
        if((level & LogLevel.ConsoleAndMaster) == LogLevel.ConsoleAndMaster)
        {
            MessageManager.SendFriendMessageAsync(QQBot.Master, log_text);
        }
    }
}

public enum LogLevel
{
    Console,

    ConsoleAndMaster
}
