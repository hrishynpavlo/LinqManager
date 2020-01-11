using AutoMapper;
using BookStore.Db.Entities;
using BookStore.Utils.Implementations;
using BookStore.Utils.Interfaces;
using BookStore.Utils.Services;
using LinqManager.Client.Db;
using LinqManager.Client.Db.Entities;
using LinqManager.WebExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace BookStore
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
            services.AddDbContext<ApplicationDbContext>(options => 
                options.UseSqlServer("Data Source=localhost\\SQLEXPRESS;Initial Catalog=BookStore;Integrated Security=True"));
            services.AddSwaggerGen(s => {
                s.SwaggerDoc("v1", new OpenApiInfo { Title = "BookStore API", Version = "v1" });
            });
            services.AddTransient<IRepository<Country>, CountryRepository>();
            services.AddTransient<IRepository<Author>, AuthorRepository>();
            services.AddTransient<IRepository<Book>, BookRepository>();
            services.AddTransient<IRepositoryAsync<PublishingHouse>, BaseRepository<PublishingHouse>>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ICountryService, CountryService>();
            services.AddTransient<IAuthorService, AuthorService>();
            services.AddTransient<IBookService, BookService>();
            services.AddTransient<IPublishingHouseService, PublishingHouseService>();
            services.AddAutoMapper(typeof(Startup));
            services.AddLinqManager();
            services.AddSingleton<IEmailService, EmailService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(s => {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "BookStore API");
            });

            app.UseExceptionHandler(app => app.Run(async context => {
                var exceptionContext = context.Features.Get<IExceptionHandlerFeature>();
                if (exceptionContext?.Error is LinqManager.Exceptions.LinqManagerException linqExcep) 
                {
                    switch (linqExcep.Type)
                    {
                        case LinqManager.Enums.ExceptionTypes.MismatchProperty:
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;
                            var json = new JObject();

                            if (linqExcep.ValidationErrors.TryGetValue("FilterBy", out var filter) && filter.Any())
                                json.Add("filterBy", JToken.FromObject(string.Join(",", filter)));

                            if (linqExcep.ValidationErrors.TryGetValue("SortBy", out var sort) && sort.Any())
                                json.Add("sortBy", JToken.FromObject(string.Join(",", sort)));

                            await context.Response.WriteAsync(json.ToString());
                            break;
                        case LinqManager.Enums.ExceptionTypes.RequestFactoryError:
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;
                            await context.Response.WriteAsync(JsonConvert.SerializeObject(new { message = linqExcep.InnerException?.Message }));
                            break;
                    }
                }

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new { message = "Something went wrong. Please contact us" }));
            }));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
