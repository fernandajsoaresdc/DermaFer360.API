public abstract class BaseRepositorio
{
    protected readonly DermaFer360Context _context;

    protected BaseRepositorio(DermaFer360Context context)
    {
        _context = context;
    }
}