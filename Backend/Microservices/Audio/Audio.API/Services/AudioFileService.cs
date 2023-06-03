using Audio.API.DataTransferObjects;
using Audio.API.Exceptions;
using Audio.API.Model;
using Audio.API.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Audio.API.Services;

public class AudioFileService : IAudioFileService
{
    private readonly ApplicationDatabaseContext _context;
    private readonly IFileService _fileService;

    public AudioFileService(
        ApplicationDatabaseContext context,
        IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    public async Task<(byte[] file, string fileName)> GetAudioFileAsync(Guid fileId)
    {
        var file = await _context.AudioFiles.FirstOrDefaultAsync(x => x.Id == fileId);
        if (file is null)
        {
            throw new InvalidFileIdBadRequest(fileId);
        }
        var systemFileNameWithExtension = file.Id + file.Extension;
        var bytes = await _fileService.GetFormFileAsync(
            systemFileNameWithExtension, file.FileSystemName);
        return (bytes, file.FileSystemName + file.Extension);
    }

    public async Task SaveAudioFileAsync(AudioFileDTO dto, Guid userId)
    {
        var id = Guid.NewGuid();
    
        await _fileService.SaveFormFileAsync(dto.File, id.ToString());
    
        var audioFile = new AudioFile()
        {
            Id = id,
            Name = dto.Name,
            FileSystemName = dto.FileSystemName,
            Extension = dto.FileExtension,
            UserId = userId,
        };
    
        _context.AudioFiles.Add(audioFile);
        await _context.SaveChangesAsync();
    }
}