using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace FinalCampus.DiController;

public static class SwaggerDependencyInjection
{
    public static IServiceCollection SwaggerService(this IServiceCollection Services)
        {
            Services.AddSwaggerGen(c =>
            {
                // c.SwaggerDoc("v1.0", new OpenApiInfo { Version = "v1.0", Title = "DynamicEtims v1.0" });
                //c.SwaggerDoc("v2.0", new OpenApiInfo { Version = "v2.0", Title = "TenziBiz v2.0" });
                c.AddSecurityDefinition(
                    JwtBearerDefaults.AuthenticationScheme, //Name the security scheme
                    new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme.",
                        Type = SecuritySchemeType.Http, //We set the scheme type to http since we're using bearer authentication
                        Scheme = "bearer" //The name of the HTTP Authorization scheme to be used in the Authorization header. In this case "bearer".
                    }
                );

                c.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Id = JwtBearerDefaults.AuthenticationScheme, //The name of the previously defined security scheme.
                                    Type = ReferenceType.SecurityScheme
                                }
                            },
                            new List<string>()
                        }
                    }
                );
                // c.OperationFilter<AddParamOperationFilter>();
                // c.EnableAnnotations();
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);
            });

            return Services;
        } 
}