using System.Web;

namespace NewsSite.DAL.Extensions;

public static class StringExtensions
{
    public static string? GetQueryParameterFromUrl(this string? url, string queryParameterName)
    {
        if (string.IsNullOrEmpty(url))
        {
            return string.Empty;
        }
        
        var query = url.Split("?")[1];
        
        return HttpUtility.ParseQueryString(query).Get(queryParameterName);
    }
}