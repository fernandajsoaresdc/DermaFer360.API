
using DermaFer360.Aplicacao;
using DermaFer360.Aplicacao.Interfaces;
using DataAccess.Repositorio;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Banco de dados SQL Server
builder.Services.AddDbContext<DermaFer360Context>(options => options.UseSqlServer( builder.Configuration.GetConnectionString("DefaultConnection")));

//Repositórios
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<IProdutoRepositorio, ProdutoRepositorio>();
builder.Services.AddScoped<IReceitaRepositorio, ReceitaRepositorio>();
builder.Services.AddScoped<IDespesaRepositorio, DespesaRepositorio>();

// Injeção das aplicações
builder.Services.AddScoped<IUsuarioAplicacao, UsuarioAplicacao>();
builder.Services.AddScoped<IProdutoAplicacao, ProdutoAplicacao>();
builder.Services.AddScoped<IReceitaAplicacao, ReceitaAplicacao>();
builder.Services.AddScoped<IDespesaAplicacao, DespesaAplicacao>();
builder.Services.AddScoped<IRelatorioAplicacao, RelatorioAplicacao>();

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTPS
app.UseHttpsRedirection();

// Controllers
app.MapControllers();

app.Run();

