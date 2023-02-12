using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Windows.Controls;

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

        public static EditValueType GetEditValueType(this JTokenType tokenType)
        {
            return tokenType switch
            {
                JTokenType.Object => EditValueType.Object,
                JTokenType.Array => EditValueType.Array,
                JTokenType.Integer => EditValueType.Integer,
                JTokenType.String => EditValueType.String,
                JTokenType.Boolean => EditValueType.Boolean,
                JTokenType.Null => EditValueType.Null,
                _ => throw new NotImplementedException()
            };
        }
    }
}
