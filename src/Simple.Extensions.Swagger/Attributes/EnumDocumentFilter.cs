
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel;
using System.Reflection;

namespace LonCloud.Extensions.Swagger.Attributes
{
    public class EnumDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (var item in swaggerDoc.Components.Schemas)
            {
                var property = item.Value;
                if (property.Enum is not null && property.Enum.Count > 0)
                {
                    var enums = property.Enum.Select(item => (OpenApiInteger)item).ToList();
                    property.Description += DescribeEnum(enums);
                }
            }
        }

        /// <summary>
        /// 描述枚举
        /// </summary>
        /// <param name="enums"></param>
        /// <returns></returns>
        private static string DescribeEnum(IEnumerable<object> enums)
        {
            var enumDescriptions = new List<string>();
            Type? type = default;
            foreach (var enumOption in enums)
            {
                if (type == null) 
                    type = enumOption.GetType();

                enumDescriptions.Add($"{Convert.ChangeType(enumOption, type.GetEnumUnderlyingType())} = {Enum.GetName(type, enumOption)}，{GetDescription(type, enumOption)}");
            }
            return $"{Environment.NewLine}{string.Join(Environment.NewLine, enumDescriptions)}";
        }

        /// <summary>
        /// 获取描述
        /// </summary>
        /// <param name="t"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string GetDescription(Type t, object value)
        {
            foreach (MemberInfo mInfo in t.GetMembers())
            {
                if (mInfo.Name == t.GetEnumName(value))
                {
                    foreach (Attribute attr in Attribute.GetCustomAttributes(mInfo))
                    {
                        if (attr.GetType() == typeof(DescriptionAttribute))
                        {
                            return ((DescriptionAttribute)attr).Description;
                        }
                    }
                }
            }
            return string.Empty;
        }
    }
}
