using CentimeterX.API.Data;
//using CentimeterX.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Oracle + EF Core
var connectionString = builder.Configuration.GetConnectionString("OracleConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(connectionString));

// Services
//builder.Services.AddScoped<RoverService>();
//builder.Services.AddScoped<SessaoCorrecaoService>();
//builder.Services.AddScoped<EstacaoBaseService>();
//builder.Services.AddScoped<OcorrenciaService>();
//builder.Services.AddScoped<UsuarioService>();
//builder.Services.AddScoped<ICorrecaoService, SessaoCorrecaoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
