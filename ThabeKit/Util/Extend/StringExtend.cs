namespace Thabe.Kit.Util.Extend;


/// <summary>
/// 字符串扩展方法
/// </summary>
public static class StringExtend
{
    /// <summary>
    /// Url解码
    /// </summary>
    public static string UrlDecode(this string url) => HttpUtility.UrlDecode(url);

    /// <summary>
    /// Url编码
    /// </summary>
    public static string UrlEncode(this string url) => HttpUtility.UrlEncode(url);


    /// <summary>
    /// 修剪字符串
    /// </summary>
    /// <param name="trim">修建类型</param>
    /// <returns></returns>
    public static string Trim(this string str, StringTrim trim)
    {
        if(trim == StringTrim.Front)
        {
            int index = 0;

            for (; index < str.Length; index++)
            {
                if (str[index] != ' ') break;
            }

            return str[index..];
        }
        else if(trim == StringTrim.Behind)
        {
            int index = str.Length - 1;

            for (; index >= 0; index--)
            {
                if (str[index] != ' ') break;
            }

            return str[..index];
        }

        return str.Trim();
    }

    /// <summary>
    /// 获取一个单词
    /// </summary>
    /// <param name="str"></param>
    /// <param name="trim"></param>
    /// <returns></returns>
    public static string OnceWord(this string str, StringTrim trim = StringTrim.Front)
    {
        str = str.Trim(trim);

        if (trim == StringTrim.Front)
        {
            int index = 0;

            for (; index < str.Length; index++)
            {
                if (str[index] == ' ') break;
            }

            return str[..index];
        }
        else if(trim == StringTrim.Behind)
        {
            int index = str.Length - 1;

            for (; index >= 0 ; index--)
            {
                if (str[index] == ' ') break;
            }

            return str[index..];
        }

        return str;
    }

    /// <summary>
    /// 弹出一个单词
    /// </summary>
    /// <param name="str"></param>
    /// <param name="trim"></param>
    /// <returns></returns>
    public static (string Word, string str) PopOnceWord(this string str, StringTrim trim = StringTrim.Front)
    {
        str = str.Trim(trim);

        if (trim == StringTrim.Front)
        {
            int index = 0;

            for (; index < str.Length; index++)
            {
                if (str[index] == ' ') break;
            }

            return (str[..index], str[index..]);
        }
        else if (trim == StringTrim.Behind)
        {
            int index = str.Length - 1;

            for (; index >= 0; index--)
            {
                if (str[index] == ' ') break;
            }

            return (str[index..], str[..index]);
        }

        return (str, str);
    }
}

public enum StringTrim
{
    /// <summary>
    /// 前面
    /// </summary>
    Front,

    /// <summary>
    /// 后面
    /// </summary>
    Behind,
}
