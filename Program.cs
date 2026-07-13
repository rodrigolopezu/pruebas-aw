using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Cortex.Mediator.DependencyInjection;

using eb17953u202421866.Shared.Domain.Repositories;
using eb17953u202421866.Shared.Infrastructure.Interfaces.AspNetCore.Configuration;
using eb17953u202421866.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using eb17953u202421866.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using eb17953u202421866.Shared.Infrastructure.Pipeline.Middleware.Extensions;
using eb17953u202421866.Shared.Interfaces.Rest.ProblemDetails;

var builder = WebApplication.CreateBuilder(args);

// --- Routing & controllers (kebab-case en URLs) -----------------------------
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers(options =>
        options.Conventions.Add(new KebabCaseRouteNamingConvention()))
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()));

// --- Swagger / OpenAPI -------------------------------------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Exam API", Version = "v1" });
    options.EnableAnnotations();
});

// --- Localization (i18n: EN default, ES-PE soportado) ------------------------
builder.Services.AddLocalization();
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "en", "en-US", "es", "es-PE" };
    options.SetDefaultCulture(supportedCultures[0])
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);
});

// --- Database (MySQL) --------------------------------------------------------
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (connectionString != null)
    {
        options.UseMySQL(connectionString);
    }
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ProblemDetailsFactory>();

// ============================================================================
// TODO EXAMEN: acá va UNA línea por cada bounded context que crees, por ejemplo:
// builder.Services.AddScoped<IStartupIncorporationRepository, StartupIncorporationRepository>();
// builder.Services.AddScoped<IStartupIncorporationCommandService, StartupIncorporationCommandService>();
// ============================================================================

builder.Services.AddCortexMediator([typeof(Program)]);

var app = builder.Build();

app.UseGlobalExceptionHandler();
app.UseRequestLocalization();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    if (app.Environment.IsDevelopment())
    {
        context.Database.EnsureDeleted();
    }
    context.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Exam API");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
