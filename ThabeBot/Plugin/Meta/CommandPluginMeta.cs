namespace Thabe.ChatBot.Plugin.Meta;


/// <summary>
/// 指令插件元数据
/// </summary>
public class CommandPluginMeta : IPluginMeta
{
    /// <summary>
    /// 插件信息
    /// </summary>
    public required PluginInfo Info { get; init; }

    /// <summary>
    /// 插件数据
    /// </summary>
    public required CommandPluginData Data { get; init; }


    public void Reload()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 获取可执行动作名称
    /// </summary>
    /// <param name="cmd"></param>
    /// <returns></returns>
    public IEnumerable<(string MethodName, ITrigger Trigger)> GetActionNames(string cmd)
    {
        foreach (var i in Data.Commands)
        {
            if (i.Trigger.IsTrigger(cmd))
            {
                yield return (i.MethodName, i.Trigger);
            }
        }
    }


    /// <summary>
    /// 从Json配置文件加载
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static CommandPluginMeta FromJsonConfig(string json)
    {
        //反序列化json
        dynamic plugin_json = JsonConvert.DeserializeObject<JToken>(json) ?? throw new ArgumentNullException();

        //读取插件基础信息
        PluginInfo plugin_info = GetPluginInfo(plugin_json);

        //获取所有指令 
        var data = GetData(plugin_info, plugin_json);


        return new CommandPluginMeta() { Info = plugin_info, Data = data};
    }

    /// <summary>
    /// 从Json文件加载
    /// </summary>
    /// <param name="path">文件路径</param>
    public static CommandPluginMeta FromJsonFile(string path)
    {
        var json = File.ReadAllText(path);

        return FromJsonConfig(json);
    }


    /// <summary>
    /// 从JsonToken读取插件基础信息
    /// </summary>
    private static PluginInfo GetPluginInfo(dynamic jsonToken)
    {
        //读取插件id
        string plugin_id = jsonToken.id;
        //读取插件名称
        string plugin_name = jsonToken.name;
        //读取插件版本
        string plugin_ver = jsonToken.ver;
        //读取插件介绍
        string plugin_desc = jsonToken.desc;

        //读取插件域名
        CommandToken plugin_domain = new(
            (string)jsonToken.domain.name,
            jsonToken.domain.alias.ToObject<string[]>() ?? Array.Empty<string>());


        return new()
        {
            Id = plugin_id,
            Name = plugin_name,
            Version = plugin_ver,
            Describe = plugin_desc,
            Domain = plugin_domain
        };
    }

    /// <summary>
    /// 从JsonToken读取插件指令
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    static CommandPluginData GetData(PluginInfo pluginInfo, dynamic pluginJsonToken)
    {
        //读取定义的关键字
        var plugin_tokens = from i in (JToken)pluginJsonToken.tokens
                            let j_prop = i as JProperty
                            let name = j_prop.Name.ToString()
                            let alias = j_prop.Value.ToObject<string[]>() ?? throw new ArgumentNullException()
                            select new CommandToken(name, alias);

        //读取所有参数定义
        Dictionary<string, string> plugin_params = new(from i in (JToken)pluginJsonToken.@params
                                                       let j_prop = i as JProperty
                                                       let name = j_prop.Name.ToString()
                                                       let param = j_prop.Value.ToString()
                                                       select new KeyValuePair<string, string>(name, param));

        //实例化指令定义数据
        CommandPluginCommandDefine cmd_define = new() { Tokens = plugin_tokens, Params = new(plugin_params) };


        List<CommandTriggerMeta> plugin_cmds = new();

        //读取插件动作类型
        Type plugin_action_type = Type.GetType((string)pluginJsonToken.class_path) ?? throw new ArgumentNullException();


        //实例化所有指令
        foreach (dynamic cmd_token in (JToken)pluginJsonToken.commands ?? throw new ArgumentNullException())
        {
            var cmd = GetCommand(pluginInfo, cmd_define, plugin_action_type, cmd_token);

            plugin_cmds.Add(cmd);
        }


        return new() { Defines = cmd_define, Commands = plugin_cmds, ActionsClass = plugin_action_type };
    }

    /// <summary>
    /// 实例化指令
    /// </summary>
    /// <param name="pluginInfo"></param>
    /// <param name="cmdDefine"></param>
    /// <param name="actionType"></param>
    /// <param name="cmdJsonToken"></param>
    /// <returns></returns>
    static CommandTriggerMeta GetCommand(PluginInfo pluginInfo, CommandPluginCommandDefine cmdDefine, Type actionType, dynamic cmdJsonToken)
    {
        //指令名称
        string cmd_name = cmdJsonToken.name;
        //指令介绍
        string cmd_desc = cmdJsonToken.desc;
        //指令语法定义
        var cmd_expr_token_path = from token_str in (string[])cmdJsonToken.expr.ToObject<string[]>()
                                  let split = token_str.Split('.')
                                  select (split[0], split[1]);

        //指令动作名称
        string cmd_action_name = cmdJsonToken.action;


        //将token转化为正则表达式段
        List<string> token_regexs = new();

        foreach (var i in cmd_expr_token_path)
        {
            var tag = @$"{i.Item1}_{i.Item2}";

            if (match_str(i.Item1, "tokens"))
            {
                var token = cmdDefine.Tokens.FirstOrDefault(x => match_str(x.Name, i.Item2)) ?? throw new ArgumentNullException();
                token_regexs.Add(CommandExpression.GetRegexString(token, tag));

                continue;
            }
            else if (match_str(i.Item1, "params"))
            {
                KeyValuePair<string, string> token = cmdDefine.Params.FirstOrDefault(x => match_str(x.Key, i.Item2));
                token_regexs.Add(CommandExpression.GetRegexString(token.Value, tag));

                continue;
            }

            throw new ArgumentException();
        }

        //获取触发器
        var regex_str = CommandExpression.ComposeRegex(token_regexs.ToArray());
        var cmd_regex_str = CommandExpression.PackRegex(pluginInfo.Domain, regex_str);
        var cmd_trigger = new RegexTrigger(cmd_regex_str);


        return new() { MethodName = cmd_action_name, Describe = cmd_desc, Name = cmd_name, Trigger = cmd_trigger };

        //字符串忽略大小写比较
        static bool match_str(string a, string b) => a.Equals(b, StringComparison.InvariantCultureIgnoreCase);
    }
}


/// <summary>
/// 指令插件指令定义
/// </summary>
public class CommandPluginCommandDefine
{
    /// <summary>
    /// 关键字定义列表
    /// </summary>
    public required IEnumerable<CommandToken> Tokens { get; init; }

    /// <summary>
    /// 参数定义列表
    /// </summary>
    public required ReadOnlyDictionary<string, string> Params { get; init; }
}


/// <summary>
/// 指令插件数据
/// </summary>
public class CommandPluginData
{
    /// <summary>
    /// 插件动作类
    /// </summary>
    public required Type ActionsClass { get; init; }

    /// <summary>
    /// 指令参数定义
    /// </summary>
    public required CommandPluginCommandDefine Defines { get; init; }

    /// <summary>
    /// 指令列表
    /// </summary>
    public required IEnumerable<CommandTriggerMeta> Commands { get; init; }
}


/// <summary>
/// 指令触发器元信息
/// </summary>
public class CommandTriggerMeta : TriggerMeta
{
    /// <summary>
    /// 执行方法名称
    /// </summary>
    public required string MethodName { get; init; }
}