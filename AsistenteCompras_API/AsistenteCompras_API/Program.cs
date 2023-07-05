using AsistenteCompras_API.Domain.Services;
using AsistenteCompras_API.Infraestructure.Contexts;
using AsistenteCompras_API.Infraestructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IComidaRepository, ComidaRepository>();
builder.Services.AddScoped<IEventoRepository, EventoRepository>();
builder.Services.AddScoped<IUbicacionRepository,UbicacionRepository>();
builder.Services.AddScoped<IOfertaRepository, OfertaRepository>();
builder.Services.AddScoped<ITipoProductoRepository, TipoProductoRepository>();
builder.Services.AddScoped<IComercioRepository, ComercioRepository>();
builder.Services.AddScoped<IEventoService,EventoService>();
builder.Services.AddScoped<IUbicacionService, UbicacionService>();
builder.Services.AddScoped<IOfertaService, OfertaService>();
builder.Services.AddScoped<IComercioService, ComercioService>();
builder.Services.AddScoped<ITipoProductoService, TipoProductoService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IListadoOfertasRepository, ListadoOfertasRepository>();
builder.Services.AddScoped<IListadoOfertasService, ListadoOfertasService>();
builder.Services.AddScoped<IRolService, RolService>();
builder.Services.AddScoped<IRolRepository, RolRepository>();
builder.Services.AddScoped<IVerificadorComercioService, VerificadorComercioService>();
builder.Services.AddDbContext<AsistenteComprasContext>(options => options.UseSqlServer(
    builder.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection")));

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = false,
//            ValidateAudience = false,
//            ValidateIssuerSigningKey = true,
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
//        };
//    });

var app = builder.Build();

app.UseCors(builder =>
{
    builder.AllowAnyHeader()
           .AllowAnyMethod()
           .AllowAnyOrigin();
});

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

//app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
