namespace Thabe.Kit.API.Com.Huaban.Model;

/// <summary>
/// 采集信息
/// </summary>
public class Pin
{
    /// <summary>
    /// 采集Id
    /// </summary>
    [JsonProperty(PropertyName = "pin_id")]
    public long Id { get; set; }

    /// <summary>
    /// 用户Id
    /// </summary>
    [JsonProperty(PropertyName = "user_id")]
    public long UserID { get; set; }

    /// <summary>
    /// 采集所属花瓣Id
    /// </summary>
    [JsonProperty(PropertyName = "board_id")]
    public long BoardId { get; set; }


    /// <summary>
    /// 文件Id
    /// </summary>
    [JsonProperty(PropertyName = "file_id")]
    public long FileId { get; set; }

    /// <summary>
    /// 文件信息
    /// </summary>
    [JsonProperty(PropertyName = "file")]
    public PinFile File { get; set; } = new();

    /// <summary>
    /// 描述
    /// </summary>
    [JsonProperty(PropertyName = "raw_text")]
    public string Describe { get; set; } = "";


    /// <summary>
    /// 标签
    /// </summary>
    [JsonProperty(PropertyName = "tags")]
    public string[] Tag { get; set; } = Array.Empty<string>();


    /// <summary>
    /// 用户信息
    /// </summary>
    [JsonProperty(PropertyName = "user")]
    public User User { get; set; } = new();


    /// <summary>
    /// 画板信息
    /// </summary>
    [JsonProperty(PropertyName = "board")]
    public Board Board { get; set; } = new();
}
