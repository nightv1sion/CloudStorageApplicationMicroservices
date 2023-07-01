using MassTransit;
using Microsoft.EntityFrameworkCore;
using Models.File;
using Files.Domain.Enums;
using Files.Infrastructure.Persistence;

namespace Files.Infrastructure.Messaging.Consumers;

public class FileSavedConsumer : IConsumer<FileSaved>
{
    private readonly ApplicationDatabaseContext _context;

    public FileSavedConsumer(ApplicationDatabaseContext context)
    {
        _context = context;
    }
    public async Task Consume(ConsumeContext<FileSaved> context)
    {
        if (Guid.TryParse(context.Message.Name, out Guid fileId))
        {
            var file = await _context.Files.FirstOrDefaultAsync(x => x.Id == fileId);
            if (file is not null)
            {
                file.UploadingStatus = UploadingStatus.Completed;
                await _context.SaveChangesAsync();
            }
        }
    }
}

