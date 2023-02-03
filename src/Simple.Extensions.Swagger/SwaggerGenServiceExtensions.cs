using LonCloud.Extensions.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Simple.Extensions.Swagger
{
    /// <summary>
    /// swagger扩展
    /// </summary>
    public static class SwaggerGenServiceExtensions
    {
        /// <summary>
        /// swagger 文档扩展注册
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection AddSwaggerGenGroup(this IServiceCollection services, Type? groupTypes = null)
        {
            groupTypes = groupTypes ?? typeof(GroupTypes);

            services.AddSwaggerGen(c =>
            {
                groupTypes.GetFields().Skip(1).ToList().ForEach(fieldInfo =>
                {
                    var groupInfo = fieldInfo.GetCustomAttributes(typeof(ApiGroupInfoAttribute), false)
                        .OfType<ApiGroupInfoAttribute>().FirstOrDefault();

                    c.SwaggerDoc(fieldInfo.Name, new()
                    {
                        Title = groupInfo?.Title,
                        Version = groupInfo?.Version,
                        Description = groupInfo?.Description
                    });
                });
                c.DocInclusionPredicate((docName, description) => true);
                c.DocInclusionPredicate((docName, apiDescription) =>
                {
                    if (docName == GroupTypes.Default.ToString())
                        return string.IsNullOrEmpty(apiDescription.GroupName);

                    return apiDescription.GroupName == docName;
                });

                //解决相同类名会报错的问题
                c.CustomSchemaIds(type => type.FullName);
                Directory.GetFiles(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "*.xml").ToList()
                    .ForEach(xml => c.IncludeXmlComments(xml, true));

                #region 启用swagger验证功能

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "bearer",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = ""
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "Bearer"}
                        },
                        new List<string>()
                    }
                });

                #endregion
            });

            return services;
        }

        /// <summary>
        /// 用提供的选项注册SwaggerUI分组中间件
        /// </summary>
        /// <returns></returns>
        public static IApplicationBuilder UseSwaggerUiGroup(this IApplicationBuilder app, Type? groupTypes = null)
        {
            groupTypes = groupTypes ?? typeof(GroupTypes);
            app.UseSwaggerUI(c =>
            {
                groupTypes.GetFields().Skip(1).ToList().ForEach(fieldInfo =>
                {
                    //获取枚举值上的特性
                    var groupInfo = fieldInfo.GetCustomAttributes(typeof(ApiGroupInfoAttribute), false)
                        .OfType<ApiGroupInfoAttribute>().FirstOrDefault();

                    var name = groupInfo is not null ? groupInfo.Title : fieldInfo.Name;

                    c.SwaggerEndpoint($"/swagger/{fieldInfo.Name}/swagger.json", name);
                });
            });

            return app;
        }
    }
}