using ROH.StandardModels.File;

namespace ROH.Blazor.Server.Interfaces.Helpers;

public interface IDownloadFileService
{
    Task Download(GameFileModel fileModel);
}