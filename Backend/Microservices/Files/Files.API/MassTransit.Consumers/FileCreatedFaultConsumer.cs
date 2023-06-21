using Files.API.Model;
using Files.API.Model.Enums;
using MassTransit;
using MassTransitModels.File;
using Microsoft.EntityFrameworkCore;

namespace Files.API.MassTransit.Consumers;
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