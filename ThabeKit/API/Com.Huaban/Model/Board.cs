namespace Thabe.Kit.API.Com.Huaban.Model;


/// <summary>
/// 画板
/// </summary>
public class Board
{
    /// <summary>
    /// 画板Id
    /// </summary>
    [JsonProperty(PropertyName = "board_id")]
    public long Id { get; set; }

    /// <summary>
    /// 所属用户Id
    /// </summary>
    [JsonProperty(PropertyName = "user_id")]
    public long UserId { get; set; }

    /// <summary>
    /// 花板标题
    /// </summary>
    [JsonProperty(PropertyName = "title")]
    public string Title { get; set; } = "";


    /// <summary>
    /// 花板描述
    /// </summary>
    [JsonProperty(PropertyName = "description")]
    public string Describe { get; set; } = "";
}