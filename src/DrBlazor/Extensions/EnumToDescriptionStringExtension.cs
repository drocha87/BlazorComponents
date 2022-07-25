using System.ComponentModel;

namespace DrBlazor;

public static class EnumExtension
{
    public static string ToDescriptionString<T>(this T @enum) where T : Enum
    {
        var fieldInfo = @enum.GetType().GetField(@enum.ToString());
        if (fieldInfo is not null)
        {
            DescriptionAttribute[] attrs = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attrs.Length > 0 ? attrs[0].Description : string.Empty;
        }
        return string.Empty;
    }
}
