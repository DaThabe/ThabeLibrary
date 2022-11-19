using System.Security.Cryptography;

namespace Thabe.Kit.Command.Expression;


/// <summary>
/// 指令关键字
/// </summary>
public class CommandToken
{
    private static readonly Regex NameRegex = new(@"^\w+$", RegexOptions.IgnoreCase);


    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// 别名列表
    /// </summary>
    public string[] Alias { get; init; }



    /// <summary>
    /// 创建一个别名
    /// </summary>
    /// <param name="name"></param>
    /// <param name="alias"></param>
    public CommandToken(string name, params string[] alias)
    {
        name = CheckName(name);
        alias = (from i in alias select CheckName(i)).ToArray();

        (Name, Alias) = (name, alias);
    }

    /// <summary>
    /// 获取所有名称关键字
    /// </summary>
    /// <returns></returns>
    public string[] GetTokens()
    {
        List<string> tokens = new() { Name };

        if(Alias != null) tokens.AddRange(Alias);

        return tokens.ToArray();
    }
    

    private static string CheckName(string name)
    {
        name = name.Trim();

        if(NameRegex.IsMatch(name) == false)
        {
            Console.WriteLine($"名称含有不支持的字符 可能会导致无法识别 请重新命名:> {name}");
        }

        return name;
    }
}
