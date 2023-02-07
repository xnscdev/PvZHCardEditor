using System;

namespace PvZHCardEditor
{
    internal static class Extensions
    {
        public static T? GetAttribute<T>(this Enum @enum) where T : Attribute
        {
            var type = @enum.GetType();
            var info = type.GetMember(@enum.ToString());
            var attr = info[0].GetCustomAttributes(typeof(T), false);
            return attr.Length > 0 ? (T)attr[0] : null;
        }

        public static string GetInternalKey(this Enum @enum)
        {
            return GetAttribute<InternalKeyAttribute>(@enum)?.Key ?? @enum.ToString();
        }
    }
}
