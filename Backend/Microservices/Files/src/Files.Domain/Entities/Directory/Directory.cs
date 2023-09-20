using System.ComponentModel.DataAnnotations.Schema;

namespace Files.Domain.Entities.Directory;

public class Directory
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid UserId { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    [ForeignKey("ParentDirectory")]
    public Guid? ParentDirectoryId { get; set; }
    public Directory ParentDirectory { get; set; }
    public ICollection<Directory> Directories { get; set; }
    public ICollection<Files.Domain.Entities.File.File> Files { get; set; }
}