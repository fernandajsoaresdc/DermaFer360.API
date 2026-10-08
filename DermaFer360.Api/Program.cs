using DermaFer360.Aplicacao;
using DermaFer360.Aplicacao.Interfaces;
using DataAccess.Repositorio;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();


// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.ParameterLocation.Header,
        Description = "Digite o token JWT. Exemplo: Bearer {seu_token}"
    });

    options.AddSecurityRequirement(document =>
        new Microsoft.OpenApi.OpenApiSecurityRequirement
        {
            [new Microsoft.OpenApi.OpenApiSecuritySchemeReference("Bearer", document)] = []
        });
});

// Banco de dados SQL Server
builder.Services.AddDbContext<DermaFer360Context>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Repositórios
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<IProdutoRepositorio, ProdutoRepositorio>();
builder.Services.AddScoped<IReceitaRepositorio, ReceitaRepositorio>();
builder.Services.AddScoped<IRelatorioRepositorio, RelatorioRepositorio>();
builder.Services.AddScoped<IDespesaRepositorio, DespesaRepositorio>();

// Injeção das aplicações
builder.Services.AddScoped<IUsuarioAplicacao, UsuarioAplicacao>();
builder.Services.AddScoped<IProdutoAplicacao, ProdutoAplicacao>();
builder.Services.AddScoped<IReceitaAplicacao, ReceitaAplicacao>();
builder.Services.AddScoped<IDespesaAplicacao, DespesaAplicacao>();
builder.Services.AddScoped<IRelatorioAplicacao, RelatorioAplicacao>();

builder.Services.AddScoped<IAutenticacaoAplicacao, AutenticacaoAplicacao>();

builder.Services.AddHttpClient<IIAAplicacao, IAAplicacao>(client =>
{
    client.BaseAddress = new Uri("http://localhost:11434/");
    client.Timeout = TimeSpan.FromMinutes(5);
});

//JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactApp", policy =>
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTPS
app.UseHttpsRedirection();

// CORS (antes de Authentication/Authorization)
app.UseCors("ReactApp");

app.UseAuthentication();
app.UseAuthorization();

// Controllers
app.MapControllers();

app.Run();