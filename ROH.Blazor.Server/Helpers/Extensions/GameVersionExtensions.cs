using ROH.StandardModels.Version;

namespace ROH.Blazor.Server.Helpers.Extensions
{
    public static class GameVersionExtensions
    {
        public static List<GameVersionListModel> ToListModel(this List<GameVersionModel> gameVersions)
        {
            return gameVersions.Select(version => version.ToListModel()).ToList();
        }
    }
}
