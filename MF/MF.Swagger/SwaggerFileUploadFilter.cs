using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using System;
using System.Collections.Generic;
using System.Reflection;

namespace MF.Swagger
{
    /// <summary>
    /// https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/1479
    /// </summary>
    public class SwaggerFileUploadFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.MethodInfo.GetCustomAttribute(typeof(FileUploadOperationAttribute), true) is FileUploadOperationAttribute fileUploadAttribute)
            {
                var requestBody = operation.RequestBody;
                if (fileUploadAttribute.ClearOtherParameters)
                {
                    operation.Parameters.Clear();
                    requestBody = new OpenApiRequestBody();
                }
                requestBody ??= new OpenApiRequestBody();
                if (!requestBody.Content.TryGetValue("multipart/form-data", out var uploadFileMediaType))
                {
                    requestBody.Content["multipart/form-data"] = uploadFileMediaType = new OpenApiMediaType();
                }
                if (uploadFileMediaType.Schema is null)
                {
                    uploadFileMediaType.Schema = new OpenApiSchema();
                }
                uploadFileMediaType.Schema.Type = "object";
                if (uploadFileMediaType.Schema.Properties is null)
                {
                    uploadFileMediaType.Schema.Properties = new Dictionary<string, OpenApiSchema>(StringComparer.OrdinalIgnoreCase);
                }
                uploadFileMediaType.Schema.Properties[fileUploadAttribute.FieldName] = new OpenApiSchema()
                {
                    Description = fileUploadAttribute.Description,
                    Type = "string",
                    Format = "binary"
                };
                if (uploadFileMediaType.Schema.Required is null)
                {
                    uploadFileMediaType.Schema.Required = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                }
                uploadFileMediaType.Schema.Required.Add(fileUploadAttribute.FieldName);

                operation.RequestBody = requestBody;
            }
            //c.MapType(typeof(IFormFile), () => new OpenApiSchema() { Type = "file", Format = "binary" });
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class FileUploadOperationAttribute : Attribute
    {
        public bool ClearOtherParameters { get; set; }

        public string FieldName { get; set; }

        public string Description { get; set; }

        public FileUploadOperationAttribute()
        {
            this.FieldName = "uploadedFile";
            this.Description = "Upload File";
        }
    }
}