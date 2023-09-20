﻿namespace Files.Application.Features.Directory.DataTransferObjects;

public class CreateDirectoryDto
{
    public string Name { get; set; }
    public Guid? ParentDirectoryId { get; set; }
    public ICollection<Guid> Directories { get; set; }
    public ICollection<Guid> Files { get; set; }
}