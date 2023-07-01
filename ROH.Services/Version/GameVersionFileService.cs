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
                string path = @$"C:\ROHUpdateFiles\{file.GameVersion.Version}.{file.GameVersion.Release}.{file.GameVersion.Review}\"; // path to file
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#elif RELEASE
                throw NotImplementedException();
#endif
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path) ?? Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\ROHFiles");
                }

                if (File.Exists(path + file.Name))
                    File.Delete(path + file.Name);

                using FileStream fs = File.Create(path + file.Name);
                // writing data in string
                byte[] info = new UTF8Encoding(true).GetBytes(file.Content);
                fs.Write(info, 0, info.Length);

                await _repository.SaveFile(file);
            }
            else
            {
                StringBuilder errors = new();
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
