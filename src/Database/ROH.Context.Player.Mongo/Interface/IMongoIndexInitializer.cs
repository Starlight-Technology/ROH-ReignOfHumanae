namespace ROH.Context.Player.Mongo.Interface;

public interface IMongoIndexInitializer
{
    Task EnsureIndexesAsync(CancellationToken cancellationToken = default);
}
