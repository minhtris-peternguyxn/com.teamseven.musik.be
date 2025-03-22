using AutoMapper;
using com.teamseven.musik.be.Models.Entities;
using com.teamseven.musik.be.Models.RequestDTO;
using com.teamseven.musik.be.Services.Interfaces;
using com.teamseven.musik.be.Repositories.interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.teamseven.musik.be.Services.QueryDB
{
    public class GenreService: IGenreService
    {
        private readonly IGenreRepository _repo;
        private readonly IMapper _mapper;

        public GenreService(IGenreRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public Genre ConvertGenreDTO(GenreRequest data)
        {
            return _mapper.Map<Genre>(data);
        }

        public GenreRequest GetGenreEntityToDTO(Genre genre)
        {
            return _mapper.Map<GenreRequest>(genre);
        }

        public async Task AddGenreAsync(GenreRequest genre)
        {
            // Check if genre exists
            if(genre == null) throw new ArgumentNullException(nameof(genre));

            if (await _repo.CheckGenresExitstAsync(genre.GenreName))
                  throw new InvalidOperationException("Genre already exists.");
            //map cc phế vl
            Genre g = new Genre()
            {
                GenreName = genre.GenreName,
                Img = genre.Img,
                CreatedDate = DateTime.UtcNow
            };

            await _repo.AddGenreAsync(g);
        }

        public async Task<IEnumerable<Genre>> ListAllGenre()
        {
            return await _repo.GetAllGenreAsync();
        }

        public async Task<Genre?> GetOneGenre(int id)
        {
            return await _repo.GetGenreAsync(id);
        }

        public async Task RemoveGenre(int id)
        {
            await _repo.DeleteGenreAsync(id);
        }
    }
}
