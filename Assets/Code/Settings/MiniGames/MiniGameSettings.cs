using Newtonsoft.Json;

namespace GameSettings
{
    public class MiniGameSettings
    {
        [JsonProperty] public MiniGameId MiniGameId { get; private set; }
        [JsonProperty] public int MaxGameSessionCount { get; private set; }
        [JsonProperty] public int RestoreTimeInSeconds { get; private set; }
        [JsonProperty] public int CountRestoredByAds { get; private set; }
    }
}
