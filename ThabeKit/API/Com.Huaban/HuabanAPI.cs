using Thabe.Kit.API.Com.Huaban.Model;
using Thabe.Kit.API.Com.Huaban.Model.Query;
using Thabe.Kit.API.Com.Huaban.Model.Query.Search;

namespace Thabe.Kit.API.Com.Huaban;


/// <summary>
/// <a href="https://huaban.com/">花瓣</a>API
/// </summary>
public static class HuabanAPI
{
    /// <summary>
    /// 花瓣API host
    /// </summary>
    private const string API_HOST = "https://api.huaban.com/";

    /// <summary>
    /// 搜索采集 
    /// </summary>
    /// <param name="query">查询内容</param>
    /// <param name="sort">排序方式</param>
    /// <param name="perPage">页面采集数</param>
    /// <param name="Page">页面数</param>
    /// <param name="hideOtherCount"></param>
    /// <param name="fileType">文件类型</param>
    /// <param name="color">颜色类型</param>
    /// <param name="category">类目=</param>
    /// <param name="material">是否显示收费素材</param>
    public static async Task<SearchResponse> Search(
        string query,
        SearchSortType sort = SearchSortType.All,
        int perPage = 20,
        int Page = 1,
        bool hideOtherCount = true,
        SearchFileType? fileType = null,
        SearchColorType? color = null,
        SearchCategoryType? category = null,
        bool? material = null
        )
    {
        string url = MergeUrl();
        return await url.GetJsonAsync<SearchResponse>();

        string MergeUrl()
        {
            List<string> query_params = new()
            {
                $"q={query.UrlEncode()}",
                $"sort={sort.GetHuabanNameToUrlName()}",
                $"per_page={perPage}",
                $"page={Page}",
                $"hide_other_count={(hideOtherCount ? 1 : 0)}",
            };

            if (fileType != null) query_params.Add($"file_type={fileType.GetHuabanNameToUrlName()}");

            if (color != null) query_params.Add($"color={color.GetHuabanNameToUrlName()}");

            if (category != null) query_params.Add($"category={category.GetHuabanNameToUrlName()}");

            if (material is not null) query_params.Add($"material={material.ToString().ToLower()}");


            return $"{API_HOST}search?{string.Join("&", query_params)}";
        };
    }

    /// <summary>
    /// 搜索提示
    /// <a href=“https://api.huaban.com/search/hint?limit=10&q=123”>测试API</a>
    /// </summary>
    /// <returns></returns>
    public static async Task<string[]> SearchHint(string query, int limit = 10)
    {
        string url = MergeUrl();
        return await url.GetJsonAsync();


        string MergeUrl()
        {
            List<string> query_params = new()
            {
                $"q={query.UrlEncode()}",
                $"limit={limit}"
            };

            return $"{API_HOST}search/hint?{string.Join("&", query_params)}";
        }
    }



    public static async Task<string> DownloadAsync(this Pin pin, string savePath)
    {
        return "";
    }


    /// <summary>
    /// 获取花瓣名称
    /// </summary>
    private static string? GetHuabanName(this Enum @enum)
    {
        var type = @enum.GetType();
        var fi = type.GetField(@enum.ToString());
        var att = fi?.GetCustomAttribute<HuabanNameAttribute>();

        return att?.Name;
    }

    /// <summary>
    /// 获取花瓣名称并转为Url名称
    /// </summary>
    private static string? GetHuabanNameToUrlName(this Enum @enum)
    {
        return @enum.GetHuabanName()?.UrlEncode();
    }
}
