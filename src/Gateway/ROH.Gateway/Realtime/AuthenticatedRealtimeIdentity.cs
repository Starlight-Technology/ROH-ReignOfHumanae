namespace ROH.Gateway.Realtime;

public sealed record AuthenticatedRealtimeIdentity(
    string AccountId,
    string CharacterId,
    string DisplayName,
    string WorldId,
    RealtimePlayerTransform InitialTransform);

public sealed record RealtimePlayerTransform(
    float X,
    float Y,
    float Z,
    float RotationX,
    float RotationY,
    float RotationZ,
    float RotationW);
