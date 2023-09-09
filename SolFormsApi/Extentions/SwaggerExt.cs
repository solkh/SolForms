using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SolFormsApi.Extentions
{
    public static class SwaggerExt
    {
        public static void AddMyApiSwaggerEndpoints(this SwaggerGenOptions options)
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "SolForm API", Version = "v1" });            
        }
    }
}
