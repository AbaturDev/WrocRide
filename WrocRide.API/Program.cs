var builder = WebApplication.CreateBuilder(args);

builder.Host.UseConfiguredSerilog();

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<WrocRideDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddValidators();

builder.Services.AddAuthorizationHandlers();

builder.Services.AddServices();

builder.Services.AddSignalR();

builder.Services.AddMiddlewares();

builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwagger();

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddCustomAuthorizationPolicies();

builder.Services.AddCorsPolicies();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors("FrontendClient");

app.UseSerilogRequestLogging();

app.UseCustomMiddlewares();

app.UseAuthentication();

app.UseHttpsRedirection();

app.MapHubs();

app.UseConfiguredSwagger();

app.UseAuthorization();

app.MapControllers();

app.Seed();

app.Run();