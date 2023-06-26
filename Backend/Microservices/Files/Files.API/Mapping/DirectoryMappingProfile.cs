using AutoMapper;
using Files.API.DataTransferObjects.Directory;
using Directory = Files.API.Model.Directory;

namespace Files.API.Mapping;

public class DirectoryMappingProfile : Profile
{
    public DirectoryMappingProfile()
    {
        CreateMap<CreateDirectoryDto, Directory>()
            .ForMember(x => x.ParentDirectoryId,
                opts => opts.MapFrom(exp => exp.ParentId))
            .ForMember(x => x.Directories, opts => opts.Ignore())
            .ForMember(x => x.Files, opts => opts.Ignore());
        
        CreateMap<UpdateDirectoryDto, Directory>()
            .ForMember(x => x.ParentDirectoryId,
                opts => opts.MapFrom(exp => exp.ParentId))
            .ForMember(x => x.Directories, opts => opts.Ignore())
            .ForMember(x => x.Files, opts => opts.Ignore());

        
        CreateMap<Directory, DirectoryDto>()
            .ForMember(x => x.Files,
                opts => opts.MapFrom(exp => exp.Files.Select(f => f.Id)))
            .ForMember(x => x.Directories,
                opts => opts.MapFrom(exp => exp.Directories.Select(d => d.Id)));

    }
}