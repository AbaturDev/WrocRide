using WrocRide.Shared.PaginationHelpers;

namespace WrocRide.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IDriverService, DriverService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICarService, CarService>();
            services.AddScoped<IRideService, RideService>();
            services.AddScoped<IUserContextService, UserContextService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRatingService, RatingService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<IReportService, ReportService>();

            services.AddHostedService<ScheduleRideGeneratorService>();

            return services;
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();

            services.AddScoped<IValidator<PageQuery>, PageQueryValidator>();
            services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
            services.AddScoped<IValidator<RegisterDriverDto>, RegisterDriverDtoValidator>();
            services.AddScoped<IValidator<UpdateCarDto>, UpdateCarDtoValidator>();
            services.AddScoped<IValidator<DriverQuery>, DriverQueryValidator>();
            services.AddScoped<IValidator<UpdateDriverPricingDto>, UpdateDriverPricingDtoValidator>();
            services.AddScoped<IValidator<UpdateDriverStatusDto>, UpdateDriverStatusDtoValidator>();
            services.AddScoped<IValidator<UpdateRideStatusDto>, UpdateRideStatusDtoValidator>();
            services.AddScoped<IValidator<RideQuery>, RideQueryValidator>();
            services.AddScoped<IValidator<UpdateUserDto>, UpdateUserDtoValidator>();
            services.AddScoped<IValidator<CreateRatingDto>, CreateRatingDtoValidator>();
            services.AddScoped<IValidator<AddCreditsDto>, AddCreditsDtoValidator>();
            services.AddScoped<IValidator<DocumentQuery>, DocumentQueryValidator>();
            services.AddScoped<IValidator<UserQuery>, UserQueryValidator>();
            services.AddScoped<IValidator<CreateRideReservationDto>, CreateRideReservationDtoValidator>();
            services.AddScoped<IValidator<CreateScheduleDto>, CreateScheduleDtoValidator>();
            services.AddScoped<IValidator<CreateReportDto>, CreateReportDtoValidator>();
            services.AddScoped<IValidator<ReportQuery>, ReportQueryValidator>();
            services.AddScoped<IValidator<UpdateReportDto>, UpdateReportDtoValidator>();
            services.AddScoped<IValidator<UpdateDocumentDto>, UpdateDocumentDtoValidator>();
            services.AddScoped<IValidator<CreateRideDto>, CreateRideDtoValidator>();

            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            return services;
        }

        public static IServiceCollection AddAuthorizationHandlers(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationHandler, ActiveUserRequirementHandler>();

            return services;
        }

        public static IServiceCollection AddMiddlewares(this IServiceCollection services)
        {
            services.AddScoped<ErrorHandlingMiddleware>();
            services.AddScoped<RequestLoggingMiddleware>();

            return services;
        }

        public static IServiceCollection AddCorsPolicies(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("FrontendClient", policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            return services;
        }

        public static IServiceCollection AddCustomAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("IsActivePolicy", policy =>
                {
                    policy.Requirements.Add(new ActiveUserRequirement());
                });
            });

            return services;
        }
    }
}
