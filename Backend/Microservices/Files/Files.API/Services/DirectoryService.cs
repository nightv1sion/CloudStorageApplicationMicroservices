using AutoMapper;
using Files.API.DataTransferObjects;
using Files.API.Exceptions;
using Files.API.Model;
using Files.API.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using Directory = Files.API.Model.Directory;

namespace Files.API.Services;

public class DirectoryService : IDirectoryService
{
    private readonly ApplicationDatabaseContext _context;
    private readonly ILogger<DirectoryService> _logger;
    private readonly IMapper _mapper;

    public DirectoryService(
        ApplicationDatabaseContext context,
        ILogger<DirectoryService> logger,
        IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;

        _logger.LogInformation("Directory Service is called");
    }

    public async Task<Directory> CreateDirectoryAsync(CreateDirectoryDto dto, Guid userId)
    {
        var directory = _mapper.Map<Directory>(dto);
        if (directory.ParentDirectoryId.HasValue)
        {
            var parent = await _context.Directories.FirstOrDefaultAsync(
                x => x.Id == dto.ParentId);

            if (parent is null)
            {
                throw new InvalidDirectoryIdBadRequestException(directory.ParentDirectoryId.Value);
            }

            directory.ParentDirectoryId = parent.Id;
        }
        directory.Created = DateTime.Now;;
        directory.UserId = userId;
        
        _context.Directories.Add(directory);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation($"Directory with id: {directory.Id} created");
        return directory;
    }
}