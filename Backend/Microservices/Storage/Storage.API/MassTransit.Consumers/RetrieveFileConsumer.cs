using MassTransit;
using Models.File;
using Storage.API.Services.Contracts;

namespace Storage.API.MassTransit.Consumers;

public class RetrieveFileConsumer : 
    IConsumer<RetrieveFile>
{
    private readonly IStorageService _storageService;

    public RetrieveFileConsumer(IStorageService storageService)
    {
        _storageService = storageService;
    }
    public async Task Consume(ConsumeContext<RetrieveFile> context)
    {
        var bytes = await _storageService.GetFileBytesAsync(
            context.Message.Name + context.Message.Extension);

        await context.RespondAsync<RetrieveFileResult>(new
        {
            Bytes = bytes,
            Size = bytes.Length
        });
    }
}