namespace QueueMessaging.File;

public interface FileCreated
{
    public string Name { get; }
    public string Extension { get; }
    public byte[] Bytes { get; }
}