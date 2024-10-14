using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

public static class QueryStringHelper
{
    public static string ToQueryString(object obj)
    {
        if (obj == null) return string.Empty;

        var properties = from p in obj.GetType().GetProperties()
                         where p.GetValue(obj, null) != null
                         select $"{HttpUtility.UrlEncode(p.Name)}={HttpUtility.UrlEncode(p.GetValue(obj, null).ToString())}";

        return "?" + string.Join("&", properties.ToArray());
    }
}
