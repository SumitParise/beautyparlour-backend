
using BeautyParlourAPIs.DAL;
using BeautyParlourAPIs.DAL.Repositories;
using BeautyParlourAPIs.DAL.Repositories.Interfaces;
using BeautyParlourAPIs.Models.DTOs;
using BeautyParlourAPIs.Services;
using BeautyParlourAPIs.Services.Interface;
using BeautyParlourAPIs.Services.JwtSecurity;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BeautyParlourAPIs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			// Define allowed origins for both Dev and Prod
			string[] allowedOrigins = new string[]
			{
	"http://localhost:5173", // Local Vite dev server
    "https://green-sky-01d955200.3.azurestaticapps.net",
	"https://www.shrutikamakeover.co.in"// Your Azure Static Web App URL
			};

			// ✅ Add CORS
			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowSpecificOrigins", policy =>
				{
					policy.WithOrigins(allowedOrigins)
						  .AllowAnyHeader()
						  .AllowAnyMethod();
				});
			});

			builder.Services.AddControllers();

			// Entity Framework DbContext configuration
			//builder.Services.AddDbContext<AppDbContext>(options =>
			//  options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


			//for azure sql with retry logic
			builder.Services.AddDbContext<AppDbContext>(options =>
			options.UseSqlServer(
				builder.Configuration.GetConnectionString("DefaultConnection"),
				sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(
					maxRetryCount: 5,                     // number of retries
					maxRetryDelay: TimeSpan.FromSeconds(10), // wait between retries
					errorNumbersToAdd: null)              // optional
				));



			// Hangfire configuration
			builder.Services.AddHangfire(config =>
			{
				config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"));
			});
			builder.Services.AddHangfireServer();


			builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            builder.Services.AddScoped<IFeedback, FeedbackService>();
            builder.Services.AddScoped<IAdmission, AdmissionRepository>();
            builder.Services.AddScoped<IAdmissionService, AdmissionService>();
			
			builder.Services.AddScoped<IEmailTemplateService, EmailTemplateService>();
			builder.Services.AddScoped<IOtpService, OTPService>();
		    builder.Services.AddScoped<IMobileOtpService, MobileOtpService>();

			builder.Services.AddLogging();
			builder.Services.Configure<EmailSetting>(builder.Configuration.GetSection("EmailSettings"));
			builder.Services.AddScoped<IEmailService, EmailService>();
			builder.Services.AddScoped<ICustomerAccountRepository,CustomerAccountRepository>();
            builder.Services.AddScoped<IAccountService,CustomerAccountService>();

			builder.Services.AddScoped<IAuthRepository, AuthRepository>();
			builder.Services.AddScoped<IAuthService, AuthService>();
			builder.Services.AddSingleton<Jwt>();



			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


			//jwt setup configuration
			var jwtSettings = builder.Configuration.GetSection("JwtSettings");
			var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidIssuer = jwtSettings["Issuer"],
					ValidateAudience = true,
					ValidAudience = jwtSettings["Audience"],
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key)
				};
			});

			builder.Services.AddAuthorization();

			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

			app.Use(async (context, next) =>
			{
				try
				{
					await next.Invoke();
				}
				catch (Exception ex)
				{
					var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
					logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
					context.Response.StatusCode = 500;
					await context.Response.WriteAsync("Internal Server Error: " + ex.Message);
				}
			});



			app.UseStaticFiles(); // allows serving files from wwwroot


			app.UseHttpsRedirection();

			// ? IMPORTANT: Use CORS BEFORE authentication/authorization
			app.UseCors("AllowSpecificOrigins");

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseHangfireDashboard("/hangfire"); // dashboard UI ? /hangfire


			app.MapControllers();

            app.Run();
        }
    }
}
