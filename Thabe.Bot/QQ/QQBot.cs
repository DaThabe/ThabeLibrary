using Thabe.Bot.Plugin;
using Thabe.Bot.Plugin.Command;

namespace Thabe.Bot.QQ;


/// <summary>
/// QQ聊天机器人
/// </summary>
public class QQBot
{
    static QQBot()
    {
        string icon = """
             _____ _           _            ____        _   
            |_   _| |__   __ _| |__   ___  | __ )  ___ | |_ 
              | | | '_ \ / _` | '_ \ / _ \ |  _ \ / _ \| __|
              | | | | | | (_| | |_) |  __/ | |_) | (_) | |_ 
              |_| |_| |_|\__,_|_.__/ \___| |____/ \___/ \__|
                                                            
            """;

        Console.WriteLine(icon);
    }


    public static string Master { get; set; } = "2217568525";

    /// <summary>
    /// MiraiBot实例
    /// </summary>
    private readonly MiraiBot _miraiBot;

    public QQBot(string addres, string verifyKey, string qq)
    {
        _miraiBot = new()
        {
            QQ = qq,
            VerifyKey = verifyKey,
            Address = addres
        };

        Log.Write($"Bot [{qq}] 已创建");
        Log.Write($"绑定地址: [{addres}]");


        _miraiBot.MessageReceived.OfType<MessageReceiverBase>().Subscribe(PluginCall);
        Log.Write($"绑定插件响应");
    }

    private void PluginCall(MessageReceiverBase receiver)
    {
        try
        {
            receiver.CallCommandPlugin();
        }
        catch (Exception ex)
        {
            Log.Write(ex);
        }
    }


    /// <summary>
    /// 启动Bot
    /// </summary>
    /// <returns></returns>
    public async Task LaunchAsync()
    {
        Log.Write("Bot 启动中");

        try
        {
            await _miraiBot.LaunchAsync();
        }
        catch (Exception ex)
        {
            Log.Write(ex);
        }
    }
}