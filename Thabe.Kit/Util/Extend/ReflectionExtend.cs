namespace Thabe.Kit.Util.Extend;

internal static class ReflectionExtend
{
    /// <summary>
    /// 获取方法参数信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="methodName"></param>
    /// <returns></returns>
    public static ParameterInfo[]? GetParameterInfo<T>(this string methodName)
    {
        return typeof(T).GetMethod(methodName)?.GetParameters();
    }
}
