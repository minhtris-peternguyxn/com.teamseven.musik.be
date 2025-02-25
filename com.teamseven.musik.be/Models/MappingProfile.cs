

using AutoMapper;
using com.teamseven.musik.be.Models.Entities;
using com.teamseven.musik.be.Models.Request;
using com.teamseven.musik.be.Models.RequestDTO;
namespace com.teamseven.musik.be.Models
{
    public class MappingProfile : Profile
    {
    public MappingProfile()
    {
        CreateMap<Track, TrackMusicRequest>();
        CreateMap<TrackMusicRequest, Track>();

        CreateMap<Genre, GenreDataTransfer >();
        CreateMap<GenreDataTransfer, Genre>();

        CreateMap<Artist, SingerInfoDataTransfer>();
        CreateMap<SingerInfoDataTransfer, Artist>();
    }
}
}
