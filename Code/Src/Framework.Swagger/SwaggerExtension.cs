using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Framework.Swagger
{
    public static class SwaggerExtension
    {
        public static void AddSwagger(this IServiceCollection services, string assemblyName, string projectName, string directoryPath)
        {
            services.AddSwaggerGen(c =>
            {
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", 
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = "bearer", 
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                  {
                      {
                          securityScheme, new string[] { }
                      }
                  });
            });
            services.AddSwaggerGenNewtonsoftSupport();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Description = $"{projectName} management api.",
                    Title = "API",
                    Version = "v1",
                });
                options.EnableAnnotations();
                var xmlFile = $"{assemblyName}.xml";
                var xmlPath = Path.Combine(directoryPath, xmlFile);
                options.IncludeXmlComments(xmlPath);
                options.UseInlineDefinitionsForEnums();
                options.ResolveConflictingActions(descriptors => descriptors.First());
            });
        }
    }
}
