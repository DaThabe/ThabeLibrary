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
        try
        {
            return File.ReadAllText(FilePath);
        }
        catch(Exception e)
        {
            Log.Write(e);
            throw;
        }
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
        try
        {
            Log.Write($"正在加载网络脚本插件: [{FileUrl}]");
            var code = FileUrl.GetStringAsync().GetAwaiter().GetResult();

            Log.Write($"网络脚本插件下载成功: [{FileUrl}]");
            return code;

        }
        catch (Exception e)
        {
            Log.Write(e);
            throw;
        }
    }
}