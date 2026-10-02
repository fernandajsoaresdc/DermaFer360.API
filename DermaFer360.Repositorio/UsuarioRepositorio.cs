using Microsoft.EntityFrameworkCore;
using DermaFer360.Dominio.Entidades;


namespace DataAccess.Repositorio;

public class UsuarioRepositorio : BaseRepositorio, IUsuarioRepositorio
{
    public UsuarioRepositorio(DermaFer360Context context) : base(context)
    { }

    public async Task<int> CriarAsync(Usuario usuario)
    {
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();

        return usuario.Id;
    }

    public async Task AtualizarAsync(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync();
    }

    public async Task<Usuario> ObterAsync(int usuarioId)
    {
        return await _context.Usuarios
        .Where(u => u.Id == usuarioId)
        .Where(u => u.Ativo)
        .FirstOrDefaultAsync();
    }

    public async Task<Usuario> ObterPorEmailAsync(string email)
    {
        return await _context.Usuarios
        .Where(u => u.Email == email)
        .Where(u => u.Ativo)
        .FirstOrDefaultAsync();
    }

    public async Task DeleteAsync(Usuario usuario)
    {
        usuario.Ativo = false;
        await _context.SaveChangesAsync();
    }

    public async Task<Usuario> ObterIncluindoInativosAsync(int usuarioId)
    {
        return await _context.Usuarios
            .Where(u => u.Id == usuarioId)
            .FirstOrDefaultAsync();
    }

    public async Task RestaurarAsync(Usuario usuario)
    {
        usuario.Ativo = true;
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Usuario>> ListarAsync()
    {
        return await _context.Usuarios
        .AsNoTracking()
        .OrderBy(u => u.Nome)
        .ToListAsync();
    }
}