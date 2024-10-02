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
            Domain.GameFiles.GameFile? file = await gameFileRepository.GetFileAsync(fileGuid);

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
            Domain.GameFiles.GameFile? file = await gameFileRepository.GetFileAsync(id);

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
                throw new InvalidOperationException("Cant find the file path!");

            if (File.Exists(filePath))
            {
                byte[] fileContent = await File.ReadAllBytesAsync(filePath);

                return new DefaultResponse(new GameFileModel(
                    name: gameFile.Name,
                    format: gameFile.Format,
                    content: fileContent,
                    size: gameFile.Size,
                    active: gameFile.Active), HttpStatusCode.OK);
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

            await gameFileRepository.SaveFileAsync(file);
        }
        catch (Exception ex)
        {
            _ = exceptionHandler.HandleException(ex);
        }
    }

    public async Task<DefaultResponse> MakeFileHasDeprecatedAsync(Guid fileGuid)
    {
        try
        {
            Domain.GameFiles.GameFile? file = await gameFileRepository.GetFileAsync(fileGuid);

            if(file is null)
                return new DefaultResponse(null, httpStatus: HttpStatusCode.NotFound, message: "File not found.");

            file = file with { Active = false };

            await gameFileRepository.UpdateFileAsync(file);

            return new DefaultResponse(HttpStatusCode.OK, message: $"The file \"{file.Name}\" of version has marked as deprecated.");
        }
        catch (Exception ex)
        {
            return exceptionHandler.HandleException(ex);
        }
    }
}