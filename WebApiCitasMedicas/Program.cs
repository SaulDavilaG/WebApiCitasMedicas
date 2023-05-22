using WebApiCitasMedicas;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();

var serviceLogger = (ILogger<Startup>)app.Services.GetService(typeof(ILogger<Startup>));

startup.Configure(app, app.Environment, serviceLogger);

app.Run();
