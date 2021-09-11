using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Server.ClientBuisnessLogicLayer;
using Server.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
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
            services.AddCors();
            services.AddControllers();
         


            services.AddMvc().AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.PropertyNamingPolicy = null;
                o.JsonSerializerOptions.DictionaryKeyPolicy = null;
            });


            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPlaceService, PlaceService>();
            services.AddScoped<IPlaceRepository, PlaceRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddSwaggerGen();
        }
    
    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Software testing");
               // c.RoutePrefix = string.Empty;
            });


            app.UseHttpsRedirection();

             app.UseRouting();

             app.UseCors(x => x
                 .AllowAnyMethod()
                  .AllowAnyHeader()
                       .SetIsOriginAllowed(origin => true)
                         .AllowCredentials());

            app.UseAuthorization();
        
              app.UseEndpoints(endpoints =>
              {
                      endpoints.MapControllers();
                  });
    }


}
}

