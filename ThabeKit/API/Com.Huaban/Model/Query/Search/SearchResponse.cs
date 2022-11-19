using Thabe.Kit.API.Com.Huaban.Model.Converter;

namespace Thabe.Kit.API.Com.Huaban.Model.Query.Search;


/// <summary>
/// 搜索响应
/// </summary>
public class SearchResponse
{
    /// <summary>
    /// 采集数量
    /// </summary>
    [JsonProperty(PropertyName = "pin_count")]
    public long PinCount { get; set; }


    /// <summary>
    /// 采集数量
    /// </summary>
    [JsonProperty(PropertyName = "page")]
    public long Page { get; set; }


    /// <summary>
    /// 采集列表
    /// </summary>
    [JsonProperty(PropertyName = "pins")]
    public Pin[] Pins { get; set; } = Array.Empty<Pin>();
}