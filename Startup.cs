using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyDeckAPI.Interfaces;
using MyDeckAPI.Models;
using MyDeckAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MyDeckAPI.Security;
using Microsoft.AspNetCore.Authorization;
using MyDeckAPI.Data.MediaContent;
using Microsoft.AspNetCore.HttpOverrides;
using MyDeckApi_Experimental.Services;
using MyDeckApi_Experimental.Services.Usecases;
using MyDeckApi_Experimental.Interfaces;
using MyDeckAPI.Services.Usecases;
using MultipartDataMediaFormatter;
using MultipartDataMediaFormatter.Infrastructure;
using System.Web.Http.SelfHost;

namespace MyDeckAPI
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _env = env;
            Configuration = configuration;


            var contentRoot = configuration.GetValue<string>(WebHostDefaults.ContentRootKey);
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthUtils.ISSUER,
                        ValidateAudience = true,
                        ValidAudience = AuthUtils.AUDIENCE,
                        ValidateLifetime = true,
                        IssuerSigningKey = AuthUtils.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.FromMinutes(5)
                    };
                }).AddJwtBearer("EmailConfirmation", options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthUtils.ISSUER,
                        ValidateAudience = true,
                        ValidAudience = AuthUtils.AUDIENCE,
                        ValidateLifetime = true,
                        IssuerSigningKey = AuthUtils.GetEmailConfirmationSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.FromMinutes(5)
                    };
                });

            if (_env.IsDevelopment())
            {
                services.AddSingleton<IAuthorizationHandler, AllowAnonymous>();
            }

            services.AddDbContext<MDContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddSingleton<SnakeCaseConverter>();
            services.AddTransient<ICardRepository, CardRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ContentSaver>();
            services.AddTransient<IUserDeckRepository, UserDeckRepository>();
            services.AddTransient<IDeckRepository, DeckRepository>();
            services.AddTransient<ISessionRepository, SessionRepository>();
            services.AddTransient<ITokenRepository, TokenRepository>();
            services.AddTransient<ISubscribeRepository, SubscribeRepository>();
            services.AddTransient<IFileRepository, FileRepository>();
            services.AddTransient<IAuthFacade, AuthFacade>();
            services.AddTransient<MailService>();
            services.AddTransient<ContentSaver>();
            services.AddTransient<AuthUtils>();
            services.AddTransient<GetDecksForTrainUseCase>();
            services.AddTransient<GetNewTokensUseCase>();
            services.AddTransient<RefreshTokensUseCase>();
            services.AddTransient<UpdateDeckUseCase>();
            services.AddTransient<SignUpWithGoogleUseCase>();
            services.AddTransient<SignUpWithEmailUseCase>();
            services.AddTransient<SignInWithEmailUseCase>();
            services.AddScoped<DeleteDeckUseCase>();
            services.AddTransient<SignInWithUsernameUseCase>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "MyDeck API", Version = "v1"});
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyDeck API V1"); });

            app.UseHttpsRedirection();

            app.UseRouting();


            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}