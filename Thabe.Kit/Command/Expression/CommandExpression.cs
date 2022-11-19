using Newtonsoft.Json.Linq;
using Thabe.Kit.Command.Trigger;
using static Thabe.Kit.Command.Expression.CommandExpression;

namespace Thabe.Kit.Command.Expression;


/// <summary>
/// 指令接口
/// </summary>
public class CommandExpression
{
    /// <summary>
    /// 指令域名
    /// </summary>
    private CommandToken? Domain { get; init; }

    /// <summary>
    /// 指令触发符号
    /// </summary>
    public static string CommandSymbol { get; set; } = "/";


    //private CommandExpression() { }

    ///// <summary>
    ///// 创建表达式
    ///// </summary>
    ///// <param name="domain">指令域名</param>
    ///// <param name="alias">指令域别名</param>
    //public static CommandExpression Create(string? domain = null, params string[] alias)
    //{
    //    return domain is null ? new() : new() { Domain = new(domain, alias) };
    //}


    ///// <summary>
    ///// 设置一个指令
    ///// </summary>
    ///// <param name="name">指令名称</param>
    ///// <param name="alias">指令别名</param>
    //public Command SetCommand(string name, params string[] alias) => new(Domain, new(name, alias));

    //public interface IBuilder
    //{
    //    ITrigger Build();
    //}


    //public class Command : IBuilder
    //{
    //    private CommandToken? DomainName { get; init; }

    //    private CommandToken CommandName { get; init; }

    //    internal Command(CommandToken? domain, CommandToken name) => (DomainName, CommandName) = (domain, name);

    //    public IBuilder SetOption(string name, params string[] alias) => new OptionBuilder(this, new(name, alias));

    //    public IBuilder SetValue(string valueRegex) => new ValueBuilder(this, valueRegex);

    //    public ITrigger Build()
    //    {
    //        var cmd = CommandRegexString();

    //        return new RegexTrigger(PackRegex(cmd));
    //    }

    //    private string CommandRegexString()
    //    {
    //        var domain = GetRegexString(DomainName, "domain");
    //        var cmd = GetRegexString(CommandName, "cmd");

    //        if (!string.IsNullOrWhiteSpace(domain))
    //        {
    //            cmd = ComposeRegex(domain, cmd);
    //        }

    //        return cmd;
    //    }
    
    //    private class OptionBuilder :  IBuilder
    //    {
    //        private CommandToken OptionName;

    //        private Command Command;

    //        public OptionBuilder(Command cmd, CommandToken name) => (Command, OptionName) = (cmd, name);

    //        public ITrigger Build()
    //        {
    //            var cmd = CommandRegexString();

    //            return new RegexTrigger(PackCommandRegex(cmd));
    //        }

    //        private string CommandRegexString()
    //        {
    //            var cmd = Command.CommandRegexString();
    //            var opt = GetRegexString(OptionName, "opt");

    //            cmd = ComposeRegex(cmd, opt);

    //            return cmd;
    //        }
    //    }

    //    private class ValueBuilder : IBuilder
    //    {
    //        private string ValueRegex;

    //        private Command Command;

    //        public ValueBuilder(Command cmd, string valueRegex) => (Command, ValueRegex) = (cmd, valueRegex);

    //        public ITrigger Build()
    //        {
    //            var cmd = CommandRegexString();

    //            return new RegexTrigger(PackCommandRegex(cmd));
    //        }

    //        private string CommandRegexString()
    //        {
    //            var cmd = Command.CommandRegexString();

    //            cmd = ComposeRegex(cmd, ValueRegex);

    //            return cmd;
    //        }
    //    }
    //}


    /// <summary>
    /// 获取名称的正则表达式字符串  格式为: (?&lt;tag&gt;(name1|name2|name3))
    /// </summary>
    /// <param name="name"></param>
    /// <param name="tag"></param>
    /// <returns></returns>
    public static string GetRegexString(CommandToken? name = null, string? tag = null)
    {
        if (name == null) return "";

        if(!string.IsNullOrWhiteSpace(tag))
        {
            return $"(?<{tag}>({string.Join('|', name.GetTokens())}))";
        }

        return $"({string.Join('|', name.GetTokens())})";
    }

    /// <summary>
    /// 获取名称的正则表达式字符串  格式为: (?&lt;tag&gt;(name1|name2|name3))
    /// </summary>
    /// <param name="regex"></param>
    /// <param name="tag"></param>
    /// <returns></returns>
    public static string GetRegexString(string regex, string? tag = null)
    {
        if (!string.IsNullOrWhiteSpace(tag))
        {
            return $"(?<{tag}>({regex}))";
        }

        return $"({regex})";
    }


    /// <summary>
    /// 打包指令正则表达式  格式为:   ^\s*/\s*(inner)\s*$
    /// </summary>
    /// <param name="innerCommand"></param>
    /// <returns></returns>
    public static string PackRegex(CommandToken domain, string innerCommand)
    {
        var domain_regex = GetRegexString(domain);

        return @$"^\s*{CommandSymbol}(\s*{domain_regex}){{0,1}}\s*{innerCommand}\s*$";
    }

    /// <summary>
    /// 合成多个指令  中间用\s*拼接
    /// </summary>
    /// <param name="cmds"></param>
    /// <returns></returns>
    public static string ComposeRegex(params string[] cmds)
    {
        return string.Join(@"\s*", cmds);
    }
}


