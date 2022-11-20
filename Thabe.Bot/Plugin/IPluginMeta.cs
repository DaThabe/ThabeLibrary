namespace Thabe.Bot.Plugin;


/// <summary>
/// 插件元数据接口
/// </summary>
public interface IPluginMeta
{
    /// <summary>
    /// 插件唯一Id
    /// </summary>
    string Id { get; }

    /// <summary>
    /// 插件名称
    /// </summary>
    string Name { get; }

    /// <summary>
    /// 插件描述
    /// </summary>
    string Describe { get; }

    /// <summary>
    /// 插件版本
    /// </summary>
    string Version { get; }

    /// <summary>
    /// 插件域名
    /// </summary>
    CommandToken Domain { get; }
}