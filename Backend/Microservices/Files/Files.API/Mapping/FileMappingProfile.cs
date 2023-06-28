using AutoMapper;
using Files.API.DataTransferObjects;
using Files.API.DataTransferObjects.File;
using File = Files.API.Model.File;

namespace Files.API.Mapping;

public class FileMappingProfile : Profile
{
    public FileMappingProfile()
    {
        CreateMap<UpdateFileDto, File>()
            .ForMember(x => x.Id, opts => opts.Ignore());
        CreateMap<FormFileDto, CreateFileDto>()
            .ForMember(x => x.Extension, 
                opts => opts.MapFrom(x => x.FileExtension));

        CreateMap<CreateFileDto, File>();
        CreateMap<File, FileDto>();
    }
}