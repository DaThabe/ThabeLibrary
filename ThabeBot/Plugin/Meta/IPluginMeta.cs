namespace Thabe.ChatBot.Plugin.Meta;


/// <summary>
/// 插件元数据接口
/// </summary>
public interface IPluginMeta
{
    /// <summary>
    /// 插件信息
    /// </summary>
    PluginInfo Info { get; }

    /// <summary>
    /// 重新加载
    /// </summary>
    void Reload();
}


/// <summary>
/// 插件基础信息
/// </summary>
public class PluginInfo
{
    /// <summary>
    /// 插件唯一Id
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// 插件名称
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// 插件描述
    /// </summary>
    public required string Describe { get; init; }

    /// <summary>
    /// 插件版本
    /// </summary>
    public required string Version { get; init; }

    /// <summary>
    /// 插件域名
    /// </summary>
    public required CommandToken Domain { get; init; }
}