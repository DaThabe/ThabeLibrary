namespace Thabe.Bot.QQ;


/// <summary>
/// QQBot扩展方法
/// </summary>
public static class QQBotExtend
{
    /// <summary>
    /// 引用回复消息
    /// </summary>
    private static async Task QuoteMessageAsync(this MessageReceiverBase receiver, MessageChain chain, int recallTime = 0)
    {
        try
        {
            string msg_id = "", id = "";

            if (receiver is GroupMessageReceiver group)
            {
                id = group.GroupId;
                msg_id = await MiraiScaffold.QuoteMessageAsync(group, chain);
            }
            else if (receiver is FriendMessageReceiver friend)
            {
                id = friend.FriendId;
                msg_id = await MiraiScaffold.QuoteMessageAsync(friend, chain);
            }

            if (recallTime != 0)
            {
                await Task.Delay(recallTime);
                await MessageManager.RecallAsync(msg_id, id);
            }

            Log.Write(chain);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }


    /// <summary>
    /// 回复消息
    /// </summary>
    /// <param name="receiver"></param>
    /// <param name="chain"></param>
    private static async Task ReplyAsync(this MessageReceiverBase receiver, MessageChain chain, int recallTime = 0)
    {
        try
        {
            string msg_id = "", id = "";

            if (receiver is GroupMessageReceiver group)
            {
                id = group.GroupId;
                msg_id = await MiraiScaffold.SendMessageAsync(group, chain);
            }
            else if (receiver is FriendMessageReceiver friend)
            {
                id = friend.FriendId;
                msg_id = await MiraiScaffold.SendMessageAsync(friend, chain);
            }

            if (recallTime != 0)
            {
                await Task.Delay(recallTime);
                await MessageManager.RecallAsync(msg_id, id);
            }

            Log.Write(chain);
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }


    /// <summary>
    /// 回复消息
    /// </summary>
    /// <param name="chain">消息链</param>
    /// <param name="replyMode">回复模式</param>
    /// <param name="recallTime">撤回消息时间</param>
    public static async Task ReplyAsync(this MessageReceiverBase receiver, MessageChain chain, Replys replyMode = Replys.None , int recallTime = 0)
    {
        MessageChainBuilder mcb = new();

        if(receiver is GroupMessageReceiver group && (replyMode & Replys.At) == Replys.At)
        {
            mcb.At(group.Sender);
        }
        if ((replyMode & Replys.Recall) != Replys.Recall)
        {
            recallTime = 0;
        }


        chain = mcb.Build() + chain;

        if ((replyMode & Replys.Quote) == Replys.Quote)
        {
            await receiver.QuoteMessageAsync(chain, recallTime);
        }
        else
        {
            await receiver.ReplyAsync(chain, recallTime);
        }
    }


    /// <summary>
    /// 回复文本消息
    /// </summary>
    /// <param name="message">消息</param>
    /// <param name="replyMode">回复模式</param>
    /// <param name="recallTime">撤回时间</param>
    public static async Task ReplyAsync(this MessageReceiverBase receiver, string message, Replys replyMode = Replys.None, int recallTime = 0)
    {
        MessageChainBuilder chain = new();
        chain.Plain(message);

        await ReplyAsync(receiver, chain.Build(), replyMode, recallTime);
    }

}


[Flags]
public enum Replys
{
    /// <summary>
    /// 默认模式
    /// </summary>
    None = 0,

    /// <summary>
    /// 引用
    /// </summary>
    Quote = 1,

    /// <summary>
    /// @
    /// </summary>
    At = 2,

    /// <summary>
    /// 撤回
    /// </summary>
    Recall = 4
}
