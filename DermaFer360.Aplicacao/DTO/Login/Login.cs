namespace DermaFer360.Aplicacao.DTO;

public class Login
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Cargo { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}