var builder = WebApplication.CreateBuilder(args);

var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtAuthentication>();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddDbContext<WrocRideDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
builder.Services.AddScoped<IValidator<RegisterDriverDto>, RegisterDriverDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateCarDto>, UpdateCarDtoValidator>();
builder.Services.AddScoped<IValidator<DriverQuery>, DriverQueryValidator>();
builder.Services.AddScoped<IValidator<UpdateDriverPricingDto>, UpdateDriverPricingDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateDriverStatusDto>, UpdateDriverStatusDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateRideStatusDto>, UpdateRideStatusDtoValidator>();
builder.Services.AddScoped<IValidator<RideQuery>, RideQueryValidator>();
builder.Services.AddScoped<IValidator<UpdateUserDto>, UpdateUserDtoValidator>();
builder.Services.AddScoped<IValidator<CreateRatingDto>, CreateRatingDtoValidator>();
builder.Services.AddScoped<IValidator<DriverRatingsQuery>, DriverRatingsQueryValidator>();
builder.Services.AddScoped<IValidator<AddCreditsDto>, AddCreditsDtoValidator>();
builder.Services.AddScoped<IValidator<DocumentQuery>, DocumentQueryValidator>();
builder.Services.AddScoped<IValidator<UserQuery>, UserQueryValidator>();
builder.Services.AddScoped<IValidator<CreateRideReservationDto>, CreateRideReservationDtoValidator>();
builder.Services.AddScoped<IValidator<CreateScheduleDto>, CreateScheduleDtoValidator>();
builder.Services.AddScoped<IValidator<CreateReportDto>, CreateReportDtoValidator>();
builder.Services.AddScoped<IValidator<ReportQuery>, ReportQueryValidator>();
builder.Services.AddScoped<IValidator<UpdateReportDto>, UpdateReportDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateDocumentDto>, UpdateDocumentDtoValidator>();
builder.Services.AddScoped<IValidator<CreateRideDto>, CreateRideDtoValidator>();
builder.Services.AddScoped<IValidator<ScheduleQuery>, ScheduleQueryValidator>();

builder.Services.AddScoped<IAuthorizationHandler, ActiveUserRequirementHandler>();

builder.Services.AddScoped<IDriverService, DriverService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IRideService, RideService>();
builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRatingService, RatingService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IScheduleService, ScheduleService>();
builder.Services.AddScoped<IReportService, ReportService>();

builder.Services.AddHostedService<ScheduleRideGeneratorService>();
builder.Services.AddSignalR();

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<RequestLoggingMiddleware>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(jwtOptions);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidAudience = jwtOptions.Issuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key))
    };

});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IsActivePolicy", policy =>
    {
        policy.Requirements.Add(new ActiveUserRequirement());
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendClient", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors("FrontendClient");

app.UseSerilogRequestLogging();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseAuthentication();

app.UseHttpsRedirection();

app.MapHub<NotificationHub>("notification-hub");

app.UseSwagger();
app.UseSwaggerUI(options =>
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "WrocRide API")
);

app.UseAuthorization();

app.MapControllers();

app.Seed();

app.Run();
