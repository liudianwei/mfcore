using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MF.Swagger
{
    public class SwaggerDefaultValues : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            ApiVersion apiVersion = context.ApiDescription.GetApiVersion();
            if (apiVersion is null)
            {
                return;
            }
            IList<OpenApiParameter> parameters = operation.Parameters;
            if (parameters is null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }
            OpenApiParameter parameter = parameters.FirstOrDefault(p => p.Name == "version");
            if (parameter is null)
            {
                parameter = new OpenApiParameter()
                {
                    Name = "version",
                    Required = true,
                    In = ParameterLocation.Query,
                    Schema = new OpenApiSchema
                    {
                        Default = new OpenApiString(apiVersion.ToString()),
                        Type = "string"
                    },
                };
                parameters.Add(parameter);
            }
            else if (parameter is OpenApiParameter pathParameter)
            {
                if (pathParameter.Name == "version")
                {
                    pathParameter.Schema = new OpenApiSchema
                    {
                        Default = new OpenApiString(apiVersion.ToString()),
                        Type = "string"
                    };
                    if (parameter.Description is null)
                    {
                        parameter.Description = "The requested API version";
                    }
                }
            }
        }
    }
}