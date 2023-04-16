using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FinancialInstituteAPI.Authorization;

public class CustomSwaggerFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        //remove some parts out of visible api
        swaggerDoc.Paths
            .Where(x => x.Key.ToLowerInvariant().StartsWith("/hook")
            )
            .ToList()
            .ForEach(x => swaggerDoc.Paths.Remove(x.Key));
    }
}
