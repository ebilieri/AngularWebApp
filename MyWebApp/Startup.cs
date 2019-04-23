using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyWebApp.Context;
using MyWebApp.Repositories;
using Newtonsoft.Json;

namespace MyWebApp
{
  public class Startup
  {
    private IConfiguration _configuration;

    public Startup(IHostingEnvironment environment)
    {
      var builder = new ConfigurationBuilder()
          .SetBasePath(environment.ContentRootPath)
          .AddJsonFile("appstting.json", optional: true, reloadOnChange: true)
          .AddJsonFile("config.json", optional: true, reloadOnChange: true);

      _configuration = builder.Build();

    }
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      var sqlConnection = _configuration.GetConnectionString("MyWebAppDB");

      services.AddDbContext<MyWebAppContext>(options =>
          options.UseMySQL(sqlConnection, s => s.MigrationsAssembly("MyWebApp")));

      services.AddMvc();

      // Mapeamentos Interfaces
      services.AddScoped<IProdutoRepository, ProdutoRepository>();
      services.AddScoped<IPedidoRepository, PedidoRepository>();

      services.AddMvc().AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseStaticFiles();
      app.UseMvcWithDefaultRoute();

      //app.Run(async (context) =>
      //{
      //    await context.Response.WriteAsync("Hello World!");
      //});
    }
  }
}
