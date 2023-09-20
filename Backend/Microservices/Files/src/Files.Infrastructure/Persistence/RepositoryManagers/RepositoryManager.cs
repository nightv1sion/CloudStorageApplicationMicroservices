﻿using Files.Infrastructure.Persistence.Repositories;
using Files.Infrastructure.Persistence.Repository.Interfaces;

namespace Files.Infrastructure.Persistence.RepositoryManagers;

public class RepositoryManager : IRepositoryManager
{
    private readonly Lazy<FileRepository> _fileRepository;
    private readonly Lazy<DirectoryRepository> _directoryRepository;
    private readonly ApplicationDatabaseContext _context;
    
    public RepositoryManager(ApplicationDatabaseContext context)
    {
        _context = context;
        _directoryRepository = new Lazy<DirectoryRepository>(() => new DirectoryRepository(context));
        _fileRepository = new Lazy<FileRepository>(() => new FileRepository(context));
    }

    public IFileRepository FileRepository => _fileRepository.Value;
    public IDirectoryRepository DirectoryRepository => _directoryRepository.Value;
    
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}