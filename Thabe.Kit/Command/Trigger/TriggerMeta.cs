namespace Thabe.Kit.Command.Trigger;


/// <summary>
/// 触发器元信息
/// </summary>
public class TriggerMeta
{
    /// <summary>
    /// 名称
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// 描述
    /// </summary>
    public required string Describe { get; init; }

    /// <summary>
    /// 触发器
    /// </summary>
    public required ITrigger Trigger { get; init; }
}
