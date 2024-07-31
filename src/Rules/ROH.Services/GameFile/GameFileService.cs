using ROH.Domain.GameFiles;
using ROH.Interfaces.Repository.GameFile;
using ROH.Interfaces.Repository.Version;
using ROH.Interfaces.Services.ExceptionService;
using ROH.Interfaces.Services.GameFile;
using ROH.StandardModels.Response;
using ROH.StandardModels.Version;

using System.Net;

namespace ROH.Services.GameFile
{
    public class GameFileService(IGameFileRepository gameFileRepository, IExceptionHandler exceptionHandler) : IGameFileService
    {
        public async Task<DefaultResponse> DownloadFile(Guid fileGuid)
        {
            try
            {
                Domain.GameFiles.GameFile? file = await gameFileRepository.GetFile(fileGuid);

                return file is null ? new DefaultResponse(null, httpStatus: HttpStatusCode.NotFound, message: "File Not Found.") : await GetGameFile(file);
            }
            catch (Exception ex)
            {
                return exceptionHandler.HandleException(ex);
            }
        }

        public async Task<DefaultResponse> DownloadFile(long id)
        {
            try
            {
                Domain.GameFiles.GameFile? file = await gameFileRepository.GetFile(id);

                return file is null ? new DefaultResponse(null, httpStatus: HttpStatusCode.NotFound, message: "File Not Found.") : await GetGameFile(file);
            }
            catch (Exception ex)
            {
                return exceptionHandler.HandleException(ex);
            }
        }

        private async Task<DefaultResponse> GetGameFile(Domain.GameFiles.GameFile gameFile)
        {
            try
            {
                string filePath = Path.Combine(gameFile.Path, gameFile.Name);

                if (File.Exists(filePath))
                {
                    byte[] fileContent = await File.ReadAllBytesAsync(filePath);

                    return new DefaultResponse(new GameVersionFileModel(
                        name: gameFile.Name,
                        format: gameFile.Format,
                        content: fileContent).ToFileModel(), HttpStatusCode.OK);
                }
                else
                {
                    return new DefaultResponse(null, httpStatus: HttpStatusCode.NotFound, message: "File Not Found.");
                }
            }
            catch (Exception ex)
            {
                return exceptionHandler.HandleException(ex);
            }
        }

        public async Task SaveFileAsync(Domain.GameFiles.GameFile file, byte[] content)
        {
            try
            {
                string filePath = Path.Combine(file.Path, file.Name);

                if (File.Exists(filePath))
                    File.Delete(filePath);

                using (FileStream fs = File.Create(filePath))
                {
                    await fs.WriteAsync(content.AsMemory(), CancellationToken.None);
                }

                await gameFileRepository.SaveFile(file);
            }
            catch (Exception ex)
            {
                _ = exceptionHandler.HandleException(ex);
            }
        }
    }
}