using Storage.API.DataTransferObjects;

namespace Storage.API.Services.Contracts;

public interface IStorageService
{
    Task<byte[]> GetFileBytesAsync(string fileNameWithExtension);
    Task SaveFormFileAsync(FormFileDto dto);
}