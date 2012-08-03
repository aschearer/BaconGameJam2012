namespace BaconGameJam.Win7
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (BaconGame game = new BaconGame())
            {
                game.Run();
            }
        }
    }
#endif
}

