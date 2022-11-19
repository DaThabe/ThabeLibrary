namespace Thabe.Kit.API.Com.Huaban.Model;


/// <summary>
/// 用户信息
/// </summary>
public class User
{
    /// <summary>
    /// 用户Id
    /// </summary>
    [JsonProperty(PropertyName = "user_id")]
    public long ID { get; set; }


    /// <summary>
    /// 用户名称
    /// </summary>
    [JsonProperty(PropertyName = "username")]
    public string Name { get; set; } = "";


    /// <summary>
    /// 用户名称
    /// </summary>
    [JsonProperty(PropertyName = "urlname")]
    public string UrlName { get; set; } = "";
}