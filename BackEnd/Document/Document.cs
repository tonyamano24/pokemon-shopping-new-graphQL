//i want to create a document class as swagger document that has basic auth and jwt authentication headers
using System.Collections.Generic;
using itsc_dotnet_practice.Document.Interface;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace itsc_dotnet_practice.Document
{
    public class Document : IDocument
    {
        public OpenApiDocument GetOpenApiDocument()
        {
            return new OpenApiDocument
            {
                Info = new OpenApiInfo
                {
                    Title = "ITSC .NET Practice API",
                    Version = "v1",
                    Description = "API documentation for ITSC .NET Practice project"
                },
                Components = new OpenApiComponents
                {
                    SecuritySchemes = new Dictionary<string, OpenApiSecurityScheme>
                    {
                        ["BasicAuth"] = new OpenApiSecurityScheme
                        {
                            Type = SecuritySchemeType.Http,
                            Scheme = "basic",
                            Description = "Basic Authentication"
                        },
                        ["BearerAuth"] = new OpenApiSecurityScheme
                        {
                            Type = SecuritySchemeType.Http,
                            Scheme = "bearer",
                            BearerFormat = "JWT",
                            Description = "JWT Bearer Authentication"
                        }
                    }
                },
                SecurityRequirements = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "BasicAuth" } }, new List<string>() },
                        { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "BearerAuth" } }, new List<string>() }
                    }
                }
            };
        }
    }
}