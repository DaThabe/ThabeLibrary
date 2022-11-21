using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Runtime.Loader;
using System.Collections.ObjectModel;

namespace Thabe.Bot.Plugin.Command;


/// <summary>
/// 脚本插件基类
/// </summary>
public abstract class ScriptPlugin : ICommandPluginTemplate
{
    #region --字段 属性--

    /// <summary>
    /// 重载锁
    /// </summary>
    private readonly object _relaodLock = new();

    /// <summary>
    /// 插件程序集
    /// </summary>
    private AssemblyLoadContext PluginAssembly;


    public required string TypeName { get; init; }

    public Type PluginType { get; private set; }

    public ICommandPluginMeta Meta { get; private set; }

    #endregion

    public void Reload()
    {
        lock (_relaodLock)
        {
            //加载源码并且编译成程序集
            var code = ReloadSourceCode();
            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var (assembly, type) = GetPlugin(TypeName, syntaxTree);
            var meta = type.GetCustomAttribute<CommandPluginConfigAttribute>();
            if(meta == null)
            {
                throw new ArgumentNullException();
            }

            //刷新数据
            Unload();

            PluginAssembly = assembly;
            PluginType = type;
            Meta = meta;
        }
    }

    public void Unload()
    {
        try
        {
            if(PluginType!= null) GC.SuppressFinalize(PluginType);
            if (Meta != null) GC.SuppressFinalize(Meta);

            if(PluginAssembly != null)
            {
                PluginAssembly.Unload();
                GC.SuppressFinalize(PluginAssembly);
            }
        }
        catch (Exception e)
        {
            Log.Write(e);
        }
    }


    /// <summary>
    /// 重载源代码
    /// </summary>
    /// <returns></returns>
    protected abstract string ReloadSourceCode();


    /// <summary>
    /// 获取插件程序集上下文
    /// </summary>
    /// <param name="originalClassName">插件类名称</param>
    /// <param name="syntaxTree">插件源文件代码</param>
    /// <exception cref="Exception"></exception>
    private static (AssemblyLoadContext Context, Type Plugin) GetPlugin(string originalClassName, SyntaxTree syntaxTree)
    {
        AssemblyLoadContext laoder = new(originalClassName);

        ///动态库类型编译
        var dll = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);

        // 这算是偷懒了吗？我把.NET Core 运行时用到的那些引用都加入到引用了。
        // 加入引用是必要的，不然连 object 类型都是没有的，肯定编译不通过。
        var references = from i in AppDomain.CurrentDomain.GetAssemblies()
                         where !string.IsNullOrEmpty(i.Location)
                         select MetadataReference.CreateFromFile(i.Location);

        var mirai_ref = MetadataReference.CreateFromFile(typeof(ScriptPlugin).Assembly.Location);

        // 指定编译选项。
        var assemblyName = $"{originalClassName}.g";
        var compilation = CSharpCompilation.Create(assemblyName, new[] { syntaxTree }, options: dll)
            .AddReferences(references).AddReferences(mirai_ref);

        // 编译到内存流中。
        using MemoryStream ms = new();
        var result = compilation.Emit(ms);

        //编译失败
        if (!result.Success)
        {
            throw new Exception(string.Join(Environment.NewLine, result.Diagnostics));
        }

        ms.Seek(0, SeekOrigin.Begin);
        var assembly = laoder.LoadFromStream(ms);
        var plugin_type = assembly.GetTypes().FirstOrDefault(x => x.Name == originalClassName);

        if (plugin_type != null)
        {
            return (laoder, plugin_type);
        }

        throw new ArgumentNullException();
    }
}