using System.Text;
using InmobiliariaMrAPI.Data;
using InmobiliariaMrAPI.Repositories;
using InmobiliariaMrAPI.Repositories.User;
using InmobiliariaMrAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

//? Add services to the container.
builder.Services.AddControllers();

//? Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
//? Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Inmobiliaria MR API",
        Version = "v1",
        Description = "API para gestión de inmobiliaria"
    });

    // Agregar soporte para archivos
    options.MapType<IFormFile>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "binary"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresa tu token JWT en el formato: Bearer {tu token aquí}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

//? JWT Configuration
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
            };
        }
    );

//? Add Authorization based on roles
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("Propietario", policy => policy.RequireRole("Propietario"));
    options.AddPolicy("Operador", policy => policy.RequireRole("Operador"));
});

//? Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IInmuebleService, InmuebleService>();
builder.Services.AddScoped<IContratoService, ContratoService>();
builder.Services.AddScoped<IInquilinoService, InquilinoService>();
builder.Services.AddScoped<IPagoService, PagoService>();
builder.Services.AddScoped<IFileService, FileService>();
//? Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IPropietarioRepository, PropietarioRepository>();
builder.Services.AddScoped<IInmuebleRepository, InmuebleRepository>();
builder.Services.AddScoped<IArchivoRepository, ArchivoRepository>();
builder.Services.AddScoped<IContratoRepository, ContratoRepository>();
builder.Services.AddScoped<IInquilinoRepository, InquilinoRepository>();
builder.Services.AddScoped<IPagosRepository, PagosRepository>();

//? DB - MySQL
var conn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(conn, ServerVersion.AutoDetect(conn)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Inmobiliaria MR API v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

//? Controllers
app.MapControllers();

app.Run();