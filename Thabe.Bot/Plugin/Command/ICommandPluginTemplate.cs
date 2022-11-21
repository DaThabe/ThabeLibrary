using Microsoft.CodeAnalysis.CSharp;

namespace Thabe.Bot.Plugin.Command;


/// <summary>
/// 指令插件模板接口
/// </summary>
public interface ICommandPluginTemplate
{
    /// <summary>
    /// 类型名称
    /// </summary>
    string TypeName { get; }

    /// <summary>
    /// 插件类 类型
    /// </summary>
    Type PluginType { get; }

    /// <summary>
    /// 指令插件元数据
    /// </summary>
    ICommandPluginMeta Meta { get; }


    /// <summary>
    /// 重载
    /// </summary>
    void Reload();

    /// <summary>
    /// 卸载
    /// </summary>
    void Unload();
}