using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace MF.Swagger
{
    public class TagOrderByNameDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Tags = new List<OpenApiTag>
            {
            };
            swaggerDoc.Tags = swaggerDoc.Tags.OrderBy(t => t.Name).ToList();
            var updatedPaths = new OpenApiPaths();
            foreach (var entry in swaggerDoc.Paths)
            {
                updatedPaths.Add(
                    entry.Key.Replace("v{version}", swaggerDoc.Info.Version),
                    entry.Value);
            }
            swaggerDoc.Paths = updatedPaths;
        }
    }
}