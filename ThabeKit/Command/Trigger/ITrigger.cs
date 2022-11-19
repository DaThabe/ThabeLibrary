namespace Thabe.Kit.Command.Trigger;


/// <summary>
/// 指令触发器接口
/// </summary>
public interface ITrigger
{
    /// <summary>
    /// 是否可以触发
    /// </summary>
    /// <param name="rawCommand">指令字符串</param>
    /// <returns></returns>
    bool IsTrigger(string rawCommand);
}
