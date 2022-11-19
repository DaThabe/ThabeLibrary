namespace Thabe.Kit.API.Com.Huaban.Model.Query.Search;


/// <summary>
/// 搜索类型
/// </summary>
public enum SearchType
{
    /// <summary>
    /// 采集
    /// </summary>
    [HuabanName("pin")]
    Pin,

    /// <summary>
    /// 画板
    /// </summary>
    [HuabanName("board")]
    Board,

}

/// <summary>
/// 搜索排序类型
/// </summary>
public enum SearchSortType
{
    /// <summary>
    /// 综合排序
    /// </summary>
    [HuabanName("all")]
    All,

    /// <summary>
    /// 创建时间排序
    /// </summary>
    [HuabanName("created_at")]
    CreatedAt
}

/// <summary>
/// 搜索文件类型
/// </summary>
public enum SearchFileType
{
    [HuabanName("image/jpeg")]
    JPEG,

    [HuabanName("image/png")]
    PNG,

    [HuabanName("image/gif")]
    GIF,

    [HuabanName("image/jpg")]
    JPG,

    [HuabanName("image/bmp")]
    BMP
}

/// <summary>
/// 搜索颜色类型
/// </summary>
public enum SearchColorType
{
    /// <summary>
    /// 绿色
    /// </summary>
    [HuabanName("green")]
    Green,

    /// <summary>
    /// 黄色
    /// </summary>
    [HuabanName("yellow")]
    Yellow,

    /// <summary>
    /// 橙色
    /// </summary>
    [HuabanName("orange")]
    Orange,

    /// <summary>
    /// 红色
    /// </summary>
    [HuabanName("red")]
    Red,

    /// <summary>
    /// 黑色
    /// </summary>
    [HuabanName("black")]
    Black,

    /// <summary>
    /// 青色
    /// </summary>
    [HuabanName("cyan")]
    Cyan,

    /// <summary>
    /// 蓝色
    /// </summary>
    [HuabanName("blue")]
    Blue,

    /// <summary>
    /// 紫色
    /// </summary>
    [HuabanName("purple")]
    Purple,

    /// <summary>
    /// 棕色
    /// </summary>
    [HuabanName("brown")]
    Brown,

    /// <summary>
    /// 白色
    /// </summary>
    [HuabanName("white")]
    White
}

/// <summary>
/// 搜索类目类型
/// </summary>
public enum SearchCategoryType
{
    /// <summary>
    /// UI/UX
    /// </summary>
    [HuabanName("web_app_icon")]
    WebAppIcon,

    /// <summary>
    /// 平面设计
    /// </summary>
    [HuabanName("design")]
    Design,

    /// <summary>
    /// 插画/漫画
    /// </summary>
    [HuabanName("illustration")]
    Illustration,

    /// <summary>
    /// 摄影
    /// </summary>
    [HuabanName("photography")]
    Photography,

    /// <summary>
    /// 游戏
    /// </summary>
    [HuabanName("games")]
    Games,

    /// <summary>
    /// 动漫
    /// </summary>
    [HuabanName("anime")]
    Anime,

    /// <summary>
    /// 工业设计
    /// </summary>
    [HuabanName("industrial_design")]
    IndustrialDesign,

    /// <summary>
    /// 建筑设计
    /// </summary>
    [HuabanName("architecture")]
    Architecture,

    /// <summary>
    /// 人文艺术
    /// </summary>
    [HuabanName("art")]
    Art,

    /// <summary>
    /// 家居装修
    /// </summary>
    [HuabanName("home")]
    Home
}