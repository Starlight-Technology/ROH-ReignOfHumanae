using ROH.Domain.Version;
using ROH.Interfaces.Repository.Version;
using ROH.Models.Response;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROH.Services.Version
{
    public class GameVersionFileService
    {
        private readonly IGameVersionFileRepository _repository;

        public GameVersionFileService(IGameVersionFileRepository gameVersionFileRepository)
        {
            _repository = gameVersionFileRepository;
        }

        public async Task<DefaultResponse> GetFiles(GameVersion version)
        {
            var files = await _repository.GetFiles(version);

            return new DefaultResponse(ObjectResponse: files);
        }

        public async Task NewFile(GameVersionFile file)
        {
            try
            {

            }
            catch (Exception e)
            {

            }
        }

    }
}
