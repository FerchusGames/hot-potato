namespace Ingvar.LiveWatch.TowerDefenceDemo
{
    // You can expose sort order values to separate const variables for more convenient usage
    public static class TD_WatchSortOrder
    {
        public const int Frame = -1002;
        public const int FPS = -1001;
        public const int TimeScale = -1000;
        public const int State = -100;
        public const int Health = -90;
        public const int Energy = -80;
        public const int TowerBuildLog = -70;
        public const int MobLog = -60;
        // Variables which order is not set would be here (between 0 and 1 exclusive)
        public const int LevelConfig = 1000;
        public const int EconomyConfig = 1001;
    }
}