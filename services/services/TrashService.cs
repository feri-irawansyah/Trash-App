using Services.Models;
using Services.Repositories;

public class TrashService
{
    private readonly ITrashRepository _repo;
    public TrashService(ITrashRepository repo) => _repo = repo;

    public Task<IEnumerable<Trash>> GetAll() => _repo.GetAllAsync();
    public Task<Trash?> Get(int id) => _repo.GetByIdAsync(id);
    public Task Add(Trash p) => _repo.AddAsync(p);
    public Task Update(Trash p) => _repo.UpdateAsync(p);
    public Task Delete(int id) => _repo.DeleteAsync(id);
}
