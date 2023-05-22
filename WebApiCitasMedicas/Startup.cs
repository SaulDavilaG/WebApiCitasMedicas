using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using WebApiCitasMedicas.Filtros;
using Microsoft.IdentityModel.JsonWebTokens;
using WebApiCitasMedicas.Middleware;
using System.Text.Json.Serialization;

namespace WebApiCitasMedicas
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(opciones =>
            {
                opciones.Filters.Add(typeof(ExcepcionFiltro));
            }).AddJsonOptions(x =>
            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));

            services.AddTransient<AccionFiltro>();

            services.AddResponseCaching();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen( c=> {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "ApiCitasMedicas", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {

            app.UseResponeHttp();

            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseResponseCaching();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
