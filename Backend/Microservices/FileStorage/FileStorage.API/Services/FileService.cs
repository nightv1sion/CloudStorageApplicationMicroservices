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

    public FileService(
        ApplicationDatabaseContext context,
        IStorageService storageService)
    {
        _context = context;
        _storageService = storageService;
    }

    public async Task<(byte[] file, string fileName)> GetFileBytesAsync(Guid fileId)
    {
        var file = await _context.Files.FirstOrDefaultAsync(x => x.Id == fileId);
        if (file is null)
        {
            throw new InvalidFileIdBadRequest(fileId);
        }
        var systemFileNameWithExtension = file.Id + file.Extension;
        var bytes = await _storageService.GetFileAsync(
            systemFileNameWithExtension, file.FileSystemName);
        return (bytes, file.FileSystemName + file.Extension);
    }

    public async Task SaveFileToStorageAsync(FileDto dto, Guid userId)
    {
        var id = Guid.NewGuid();
    
        await _storageService.SaveFormFileAsync(dto.File, id.ToString());
    
        var audioFile = new Model.File()
        {
            Id = id,
            Name = dto.Name,
            FileSystemName = dto.FileSystemName,
            Extension = dto.FileExtension,
            UserId = userId,
        };
    
        _context.Files.Add(audioFile);
        await _context.SaveChangesAsync();
    }
}