using AgathaChristie.Application.DTOs;
using AgathaChristie.Application.Interfaces;

namespace AgathaChristie.Application.Services;

public class DetectiveService
{
    private readonly IDetectiveRepository _detectiveRepository;

    public DetectiveService(IDetectiveRepository detectiveRepository)
    {
        _detectiveRepository = detectiveRepository;
    }

    public async Task<IEnumerable<DetectiveResponse>> GetAllAsync()
    {
        var detectives = await _detectiveRepository.GetAllAsync();
        return detectives.Select(d => new DetectiveResponse
        {
            Id = d.Id,
            Name = d.Name,
            HexColor = d.HexColor
        });
    }

    public async Task<DetectiveResponse?> GetByIdAsync(Guid id)
    {
        var detective = await _detectiveRepository.GetByIdAsync(id);
        return detective is null ? null : new DetectiveResponse
        {
            Id = detective.Id,
            Name = detective.Name,
            HexColor = detective.HexColor
        };
    }
}