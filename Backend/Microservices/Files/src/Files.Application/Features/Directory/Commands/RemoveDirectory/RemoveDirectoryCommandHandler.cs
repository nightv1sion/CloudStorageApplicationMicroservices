﻿using Files.Application.Extensions.Interfaces;
using MediatR;

namespace Files.Application.Features.Directory.Commands.RemoveDirectory;

public class RemoveDirectoryCommandHandler : IRequestHandler<RemoveDirectoryCommand>
{
    private readonly IDirectoryService _directoryService;

    public RemoveDirectoryCommandHandler(
        IDirectoryService directoryService)
    {
        _directoryService = directoryService;
    }
    public async Task Handle(
        RemoveDirectoryCommand request, 
        CancellationToken cancellationToken)
    {
        await _directoryService.DeleteDirectoryAsync(request.UserId, request.ParentDirectoryId, request.DirectoryId,
            cancellationToken);
    }
}