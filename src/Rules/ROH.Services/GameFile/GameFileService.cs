using ROH.Interfaces.Repository.GameFile;
using ROH.Interfaces.Services.ExceptionService;
using ROH.Interfaces.Services.GameFile;
using ROH.StandardModels.File;
using ROH.StandardModels.Response;

using System.Net;

namespace ROH.Services.GameFile;

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

            if (string.IsNullOrWhiteSpace(filePath))
                throw new InvalidOperationException("Cant find the file patch!");

            if (File.Exists(filePath))
            {
                byte[] fileContent = await File.ReadAllBytesAsync(filePath);

                return new DefaultResponse(new GameFileModel(
                    name: gameFile.Name,
                    format: gameFile.Format,
                    content: fileContent), HttpStatusCode.OK);
            }
            else
            {
                return new DefaultResponse(null, httpStatus: HttpStatusCode.NotFound, message: "File not found.");
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
            if (!Directory.Exists(file.Path))
            {
                _ = Directory.CreateDirectory(file.Path);
            }

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