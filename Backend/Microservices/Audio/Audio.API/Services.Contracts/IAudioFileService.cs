using Audio.API.DataTransferObjects;
using Audio.API.Model;

namespace Audio.API.Services.Contracts;

public interface IAudioFileService
{
    public Task<(byte[] file, string fileName)> GetAudioFileAsync(Guid fileId);
    public Task SaveAudioFileAsync(AudioFileDTO dto, Guid userId);
}