using MassTransit;
using MassTransitModels.File;
using Storage.API.Services.Contracts;

namespace Storage.API.MassTransit.Consumers;

public class FileCreatedConsumer : IConsumer<FileCreated>
{
    private readonly IStorageService _storageService;

    public FileCreatedConsumer(IStorageService storageService)
    {
        _storageService = storageService;
    }
    public async Task Consume(ConsumeContext<FileCreated> context)
    {
        await _storageService.SaveFileBytesAsync(
            context.Message.Bytes, context.Message.Name, context.Message.Extension);
    }
}