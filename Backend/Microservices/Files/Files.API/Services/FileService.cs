using AutoMapper;
using Files.API.DataTransferObjects;
using Files.API.Exceptions;
using Files.API.Model;
using Files.API.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using File = Files.API.Model.File;

namespace Files.API.Services;

public class FileService : IFileService
{
    private readonly ApplicationDatabaseContext _context;
    private readonly IMapper _mapper;

    public FileService(
        ApplicationDatabaseContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<Model.File> GetFileAsync(Guid userId, Guid fileId)
    {
        var file = await _context.Files
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync(x => x.Id == fileId);
        
        if (file is null)
        {
            throw new InvalidFileIdBadRequest(fileId);
        }

        return file;
    }
    public async Task<ICollection<Model.File>> GetFilesByUserIdAsync(Guid userId)
    {
        var files = await _context.Files
            .Where(x => x.UserId == userId)
            .ToListAsync();

        return files;
    }

    public async Task UpdateFileAsync(Guid userId, UpdateFileDto dto)
    {
        var file = await _context.Files
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync(x => x.Id == dto.Id);

        if (file is null)
        {
            throw new InvalidFileIdBadRequest(userId);
        }

        _mapper.Map(dto, file);

        await _context.SaveChangesAsync();
    }

    public async Task<File> CreateFileAsync(CreateFileDto dto)
    {
        var entity = _mapper.Map<File>(dto);

        _context.Files.Add(entity);

        await _context.SaveChangesAsync();

        return entity;
    }
}