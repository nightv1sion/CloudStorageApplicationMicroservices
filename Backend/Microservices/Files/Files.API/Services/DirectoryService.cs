using AutoMapper;
using Files.API.DataTransferObjects;
using Files.API.DataTransferObjects.Directory;
using Files.API.Exceptions;
using Files.API.Model;
using Files.API.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using Directory = Files.API.Model.Directory;
using File = Files.API.Model.File;

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

    public async Task<DirectoryDto> GetDirectoryAsync(Guid userId, Guid directoryId)
    {
        var directory = await FindDirectoryAsync(userId, directoryId);
        var dto = _mapper.Map<DirectoryDto>(directory);
        return dto;
    }
    public async Task<ICollection<DirectoryDto>> GetDirectoriesAsync(Guid userId)
    {
        var directories = await _context.Directories
            .Include(x => x.Files)
            .Include(x => x.Directories)
            .Where(x => x.UserId == userId).ToListAsync();
        
        return _mapper.Map<ICollection<DirectoryDto>>(directories);
    }
    public async Task<DirectoryDto> CreateDirectoryAsync(Guid userId, CreateDirectoryDto dto)
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

        if (dto.Directories.Count > 0)
        {
            directory.Directories = new List<Directory>();
            foreach (var directoryId in dto.Directories)
            {
                var dir = await FindDirectoryAsync(userId, directoryId);
                directory.Directories.Add(dir);
            }
        }

        if (dto.Files.Count > 0)
        {
            directory.Files = new List<File>();
            foreach (var fileId in dto.Files)
            {
                var file = await _context.Files.FirstOrDefaultAsync(x => x.Id == fileId);
                if (file is null)
                {
                    throw new InvalidFileIdBadRequestException(fileId);
                }
                directory.Files.Add(file);
            }
        }
        
        _context.Directories.Add(directory);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation($"Directory with id: {directory.Id} created");
        return _mapper.Map<DirectoryDto>(directory);
    }

    public async Task UpdateDirectoryAsync(Guid userId, UpdateDirectoryDto dto)
    {
        var directory = await _context.Directories
            .Include(x => x.Directories)
            .Include(x => x.Files)
            .FirstOrDefaultAsync(x => x.UserId == userId && x.Id == dto.Id);

        if (directory is null)
        {
            throw new InvalidDirectoryIdBadRequestException(dto.Id);
        }

        _mapper.Map(dto, directory);
        
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
        directory.Updated = DateTime.Now;;
        if (dto.Directories.Count > 0)
        {
            directory.Directories.Clear();
            foreach (var directoryId in dto.Directories)
            {
                var dir = await FindDirectoryAsync(userId, directoryId);
                directory.Directories.Add(dir);
            }
        }

        if (dto.Files.Count > 0)
        {
            directory.Files.Clear();
            foreach (var fileId in dto.Files)
            {
                var file = await _context.Files.FirstOrDefaultAsync(x => x.Id == fileId);
                if (file is null)
                {
                    throw new InvalidFileIdBadRequestException(fileId);
                }
                directory.Files.Add(file);
            }
        }
        _context.Directories.Update(directory);
        await _context.SaveChangesAsync();
        _logger.LogInformation($"Directory with id: {directory.Id} updated");
    }
    public async Task DeleteDirectoryAsync(Guid userId, Guid directoryId)
    {
        var directory = await FindDirectoryAsync(userId, directoryId);
        _context.Directories.Remove(directory);
        await _context.SaveChangesAsync();
        _logger.LogInformation($"Directory with id: {directory.Id} deleted");

    }
    private async Task<Directory> FindDirectoryAsync(Guid userId, Guid directoryId)
    {
        var directory = await _context.Directories
            .FirstOrDefaultAsync(x => x.UserId == userId && x.Id == directoryId);
        if (directory is null)
        {
            throw new InvalidDirectoryIdBadRequestException(directoryId);
        }

        return directory;
    }
}