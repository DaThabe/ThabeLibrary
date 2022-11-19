namespace Thabe.Kit.Command.Trigger;


/// <summary>
/// 正则表达式指令触发器
/// </summary>
public class RegexTrigger : ITrigger
{
    private Regex Regex { get; init; }

    /// <summary>
    /// 用正则表达式对象初始化
    /// </summary>
    public RegexTrigger(Regex regex) => Regex = regex;

    /// <summary>
    /// 用正则表达式字符串初始化 并且默认忽略大小写
    /// </summary>
    public RegexTrigger(string regexString, RegexOptions options = RegexOptions.IgnoreCase) 
        => Regex = new(regexString, options);

    public bool IsTrigger(string rawCommand) => Regex.IsMatch(rawCommand);

    public GroupCollection GetMatchGroup(string rawCommand)
    {
        return Regex.Match(rawCommand).Groups;
    }
}
