using System.Threading.Tasks;

namespace ROH.Launcher.Services
{
    public interface IFolderPicker
    {
        Task<string?> PickFolderAsync();
    }
}
