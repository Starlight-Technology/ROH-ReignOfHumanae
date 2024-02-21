WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Kestrel to listen on a specific port
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(9001);
    options.Limits.MaxRequestBodySize = null;
});

WebApplication app = builder.Build();


_ = app.UseSwagger();
_ = app.UseSwaggerUI();


app.UseAuthorization();

app.MapControllers();

app.Run();