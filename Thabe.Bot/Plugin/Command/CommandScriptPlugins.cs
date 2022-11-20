using Flurl.Http;

namespace Thabe.Bot.Plugin.Command;


/// <summary>
/// 本地脚本插件
/// </summary>
public class LocalCommandScriptPlugin : ScriptPlugin
{
    private string FilePath { get; init; }

    public LocalCommandScriptPlugin(string filePath) => FilePath = filePath;

    protected override string ReloadSourceCode()
    {
        return File.ReadAllText(FilePath);
    }
}


/// <summary>
/// 网络脚本插件
/// </summary>
public class WebCommandScriptPlugin : ScriptPlugin
{
    private string FileUrl { get; init; }

    public WebCommandScriptPlugin(string url) => FileUrl = url;

    protected override string ReloadSourceCode()
    {
        return FileUrl.GetStringAsync().GetAwaiter().GetResult();
    }
}