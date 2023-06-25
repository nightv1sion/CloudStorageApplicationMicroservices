using AutoMapper;
using Files.API.DataTransferObjects;
using Directory = Files.API.Model.Directory;

namespace Files.API.Mapping;

public class DirectoryMappingProfile : Profile
{
    public DirectoryMappingProfile()
    {
        CreateMap<CreateDirectoryDto, Directory>()
            .ForMember(x => x.ParentDirectory, 
                opts => opts.MapFrom(exp => exp.ParentId));
    }
}