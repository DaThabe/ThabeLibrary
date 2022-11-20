using System.Threading;

namespace Thabe.Bot.Plugin.Command;


/// <summary>
/// 指令插件接口
/// </summary>
public interface ICommandPlugin : IPlugin
{
}




/// <summary>
/// 指令插件扩展方法
/// </summary>
public static class CommandPluginExtend
{
    /// <summary>
    /// 插件元数据
    /// </summary>
    private static readonly List<ICommandPluginTemplate> _plugins = new();

    /// <summary>
    /// 用户对话上下文
    /// </summary>
    private static readonly Dictionary<string, CommandPluginContext> _memberContexts = new();

    public static IEnumerable<ICommandPluginTemplate> Plugins => _plugins;

    /// <summary>
    /// 注册插件
    /// </summary>
    /// <exception cref="Exception"></exception>
    public static void Register(this ICommandPluginTemplate plugin)
    {
        if (_plugins.Any(x => x.Meta.Id.Equals(plugin.Meta.Id, StringComparison.InvariantCultureIgnoreCase)))
        {
            throw new Exception("插件id冲突");
        }

        plugin.Reload();
        _plugins.Add(plugin);
    }

    /// <summary>
    /// Call插件
    /// </summary>
    public static void CallCommandPlugin(this MessageReceiverBase receiver)
    {
        //获取用户id
        var id = get_member_id();
        if (string.IsNullOrWhiteSpace(id)) return;

        //如果存在实例
        if (_memberContexts.TryGetValue(id, out CommandPluginContext? context))
        {
            var action = context.CurrentAction;
            context.Receiver = receiver;

            context.CurrentAction = null;

            action?.Invoke();
            release_message();

            //如果下来没有动作则移除上下文
            if (context.CurrentAction == null)
            {
                context.CurrentAction = context.FirstAction;
                _memberContexts.Remove(id);

                return;
            }

            msg_context_timeout();
        }


        //获取用户输入的字符串
        var cmd_str = receiver.MessageChain.GetPlainMessage();
        if (string.IsNullOrWhiteSpace(cmd_str)) return;

        //获取所有可执行动作
        var all_excute = (from plugin in _plugins
                                 from cmd in plugin.Meta!.Commands
                                 where cmd.Trigger.IsTrigger(cmd_str)
                                 select (cmd.MethodName, plugin, cmd.Trigger)).ToArray();

        //获取第一个可执行动作
        var excute = all_excute.FirstOrDefault();
        if (excute == (null, null, null)) return;


        //创建插件实例
        if (excute.plugin.PluginType == null) return;
        if (Activator.CreateInstance(excute.plugin.PluginType) is not ICommandPlugin instance) return;


        //获取第一个动作方法实例
        var method = excute.plugin.PluginType?.GetTypeInfo().DeclaredMethods.FirstOrDefault(x =>
        {
            return x.Name.Equals(excute.MethodName, StringComparison.InvariantCultureIgnoreCase);
        });
        if (method == null) return;
        Action first_action = (Action)Delegate.CreateDelegate(typeof(Action), instance, method);


        _memberContexts[id] = new()
        {
            Receiver = receiver,
            MemberId = id,
            FirstAction = first_action,
            CurrentAction = first_action,
            PluginInstance = instance,
            PluginTempalte = excute.plugin,
            MatchGroup = ((RegexTrigger)excute.Trigger).GetMatchGroup(cmd_str)
        };

        first_action?.Invoke();
        release_message();


        //瞬态对话
        if (_memberContexts[id].CurrentAction == _memberContexts[id].FirstAction)
        {
            _memberContexts.Remove(id);
            return;
        }

        msg_context_timeout();


        async void msg_context_timeout()
        {
            if (!_memberContexts.TryGetValue(id, out var context)) return;

            if (context.TimeOut < 0) return;

            
            var timeout = context.TimeOut;
            context.TimeOut = 0;

            await Task.Delay(timeout);

            if (context.TimeOut <= 0) return;

            var receiver = context.PluginInstance.GetReceiver();
            await receiver.ReplyAsync($"[{context.FirstAction.Method.Name}] 任务已超时!");

            _memberContexts.Remove(id);
        }

        void release_message()
        {
            var context = _memberContexts[id];
            var rece = context.PluginInstance.GetReceiver();

            foreach (var i in context.MsgBuffer)
            {
                rece.ReplyAsync(i.Chain, i.Mode, i.RecallTime).GetAwaiter().GetResult();
            }

            context.MsgBuffer.Clear();
        }

        string? get_member_id()
        {
            string? id = null;

            if (receiver is FriendMessageReceiver f) id = $"{f.Sender.Id}";
            if (receiver is GroupMessageReceiver g) id = $"{g.GroupId}_{g.Sender.Id}";

            return id;
        }
    }

