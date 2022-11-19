namespace Thabe.Kit.API.Com.Huaban.Model.Query;


/// <summary>
/// 花瓣名称特性
/// </summary>
[AttributeUsage(AttributeTargets.All)]
internal class HuabanNameAttribute : Attribute
{
    public HuabanNameAttribute(string name) => Name = name;

    /// <summary>
    /// 花瓣名称
    /// </summary>
    public string Name { get; }
}
