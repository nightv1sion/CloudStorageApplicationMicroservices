using System.ComponentModel.DataAnnotations.Schema;

namespace Files.API.Model;

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
    public ICollection<File> Files { get; set; }
}