using AutoMapper;
using Files.Application.Features.File.DataTransferObjects;

namespace Files.Application.Features.File;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UpdateFileDto, Domain.Entities.File.File>()
            .ForMember(x => x.Id, opts => opts.Ignore());
        CreateMap<FormFileDto, CreateFileDto>()
            .ForMember(x => x.Extension, 
                opts => opts.MapFrom(x => x.FileExtension));

        CreateMap<CreateFileDto, Domain.Entities.File.File>();
        CreateMap<Domain.Entities.File.File, FileDto>();
    }
}