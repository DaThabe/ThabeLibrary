namespace Thabe.ChatBot.Plugin.Com.DaThabe;

internal class Commands : ICommandsPlugin
{
    /// <summary>
    /// 复读
    /// </summary>
    public async void Repeat()
    {
        await this.GetReceiver().ReplyAsync("下一句我会复读", Replys.At | Replys.Quote | Replys.Recall, 5000);

        this.SetAtionContext(ReceiveMessage);

        async void ReceiveMessage()
        {
            var recevier = this.GetReceiver();

            await recevier.ReplyAsync(recevier.MessageChain.GetPlainMessage(), Replys.At | Replys.Quote | Replys.Recall, 5000);
        }
    }

    /// <summary>
    /// 说一句话
    /// </summary>
    public async void Say()
    {
        var recevier = this.GetReceiver();

        var input = this.GetInputParam("plain_text");
        await recevier.ReplyAsync(input);
    }

    /// <summary>
    /// 查看引用消息的消息
    /// </summary>
    public async void Quote()
    {
        var recevier = this.GetReceiver();

        var quote = recevier.MessageChain.OfType<QuoteMessage>().FirstOrDefault();
        if (quote == null) return;

        MessageChain chain = new(quote.Origin);
        await recevier.ReplyAsync(chain);
    }
}