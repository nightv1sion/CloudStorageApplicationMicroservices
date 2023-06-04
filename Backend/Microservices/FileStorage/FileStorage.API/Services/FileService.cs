using AutoMapper;
using FileStorage.API.DataTransferObjects;
using FileStorage.API.Exceptions;
using FileStorage.API.Model;
using FileStorage.API.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using File = FileStorage.API.Model.File;

namespace FileStorage.API.Services;

public class FileService : IFileService
{
    private readonly ApplicationDatabaseContext _context;
    private readonly IStorageService _storageService;
    private readonly IMapper _mapper;

    public FileService(
        ApplicationDatabaseContext context,
        IStorageService storageService,
        IMapper mapper)
    {
        _context = context;
        _storageService = storageService;
        _mapper = mapper;
    }
    public async Task<File> GetFileAsync(Guid userId, Guid fileId)
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
    public async Task<ICollection<File>> GetFilesByUserIdAsync(Guid userId)
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
    public async Task<(byte[] file, string fileName)> GetFileBytesAsync(Guid userId, Guid fileId)
    {
        var file = await _context.Files
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync(x => x.Id == fileId);
        
        if (file is null)
        {
            throw new InvalidFileIdBadRequest(fileId);
        }
        var systemFileNameWithExtension = file.Id + file.Extension;
        var bytes = await _storageService.GetFileAsync(
            systemFileNameWithExtension, file.Name);
        return (bytes, file.Name + file.Extension);
    }
    public async Task SaveFileToStorageAsync(FormFileDto dto, Guid userId)
    {
        var id = Guid.NewGuid();
    
        await _storageService.SaveFormFileAsync(dto.File, id.ToString());
    
        var audioFile = new Model.File()
        {
            Id = id,
            Name = dto.Name,
            Extension = dto.FileExtension,
            UserId = userId,
        };
    
        _context.Files.Add(audioFile);
        await _context.SaveChangesAsync();
    }
}