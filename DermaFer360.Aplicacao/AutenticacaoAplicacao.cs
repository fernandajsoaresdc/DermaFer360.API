using DataAccess.Repositorio;
using DermaFer360.Aplicacao.DTO;
using DermaFer360.Aplicacao.Interfaces;
using DermaFer360.Dominio.Entidades;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;



namespace Projeto360.Aplicacao;

public class AutenticacaoAplicacao : IAutenticacaoAplicacao
{
    readonly IUsuarioRepositorio _usuarioRepositorio;
    readonly IConfiguration _configuration;

    public AutenticacaoAplicacao(IUsuarioRepositorio usuarioRepositorio, IConfiguration configuration)
    {
        _usuarioRepositorio = usuarioRepositorio;
        _configuration = configuration;
    }

    public async Task<Login> LoginAsync(LoginAutenticacao login)
    {
        var usuario = await _usuarioRepositorio.ObterPorEmailAsync(login.Email);

        if (usuario == null || usuario.Senha != login.Senha)
            throw new Exception("E-mail ou senha inválidos");

        return new Login
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
            Cargo = usuario.Cargo.ToString(),
            Token = GerarToken(usuario)
        };
    }

    private string GerarToken(Usuario usuario)
    {
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);

        var claims = new[]
        {
        new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
        new Claim("Nome", usuario.Nome),
        new Claim("Cargo", usuario.Cargo.ToString())
    };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(
                Convert.ToDouble(_configuration["Jwt:ExpirationHours"])),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}