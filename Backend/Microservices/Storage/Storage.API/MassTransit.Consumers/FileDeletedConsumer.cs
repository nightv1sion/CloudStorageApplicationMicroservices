using MassTransit;
using MassTransitModels.File;
using Storage.API.Services.Contracts;

namespace Storage.API.MassTransit.Consumers;

public class FileDeletedConsumer : IConsumer<FileDeleted>
{
    private readonly IStorageService _storageService;

    public FileDeletedConsumer(IStorageService storageService)
    {
        _storageService = storageService;
    }

    public async Task Consume(ConsumeContext<FileDeleted> context)
    {
        _storageService.DeleteFile(context.Message.Name, context.Message.Extension);
    }
}