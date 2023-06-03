using FluentValidation;

using ROH.Domain.Version;
using ROH.Interfaces.Repository.Version;
using ROH.Models.Response;
using ROH.Validations.Version;

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
        private readonly IValidator<GameVersionFile> _validator;

        public GameVersionFileService(IGameVersionFileRepository gameVersionFileRepository, IValidator<GameVersionFile> validator)
        {
            _repository = gameVersionFileRepository;
            _validator = validator;
        }

        public async Task<DefaultResponse> GetFiles(GameVersion version)
        {
            var files = await _repository.GetFiles(version);

            return new DefaultResponse(ObjectResponse: files);
        }

        public async Task NewFile(GameVersionFile file)
        {

            var validation = await _validator.ValidateAsync(file);

            if (validation.IsValid)
            {
#if DEBUG
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                string path = @$"C:\ROHUpdateFiles\{file.GameVersion.Version}.{file.GameVersion.Release}.{file.GameVersion.Review}\{file.Name}"; // path to file
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#elif RELEASE
#endif
                using (FileStream fs = File.Create(path))
                {
                    // writing data in string
                    string dataAsString = "data"; //your data
                    byte[] info = new UTF8Encoding(true).GetBytes(dataAsString);
                    fs.Write(info, 0, info.Length);

                    // writing data in bytes already
                    byte[] data = new byte[] { 0x0 };
                    fs.Write(data, 0, data.Length);
                }
            }
            else
            {
                StringBuilder errors = new StringBuilder();
                foreach (var error in validation.Errors)
                {
                    errors.Append($";{error}");
                }
                string errorString = errors.ToString();

                throw new Exception(errorString);
            }

        }

    }
}
