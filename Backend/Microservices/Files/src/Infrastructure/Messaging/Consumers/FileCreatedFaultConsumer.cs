using Microsoft.EntityFrameworkCore;
using Files.Domain.Enums;
using Files.Infrastructure.Persistence;
using MassTransit;
using QueueMessaging.File;

namespace Files.Infrastructure.Messaging.Consumers;
public class FileCreatedFaultConsumer : IConsumer<Fault<FileCreated>>
{
    private readonly ApplicationDatabaseContext _context;

    public FileCreatedFaultConsumer(
        ApplicationDatabaseContext context)
    {
        _context = context;
    }
    public async Task Consume(ConsumeContext<Fault<FileCreated>> context)
    {
        var faultMessage = context.Message.Message;
        if (Guid.TryParse(faultMessage.Name, out Guid fileId))
        {
            var file = await _context.Files.FirstOrDefaultAsync(x => x.Id == fileId);
            if (file is not null)
            {
                file.UploadingStatus = UploadingStatus.Fault;
                await _context.SaveChangesAsync();
            }
        }
    }
}
