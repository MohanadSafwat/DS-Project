using IdentityModel.Client;
using JWTAuthentication.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using MarketPlace.Models.Repositories;
using MarketPlace.Models;
using MarketPlace.Models.Repository;
using MarketPlace.Dtos;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace JWTAuthentication
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
            services.AddControllers();
            services.AddScoped<IProductRepository<Product>, ProductDbRepository>();
            services.AddTransient<IProductRepository<Product>, ProductDbRepository>();
            services.AddScoped<IAssociatedRepository<AssociatedSell, ProductSellerReadDto>, AssociatedSellRepository>();
            services.AddScoped<IAssociatedRepository<AssociatedBought, ProductBoughtReadDto>, AssociatedBoughtRepository>();
            services.AddScoped<IAssociatedRepository<AssociatedShared, ProductSharedReadDto>, AssociatedSharedRepository>();
            services.AddScoped<IOrderRepository<Order>, OrderDbRepository>();
            services.AddScoped<IOrderRepository<OrderItem>, OrderItemDbRepository>();
            // services.AddDbContext<AppDBContext>(option => option.UseSqlServer(Configuration.GetConnectionString("ConnStr")));
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
    builder =>
    {
                builder.WithOrigins("https://webapp.io/", "https://www.webapp.io/")
    .SetIsOriginAllowed((host) => true)
    .AllowAnyHeader()
    .AllowAnyMethod();
            });
            });
            // For Entity Framework
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ConnStr")));

            // For Identity
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Adding Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            

            // Adding Jwt Bearer
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };
            });

            /* services.AddAuthorization(auth =>
             {
                 auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                     .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                     .RequireAuthenticatedUser().Build());
             });*/

            /* services.AddAuthentication().AddOpenIdConnectServer(options =>
             {
                 options.AllowInsecureHttp = true;
                 options.TokenEndpointPath = new PathString("/token");
                 options.AccessTokenLifetime = TimeSpan.FromDays(1);
                 options.TokenEndpointPath = "/token";
                 options.Provider = new SimpleAuthorizationServerProvider();
             });*/


            /*services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.Events = new CookieAuthenticationEvents
    {
        // After the auth cookie has been validated, this event is called.
        // In it we see if the access token is close to expiring.  If it is
        // then we use the refresh token to get a new access token and save them.
        // If the refresh token does not work for some reason then we redirect to 
        // the login screen.
        OnValidatePrincipal = async cookieCtx =>
        {
            var now = DateTimeOffset.UtcNow;
            var expiresAt = cookieCtx.Properties.GetTokenValue("expires_at");
            var accessTokenExpiration = DateTimeOffset.Parse(expiresAt);
            var timeRemaining = accessTokenExpiration.Subtract(now);
            // TODO: Get this from configuration with a fall back value.
            var refreshThresholdMinutes = 5;
            var refreshThreshold = TimeSpan.FromMinutes(refreshThresholdMinutes);

            if (timeRemaining < refreshThreshold)
            {
                var refreshToken = cookieCtx.Properties.GetTokenValue("refresh_token");
                // TODO: Get this HttpClient from a factory
                var response = await new HttpClient().RequestRefreshTokenAsync(new RefreshTokenRequest
                {
                    Address = tokenUrl,
                    ClientId = clientId,
                    ClientSecret = clientSecret,
                    RefreshToken = refreshToken
                });

                if (!response.IsError)
                {
                    var expiresInSeconds = response.ExpiresIn;
                    var updatedExpiresAt = DateTimeOffset.UtcNow.AddSeconds(expiresInSeconds);
                    cookieCtx.Properties.UpdateTokenValue("expires_at", updatedExpiresAt.ToString());
                    cookieCtx.Properties.UpdateTokenValue("access_token", response.AccessToken);
                    cookieCtx.Properties.UpdateTokenValue("refresh_token", response.RefreshToken);

                    // Indicate to the cookie middleware that the cookie should be remade (since we have updated it)
                    cookieCtx.ShouldRenew = true;
                }
                else
                {
                    cookieCtx.RejectPrincipal();
                    await cookieCtx.HttpContext.SignOutAsync();
                }
            }
        }
    };
})
.AddOpenIdConnect(options =>
{
    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

    options.Authority = oidcDiscoveryUrl;
    options.ClientId = clientId;
    options.ClientSecret = clientSecret;

    options.RequireHttpsMetadata = true;

    options.ResponseType = OidcConstants.ResponseTypes.Code;
    options.UsePkce = true;
    // This scope allows us to get roles in the service.
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("offline_access");

    // This aligns the life of the cookie with the life of the token.
    // Note this is not the actual expiration of the cookie as seen by the browser.
    // It is an internal value stored in "expires_at".
    options.UseTokenLifetime = false;
    options.SaveTokens = true;
});*/
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors("AllowAll");
app.UseMiddleware<CorsMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
