using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;

namespace Bookify.API.Hypermedia;

public class HateoasSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(EntityResponseWrapper<>) ||
            context.Type.IsGenericType && context.Type.GetGenericTypeDefinition() == typeof(EntityResponseWrapper<>))
        {
            schema.Example = new OpenApiObject
            {
                ["value"] = new OpenApiObject
                {
                    ["id"] = new OpenApiString(Guid.NewGuid().ToString()),
                    ["name"] = new OpenApiString("Example Name"),
                    ["description"] = new OpenApiString("Example Description")
                },
                ["_links"] = new OpenApiArray
                {
                    new OpenApiObject
                    {
                        ["href"] = new OpenApiString("https://api.example.com/resource/123"),
                        ["rel"] = new OpenApiString("self"),
                        ["method"] = new OpenApiString("GET")
                    },
                    new OpenApiObject
                    {
                        ["href"] = new OpenApiString("https://api.example.com/resource/123"),
                        ["rel"] = new OpenApiString("update"),
                        ["method"] = new OpenApiString("PATCH")
                    }
                }
            };
        }
    }
}