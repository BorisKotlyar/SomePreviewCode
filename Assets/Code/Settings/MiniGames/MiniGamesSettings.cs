using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace GameSettings
{
    public class MiniGamesSettings
    {
        [JsonProperty] public ReadOnlyCollection<MiniGameSettings> Games { get; private set; }
    }
}
