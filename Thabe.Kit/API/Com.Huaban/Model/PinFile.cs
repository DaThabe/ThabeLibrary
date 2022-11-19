namespace Thabe.Kit.API.Com.Huaban.Model;

public class PinFile
{
    /// <summary>
    /// 图片Key
    /// </summary>
    [JsonProperty(PropertyName = "key")]
    public string Key { get; set; } = "";

    /// <summary>
    /// 文件类型
    /// </summary>
    [JsonProperty(PropertyName = "type")]
    public string Type { get; set; } = "";


    /// <summary>
    /// 图片宽度
    /// </summary>
    [JsonProperty(PropertyName = "width")]
    public int Width { get; set; }

    /// <summary>
    /// 图片高度
    /// </summary>
    [JsonProperty(PropertyName = "height")]
    public int Height { get; set; }

    /// <summary>
    /// 图片帧数?
    /// </summary>
    [JsonProperty(PropertyName = "frames")]
    public int Frame { get; set; }


    /// <summary>
    /// 主题色
    /// </summary>
    [JsonProperty(PropertyName = "theme")]
    public string ThemeColorHex { get; set; } = "";
}
