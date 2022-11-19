namespace Thabe.ChatBot.Plugin;


/// <summary>
/// 指令插件接口
/// </summary>
public interface ICommandsPlugin : IPlugin
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
    private static readonly List<CommandPluginMeta> _plugins = new();

    /// <summary>
    /// 用户对话上下文
    /// </summary>
    private static readonly Dictionary<string, CommandPluginContext> _memberContexts = new();


    /// <summary>
    /// 注册插件
    /// </summary>
    /// <exception cref="Exception"></exception>
    public static void Register(this CommandPluginMeta pluginMeta)
    {
        if(_plugins.Any(x => x.Info.Id.Equals(pluginMeta.Info.Id, StringComparison.InvariantCultureIgnoreCase)))
        {
            throw new Exception("插件id冲突");
        }

        _plugins.Add(pluginMeta);
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

            //如果下来没有动作则移除上下文
            if(context.CurrentAction == null)
            {
                context.CurrentAction = context.FirstAction;
                _memberContexts.Remove(id);
            }

            return;
        }


        //获取用户输入的字符串
        var cmd = receiver.MessageChain.GetPlainMessage();
        if (string.IsNullOrWhiteSpace(cmd)) return;

        //获取所有可执行动作
        var all_excute_action = (from cmd_plugin in _plugins
                                 let action_names = cmd_plugin.GetActionNames(cmd)
                                 where action_names is not null
                                 from name in action_names
                                 select (name.MethodName, cmd_plugin, name.Trigger)).ToArray();

        //获取第一个可执行动作
        var excute_action = all_excute_action.FirstOrDefault();
        if (excute_action == (null, null, null)) return;

        //获取指令动作类型实例
        var type = excute_action.cmd_plugin.Data.ActionsClass;
        if (Activator.CreateInstance(type) is not ICommandsPlugin instance) return;

        //获取第一个动作方法实例
        var method = type.GetTypeInfo().DeclaredMethods.FirstOrDefault(x =>
        {
            return x.Name.Equals(excute_action.MethodName, StringComparison.InvariantCultureIgnoreCase);
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
            PluginMeta = excute_action.cmd_plugin,
            MatchGroup = ((RegexTrigger)excute_action.Trigger).GetMatchGroup(cmd)
        };
        first_action();

        //瞬态对话
        if(_memberContexts[id].CurrentAction == _memberContexts[id].FirstAction)
        {
            _memberContexts.Remove(id);
        }

        return;

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
    public static CommandPluginContext? GetPluginContext(this ICommandsPlugin plugin)
    {
        return _memberContexts.FirstOrDefault(x => x.Value.PluginInstance == plugin).Value;
    }


    /// <summary>
    /// 获取输入的关键字
    /// </summary>
    /// <param name="plugin"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string GetInputToken(this ICommandsPlugin plugin, string name)
    {
        return plugin.GetPluginContext()!.MatchGroup[$"tokens_{name}"].Value;
    }

    /// <summary>
    /// 获取输入的参数
    /// </summary>
    public static string GetInputParam(this ICommandsPlugin plugin, string name)
    {
        return plugin.GetPluginContext()!.MatchGroup[$"params_{name}"].Value;
    }

    /// <summary>
    /// 设置动作上下文
    /// </summary>
    public static void SetAtionContext(this ICommandsPlugin plugin, Action action)
    {
        var context = plugin.GetPluginContext();
        if (context == null) return;

        context.CurrentAction = action;
    }

    /// <summary>
    /// 获取消息接收器
    /// </summary>
    public static MessageReceiverBase GetReceiver(this ICommandsPlugin plugin)
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
    public required ICommandsPlugin PluginInstance { get; init; }

    /// <summary>
    /// 插件元信息
    /// </summary>
    public required CommandPluginMeta PluginMeta { get; init; }

    /// <summary>
    /// 初始动作
    /// </summary>
    public required Action FirstAction { get; set; }


    /// <summary>
    /// 当前动作
    /// </summary>
    public required Action? CurrentAction { get; set; }

    /// <summary>
    /// 消息接收器
    /// </summary>
    public required MessageReceiverBase Receiver { get; set; }

    /// <summary>
    /// 触发器匹配结果
    /// </summary>
    public required GroupCollection MatchGroup { get; set; }
}