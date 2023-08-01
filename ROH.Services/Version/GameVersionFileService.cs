using FluentValidation;

using ROH.Domain.Version;
using ROH.Interfaces.Repository.Version;
using ROH.Interfaces.Services.Version;
using ROH.Models.Response;

using System.IO;
using System.Net;
using System.Text;

namespace ROH.Services.Version
{
    public class GameVersionFileService : IGameVersionFileService
    {
        private readonly IGameVersionFileRepository _repository;
        private readonly IGameVersionService _gameVersion;
        private readonly IValidator<GameVersionFile> _validator;

        public GameVersionFileService(IGameVersionFileRepository gameVersionFileRepository, IValidator<GameVersionFile> validator, IGameVersionService gameVersion)
        {
            _repository = gameVersionFileRepository;
            _validator = validator;
            _gameVersion = gameVersion;
        }

        public async Task<DefaultResponse> DownloadFile(long id)
        {
            var file = await _repository.GetFile(id);

            if (file != null)
            {

                var validation = await _validator.ValidateAsync(file);
                if (validation.IsValid)
                {
                    if (file.GameVersion != null &&
                        await _gameVersion.VerifyIfVersionExist(file.GameVersion))
                    {
                        string path = string.Empty;
#if DEBUG
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        path = @$"C:\ROHUpdateFiles\{file.GameVersion.Version}.{file.GameVersion.Release}.{file.GameVersion.Review}\"; // path to file
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#elif RELEASE
                throw NotImplementedException();
#endif
                        var fileContent = await File.ReadAllLinesAsync(path + file.Name);

                        return new DefaultResponse(fileContent[0], HttpStatusCode.OK);
                    }
                    else
                    {
                        return new DefaultResponse(null, HttpStatus: HttpStatusCode.NotFound, Message: "Game Version Not Found.");
                    }
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
            else
            {
                return new DefaultResponse(null, HttpStatus: HttpStatusCode.NotFound, Message: "File Not Found.");
            }
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
                if (file.GameVersion != null &&
                    await _gameVersion.VerifyIfVersionExist(file.GameVersion))
                {
                    string path = "";
#if DEBUG
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    path = @$"C:\ROHUpdateFiles\{file.GameVersion.Version}.{file.GameVersion.Release}.{file.GameVersion.Review}\"; // path to file
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#elif RELEASE
                throw NotImplementedException();
#endif
                    file = file with { Path = path };

                    if (!Directory.Exists(Path.GetDirectoryName(path)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(path) ?? Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\ROHFiles");
                    }

                    if (File.Exists(path + file.Name))
                        File.Delete(path + file.Name);

                    using FileStream fs = File.Create(path + file.Name);

                    byte[] info = new UTF8Encoding(true).GetBytes(file.Content);
                    await fs.WriteAsync(info);

                    await _repository.SaveFile(file);
                }
                else
                {
                    throw new Exception("Game Version Not Found.");
                }
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
