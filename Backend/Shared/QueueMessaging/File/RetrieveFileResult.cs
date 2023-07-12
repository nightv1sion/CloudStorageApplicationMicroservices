namespace Models.File;

public interface RetrieveFileResult
{
    public byte[] Bytes { get; }
    public double Size { get; }
}