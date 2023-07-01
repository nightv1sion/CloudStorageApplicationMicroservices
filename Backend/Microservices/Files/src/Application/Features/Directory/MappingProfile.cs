using AutoMapper;
using Files.Application.Features.Directory.DataTransferObjects;

namespace Files.Application.Features.Directory;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateDirectoryDto, Domain.Entities.Directory.Directory>()
            .ForMember(x => x.Directories, opts => opts.Ignore())
            .ForMember(x => x.Files, opts => opts.Ignore());
        
        CreateMap<UpdateDirectoryDto, Domain.Entities.Directory.Directory>()
            .ForMember(x => x.Directories, opts => opts.Ignore())
            .ForMember(x => x.Files, opts => opts.Ignore());

        
        CreateMap<Domain.Entities.Directory.Directory, DirectoryDto>()
            .ForMember(x => x.Files,
                opts => opts.MapFrom(exp => exp.Files.Select(f => f.Id)))
            .ForMember(x => x.Directories,
                opts => opts.MapFrom(exp => exp.Directories.Select(d => d.Id)));

    }
}