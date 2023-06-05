using AutoMapper;
using Files.API.DataTransferObjects;
using File = Files.API.Model.File;

namespace Files.API.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UpdateFileDto, Model.File>()
            .ForMember(x => x.Id, opts => opts.Ignore());
    }
}