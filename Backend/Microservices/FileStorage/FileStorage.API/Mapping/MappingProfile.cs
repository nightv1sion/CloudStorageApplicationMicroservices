using AutoMapper;
using FileStorage.API.DataTransferObjects;
using File = FileStorage.API.Model.File;

namespace FileStorage.API.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UpdateFileDto, File>()
            .ForMember(x => x.Id, opts => opts.Ignore());
    }
}