namespace BaconGameJam.Common.Models.Sessions
{
    public class GameSettings : ModelBase
    {
        public GameSettings()
        {
            this.IsSoundEnabled = true;
            this.IsMusicEnabled = true;
        }

        public bool IsSoundEnabled { get; set; }
        public bool IsMusicEnabled { get; set; }
    }
}