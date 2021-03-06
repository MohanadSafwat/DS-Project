using MarketPlace.Models;
using MarketPlace.Models.Repositories;
using MarketPlace.Models.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace
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
            services.AddDbContext<AppDBContext>(option => option.UseSqlServer(Configuration.GetConnectionString("SqlCon")));
            services.AddScoped<IProductRepository<Product>, ProductDbRepository>();
            services.AddTransient<IProductRepository<Product>, ProductDbRepository>();
            services.AddScoped<IAssociatedRepository<AssociatedSell>, AssociatedSellRepository>();
            services.AddScoped<IAssociatedRepository<AssociatedBought>, AssociatedBoughtRepository>();
            services.AddScoped<IAssociatedRepository<AssociatedShared>, AssociatedSharedRepository>();
            services.AddScoped<IOrderRepository<Order>, OrderDbRepository>();
            services.AddScoped<IOrderRepository<OrderItem>, OrderItemDbRepository>();
            services.Configure<AppDbConnection>(options =>
            {
                options.ConnectionString = Configuration.GetConnectionString("SqlCon");
            });
            services.AddControllersWithViews();
            /*services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ByYM000OLlMQG6VVVp1OH7Xzyr7gHuw1qvUC5dcGt3SNM")),
                    ValidIssuer = "http://localhost:61955",
                    ValidAudience = "http://localhost:4200",
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    ValidateIssuer = true
                };


            });*/
            //services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            /*services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
                        .AddCookie("Cookies")
                        .AddOpenIdConnect("oidc", options =>
                        {
                            options.SignInScheme = "Cookies";

                            options.Authority = "http://localhost:5000";
                            options.RequireHttpsMetadata = false;

                            options.ClientId = "testclient";
                            options.ClientSecret = "secret";
                            options.ResponseType = "code id_token";
                            options.SaveTokens = true;
                            options.GetClaimsFromUserInfoEndpoint = true;

                            options.Scope.Add("testapi");
                            options.Scope.Add("offline_access");
                        });*/
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
