namespace Storage.API.Services.Contracts;

public interface IFileSystemService
{
    Task SaveFileBytesAsync(byte[] fileBytes, string filePath);
    Task<byte[]> GetFileBytesAsync(string filePath);
    void DeleteFile(string filePath);
    bool CheckIfFileExists(string filePath);
    bool CheckIfDirectoryExists(string path);
    void CreateDirectory(string path);
}