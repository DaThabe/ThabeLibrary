namespace Thabe.Bot.Plugin.Command;


/// <summary>
/// 指令插件元数据接口
/// </summary>
public interface ICommandPluginMeta : IPluginMeta
{
    /// <summary>
    /// 关键字定义列表
    /// </summary>
    IEnumerable<CommandToken> Tokens { get; }

    /// <summary>
    /// 参数定义列表
    /// </summary>
    ReadOnlyDictionary<string, string> Params { get; }

    /// <summary>
    /// 指令列表
    /// </summary>
    IEnumerable<CommandMeta> Commands { get; }
}
