using System;
using Mirai.Net.Data.Messages;
using Mirai.Net.Data.Messages.Concretes;
using System.Linq;
using Thabe.Bot.QQ;
using Thabe.Bot.Plugin.Command;

namespace Thabe.Bot.Plugin.Com.DaThabe;



[CommandPluginConfig("""
{
    "id": "com.dathabe",
    "name": "测试",
    "ver": "0.0.1",
    "desc": "这是一个测试用的模板",
    "domain": {
    "name": "dathabe",
    "alias": [ "thabe" ]
    },
    "tokens": {
        "repeat": [ "复读" ],
        "say": [ "说" ],
        "quote": [ "引用" ],
        "reload": [ "重载" ]
    },
    "params": {
        "plain_text": "\\w+"
    },
    "commands": [
    {
        "name": "复读下一句话",
        "describe": "就是复读下一句话",
        "expr": [ "tokens.repeat" ],
        "action": "Repeat"
    },
    {
        "name": "引用一个消息",
        "describe": "就是引用一个消息",
        "expr": [ "tokens.quote" ],
        "action": "Quote"
    },
    {
        "name": "说一句话",
        "describe": "就是引用一个消息",
        "expr": [ "tokens.say", "params.plain_text" ],
        "action": "Say"
    },
    {
        "name": "重新加载插件",
        "describe": "就是重新加载插件",
        "expr": [ "tokens.reload" ],
        "action": "Reload"
    }
    ]
}
""")]
internal class Commands : ICommandPlugin
{
    /// <summary>
    /// 复读
    /// </summary>
    public async void Repeat()
    {
        this.SetAtionContext(ReceiveMessage);

        await this.GetReceiver().ReplyAsync("下一句就复读", Replys.At | Replys.Quote | Replys.Recall, 5000);


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


    public void Reload()
    {
        foreach(var i in CommandPluginExtend.Plugins)
        {
            Console.WriteLine($"正在重载: [{i.Meta.Name}]");
            i.Reload();
            Console.WriteLine($"重载成功: [{i.Meta.Name}]");
        }
    }
}