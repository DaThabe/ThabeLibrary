namespace Thabe.ChatBot.QQ;


/// <summary>
/// QQ聊天机器人
/// </summary>
public class QQBot
{
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


        _miraiBot.MessageReceived.OfType<MessageReceiverBase>().Subscribe(PluginCall);
    }

    private void PluginCall(MessageReceiverBase receiver)
    {
        receiver.CallCommandPlugin();
    }


    /// <summary>
    /// 启动Bot
    /// </summary>
    /// <returns></returns>
    public async Task LaunchAsync() => await _miraiBot.LaunchAsync();
}