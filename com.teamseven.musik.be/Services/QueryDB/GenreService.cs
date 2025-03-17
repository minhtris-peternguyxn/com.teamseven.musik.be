using AutoMapper;
using com.teamseven.musik.be.Models.Entities;
using com.teamseven.musik.be.Models.RequestDTO;
using com.teamseven.musik.be.Repositories.interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.teamseven.musik.be.Services.QueryDB
{
    public class GenreService
    {
        private readonly IGenreRepository _repo;
        private readonly IMapper _mapper;

        public GenreService(IGenreRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public Genre ConvertGenreDTO(GenreDataTransfer data)
        {
            return _mapper.Map<Genre>(data);
        }

        public GenreDataTransfer GetGenreEntityToDTO(Genre genre)
        {
            return _mapper.Map<GenreDataTransfer>(genre);
        }

        public async Task AddGenreAsync(GenreDataTransfer genre)
        {
            // Check if genre exists
            Genre tmp = await _repo.GetGerneByNameAsync(genre.GenreName);
            if (tmp != null)
            {
                throw new InvalidOperationException("Genre already exists.");
            }

            // Convert DTO to entity and add to repository
            Genre genreEntity = ConvertGenreDTO(genre);
            await _repo.AddGenreAsync(genreEntity);
        }

        public async Task<IEnumerable<Genre>> ListAllGenre()
        {
            return await _repo.GetAllGenreAsync();
        }

        public async Task<Genre> GetOneGenre(int id)
        {
            return await _repo.GetGenreAsync(id);
        }

        public async Task RemoveGenre(int id)
        {
            await _repo.DeleteGenreAsync(id);
        }
    }
}