    /// <summary>
    /// 获取插件上下文实例
    /// </summary>
    public static CommandPluginContext? GetPluginContext(this ICommandPlugin plugin)
    {
        return _memberContexts.FirstOrDefault(x => x.Value.PluginInstance == plugin).Value;
    }


    /// <summary>
    /// 获取输入的关键字
    /// </summary>
    /// <param name="plugin"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string GetInputToken(this ICommandPlugin plugin, string name)
    {
        return plugin.GetPluginContext()!.MatchGroup[$"tokens_{name}"].Value;
    }

    /// <summary>
    /// 获取输入的参数
    /// </summary>
    public static string GetInputParam(this ICommandPlugin plugin, string name)
    {
        return plugin.GetPluginContext()!.MatchGroup[$"params_{name}"].Value;
    }

    /// <summary>
    /// 设置动作上下文
    /// </summary>
    public static void SetAtionContext(this ICommandPlugin plugin, Action action, int timeout = 15000)
    {
        var context = plugin.GetPluginContext();
        if (context == null) return;

        context.CurrentAction = action;
        context.TimeOut = timeout;
    }

    /// <summary>
    /// 添加消息进入缓存区 将在方法结束收统一发送
    /// </summary>
    /// <param name="plugin"></param>
    /// <param name="msg"></param>
    /// <param name="replys"></param>
    /// <param name="recallTime"></param>
    public static void ReplyToBuffer(this ICommandPlugin plugin, string msg, Replys replys = Replys.None, int recallTime = 0)
    {
        plugin.GetPluginContext().MsgBuffer?.Add(new()
        {
            Chain = new() { new PlainMessage(msg) },
            Mode = replys,
            RecallTime = recallTime
        });
    }

    /// <summary>
    /// 获取消息接收器
    /// </summary>
    public static MessageReceiverBase GetReceiver(this ICommandPlugin plugin)
    {
        return plugin.GetPluginContext()!.Receiver;
    }
}


/// <summary>
/// 指令插件上下文
/// </summary>
public class CommandPluginContext
{
    /// <summary>
    /// 关联用户Id
    /// </summary>
    public required string MemberId { get; init; }

    /// <summary>
    /// 指令动作类实例
    /// </summary>
    public required ICommandPlugin PluginInstance { get; init; }

    /// <summary>
    /// 插件元信息
    /// </summary>
    public required ICommandPluginTemplate PluginTempalte { get; init; }


    /// <summary>
    /// 初始动作
    /// </summary>
    public required Action FirstAction { get; set; }

    /// <summary>
    /// 当前动作
    /// </summary>
    public required Action? CurrentAction { get; set; }
    
    /// <summary>
    /// 上下文等待时长
    /// </summary>
    public int TimeOut { get; set; } = 0;

    /// <summary>
    /// 消息暂存区
    /// </summary>
    public List<MessageBlock> MsgBuffer { get; } = new();

    /// <summary>
    /// 消息接收器
    /// </summary>
    public required MessageReceiverBase Receiver { get; set; }

    /// <summary>
    /// 触发器匹配结果
    /// </summary>
    public required GroupCollection MatchGroup { get; set; }
}


public class MessageBlock
{
    public required MessageChain Chain { get; init; }

    public required Replys Mode { get; init; }

    public required int RecallTime { get; init; }
}