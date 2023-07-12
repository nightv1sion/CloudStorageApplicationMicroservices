using MassTransit;
using QueueMessaging.File;
using Models.File;
using Storage.API.Services.Contracts;

namespace Storage.API.MassTransit.Consumers;

public class FileCreatedConsumer : IConsumer<FileCreated>
{
    private readonly IStorageService _storageService;
    private readonly IPublishEndpoint _publishEndpoint;

    public FileCreatedConsumer(
        IStorageService storageService,
        IPublishEndpoint publishEndpoint)
    {
        _storageService = storageService;
        _publishEndpoint = publishEndpoint;
    }
    public async Task Consume(ConsumeContext<FileCreated> context)
    {
        await _storageService.SaveFileBytesAsync(
            context.Message.Bytes, context.Message.Name, context.Message.Extension);
        
        await _publishEndpoint.Publish<FileSaved>(new
        {
            Name = context.Message.Name,
            Extension = context.Message.Extension
        });
    }
}