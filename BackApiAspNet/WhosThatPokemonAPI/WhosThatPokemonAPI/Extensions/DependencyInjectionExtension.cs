using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using WhosThatPokemonAPI.Data;
using WhosThatPokemonAPI.Helpers;
using WhosThatPokemonAPI.Models;
using WhosThatPokemonAPI.Repositories;
using WhosThatUserAPI.Repositories;
using Type = WhosThatPokemonAPI.Models.Type;

namespace WhosThatPokemonAPI.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static void InjectDependencies(this WebApplicationBuilder builder)
        {
            builder.AddDbContext();
            builder.ConfigureSwagger();
            builder.FixObjectCycleError();
            builder.AddCorsPolicy();
            builder.AddRepositories();
            builder.AddAuthentication();
            builder.AddAuthorization();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
        }

        private static void AddDbContext(this WebApplicationBuilder builder)
        {
            // Récupération connectionString
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            // DataDbContext
            builder.Services.AddDbContext<DataDbContext>(options => options.UseSqlServer(connectionString));
        }

        private static void ConfigureSwagger(this WebApplicationBuilder builder)
        {
            // Pour configurer Swagger et pouvoir l'utiliser avec des JWT
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Who's That Pokemon API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Type = SecuritySchemeType.Http
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                   {
                     new OpenApiSecurityScheme
                     {
                       Reference = new OpenApiReference
                       {
                         Type = ReferenceType.SecurityScheme,
                         Id = "Bearer"
                       }
                      },
                      new string[] { }
                    }
                });
            });
        }

        private static void FixObjectCycleError(this WebApplicationBuilder builder)
        {
            // le .AddJsonOptions(...) fix l'erreur "A possible object cycle was detected"
            builder.Services.AddControllers().AddJsonOptions(x =>
                            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            builder.Services.AddEndpointsApiExplorer();
        }

        private static void AddCorsPolicy(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(policy =>
            {
                policy.AddPolicy("MyPolicy", builder =>
                  builder.WithOrigins("*")
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
             );
            });
        }

        private static void AddRepositories(this WebApplicationBuilder builder)
        {
            //builder.Services.AddScoped<IRepository<Pokemon>, PokemonRepository>()
            builder.Services.AddScoped<PokemonRepository, PokemonRepository>();
            builder.Services.AddScoped<IRepository<User>, UserRepository>();
            builder.Services.AddScoped<IRepository<Type>, TypeRepository>();
            builder.Services.AddScoped<IRepository<UserPokemon>, UserPokemonRepository>();
        }


        private static void AddAuthentication(this WebApplicationBuilder builder)
        {
            // Récupérer la section AppSetings du fichier appsettings.json
            IConfigurationSection appSettingsSection = builder.Configuration.GetSection(nameof(AppSettings)); // La classe AppSettings vient des Helpers
            builder.Services.Configure<AppSettings>(appSettingsSection);
            AppSettings appSettings = appSettingsSection.Get<AppSettings>();

            // Authentication
            byte[] key = Encoding.ASCII.GetBytes(appSettings.SecretKey);
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true,
                    ValidateAudience = true,
                    ValidAudience = appSettings.ValidAudience,
                    ValidateIssuer = true,
                    ValidIssuer = appSettings.ValidIssuer,
                    ClockSkew = TimeSpan.Zero
                };
            });
        }

        // Différents rôles pour les différents droits d'accès
        private static void AddAuthorization(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthorization(options =>
            {
                // Pour les Admins
                options.AddPolicy(Constants.PolicyAdmin, police =>
                {
                    police.RequireClaim(ClaimTypes.Role, Constants.RoleAdmin);
                });

                // Pour les users non admins
                options.AddPolicy(Constants.PolicyUser, police =>
                {
                    police.RequireClaim(ClaimTypes.Role, Constants.RoleUser);
                });
            });
        }
    }
}
