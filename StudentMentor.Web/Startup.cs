using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using StudentMentor.Data.Entities;
using StudentMentor.Data.Enums;
using StudentMentor.Domain.Models.Configurations;
using StudentMentor.Domain.Repositories.Implementations;
using StudentMentor.Domain.Repositories.Interfaces;
using StudentMentor.Domain.Services.Implementations;
using StudentMentor.Domain.Services.Interfaces;
using StudentMentor.Web.Infrastructure;
using StudentMentor.Web.Infrastructure.AuthorizationRequirements;

namespace StudentMentor.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddHttpContextAccessor();


            services.AddDbContext<StudentMentorDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("StudentMentor")));

            var jwtConfiguration = new JwtConfiguration();
            Configuration.GetSection(nameof(JwtConfiguration)).Bind(jwtConfiguration);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtConfiguration.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtConfiguration.AudienceId,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(jwtConfiguration.GetAudienceSecretBytes())
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.Admin, policy => policy.Requirements.Add(new RoleRequirement(UserRole.Admin)));
                options.AddPolicy(Policies.Mentor, policy => policy.Requirements.Add(new RoleRequirement(UserRole.Mentor)));
                options.AddPolicy(Policies.Student, policy => policy.Requirements.Add(new RoleRequirement(UserRole.Student)));
            });

            services.AddSingleton<IAuthorizationHandler, RoleRequirementHandler>();

            services.Configure<JwtConfiguration>(Configuration.GetSection(nameof(JwtConfiguration)));

            services.AddTransient<IClaimProvider, ClaimProvider>();
            services.AddTransient<IJwtService, JwtService>();

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IStudentRepository, StudentRepository>();

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
