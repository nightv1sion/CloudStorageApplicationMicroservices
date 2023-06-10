namespace MassTransitModels.File;

public interface FileCreated
{
    public string Name { get; set; }
    public string Extension { get; set; }
    public byte[] Bytes { get; set; }
}