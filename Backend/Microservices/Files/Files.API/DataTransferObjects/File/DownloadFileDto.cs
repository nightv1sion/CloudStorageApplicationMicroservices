namespace Files.API.DataTransferObjects.File;

public class DownloadFileDto
{
    public byte[] Bytes { get; set; }
    public string Name { get; set; }
    public string Extension { get; set; }
}