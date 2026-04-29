using AgathaChristie.Application.Interfaces;
using AgathaChristie.Domain.Models;
using AgathaChristie.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AgathaChristie.Infrastructure.Repositories;

public class DetectiveRepository : IDetectiveRepository
{
    private readonly AppDbContext _db;

    public DetectiveRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Detective>> GetAllAsync() =>
        await _db.Detectives.ToListAsync();

    public async Task<Detective?> GetByIdAsync(Guid id) =>
        await _db.Detectives.FindAsync(id);
}