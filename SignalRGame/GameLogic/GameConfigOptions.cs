namespace SignalRGame.GameLogic
{
    public class GameConfigOptions
    {
        public const string GameConfig = "GameConfig";

        public static float canvasWidth { get; set; } = 1000;
        public static float canvasHeight { get; set; } = 1000;

        public float gameSpeed { get; set; } = 2f;

        public float jetSpeed { get; set; }
        public float bulletSpeed { get; set; } = 2f;
        public int bulletLifetime { get; set; } = 3000;
        public float turnSpeed { get; set; } = 0.035f;
    }
}
