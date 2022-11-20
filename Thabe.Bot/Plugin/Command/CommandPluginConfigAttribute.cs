namespace Thabe.Bot.Plugin.Command;



/// <summary>
/// 指令特性
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class CommandPluginConfigAttribute : Attribute, ICommandPluginMeta
{
    #region --属性--

    /// <summary>
    /// 插件唯一Id
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// 插件名称
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// 插件描述
    /// </summary>
    public string Describe { get; }

    /// <summary>
    /// 插件版本
    /// </summary>
    public string Version { get; }

    /// <summary>
    /// 插件域名
    /// </summary>
    public CommandToken Domain { get; }

    /// <summary>
    /// 关键字定义列表
    /// </summary>
    public IEnumerable<CommandToken> Tokens { get; }

    /// <summary>
    /// 参数定义列表
    /// </summary>
    public ReadOnlyDictionary<string, string> Params { get; }

    /// <summary>
    /// 指令列表
    /// </summary>
    public IEnumerable<CommandMeta> Commands { get; }

    #endregion

    public CommandPluginConfigAttribute(string configJson)
    {
        //反序列化json
        dynamic plugin_json = JsonConvert.DeserializeObject<JToken>(configJson) ?? throw new ArgumentNullException();

        #region --基础信息--

        //读取插件id
        Id = plugin_json.id;
        //读取插件名称
        Name = plugin_json.name;
        //读取插件版本
        Version = plugin_json.ver;
        //读取插件介绍
        Describe = plugin_json.desc;

        //读取插件域名
        Domain = new((string)plugin_json.domain.name,
            plugin_json.domain.alias.ToObject<string[]>() ?? Array.Empty<string>());

        #endregion

        #region --加载需要解析的数据--

        //读取定义的关键字
        Tokens = from i in (JToken)plugin_json.tokens
                 let j_prop = i as JProperty
                 let name = j_prop.Name.ToString()
                 let alias = j_prop.Value.ToObject<string[]>() ?? throw new ArgumentNullException()
                 select new CommandToken(name, alias);

        //读取所有参数定义
        Params = new(new Dictionary<string, string>(from i in (JToken)plugin_json.@params
                                                    let j_prop = i as JProperty
                                                    let name = j_prop.Name.ToString()
                                                    let param = j_prop.Value.ToString()
                                                    select new KeyValuePair<string, string>(name, param)));

        //所有指令
        List<CommandMeta> plugin_cmds = new();

        //实例化所有指令
        foreach (dynamic cmd_token in (JToken)plugin_json.commands ?? throw new ArgumentNullException())
        {
            var cmd = load_command(cmd_token);
            plugin_cmds.Add(cmd);
        }

        Commands = plugin_cmds;


        #endregion

        /// <summary>
        /// 实例化指令
        /// </summary>
        /// <param name="pluginInfo"></param>
        /// <param name="cmdDefine"></param>
        /// <param name="actionType"></param>
        /// <param name="cmdJsonToken"></param>
        /// <returns></returns>
        CommandMeta load_command(dynamic cmdJsonToken)
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
                    var token = Tokens.FirstOrDefault(x => match_str(x.Name, i.Item2)) ?? throw new ArgumentNullException();
                    token_regexs.Add(CommandExpression.GetRegexString(token, tag));

                    continue;
                }
                else if (match_str(i.Item1, "params"))
                {
                    KeyValuePair<string, string> token = Params.FirstOrDefault(x => match_str(x.Key, i.Item2));
                    token_regexs.Add(CommandExpression.GetRegexString(token.Value, tag));

                    continue;
                }

                throw new ArgumentException();
            }

            //获取触发器
            var regex_str = CommandExpression.ComposeRegex(token_regexs.ToArray());
            var cmd_regex_str = CommandExpression.PackRegex(Domain, regex_str);
            var cmd_trigger = new RegexTrigger(cmd_regex_str);


            return new() { MethodName = cmd_action_name, Describe = cmd_desc, Name = cmd_name, Trigger = cmd_trigger };

            //字符串忽略大小写比较
            static bool match_str(string a, string b) => a.Equals(b, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}


/// <summary>
/// 指令触发器元信息
/// </summary>
public class CommandMeta
{
    /// <summary>
    /// 名称
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// 描述
    /// </summary>
    public required string Describe { get; init; }

    /// <summary>
    /// 触发器
    /// </summary>
    public required ITrigger Trigger { get; init; }

    /// <summary>
    /// 执行方法名称
    /// </summary>
    public required string MethodName { get; init; }
}