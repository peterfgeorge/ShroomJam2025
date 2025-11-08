using Godot;
using System;
using Godot.Collections;

public partial class GameController : Node {
    public static GameController Instance { get; private set; }

    [Signal] public delegate void GameTimerTimeoutEventHandler();

    // Scene References
    public static readonly Array<String> MiniGamePaths = [
        "Scenes/MiniGame-Frogger.tscn",
        "Scenes/JetpackJoyride.tscn",
    ];

    // Timer overlay
    public static readonly String TimerOverlayPath = "Prefabs/TimerOverlay.tscn";

    // Game State
    public int SceneIndex = 0;
    public int Game_Score = 0;
    public float Game_TimeLimit = 20;
    public SceneTreeTimer Game_Timer;
    public bool gameStarted = false;

    public override void _Ready() {
        // Initialize Singleton
        Instance = this;
    }

    // Game State - Init. Begin new run
    public void StartGame() {
        gameStarted = true;
        // Reset Game State
        SceneIndex = 0;
        Game_Score = 0;

        // Create Game Permutation
        MiniGamePaths.Shuffle();

        // Load First Game
        StartGameTimer();
        CallDeferred("ChangeScene", MiniGamePaths[SceneIndex]);
    }

    // Game State - Progression
    public void PassGame(int minigame_score) {
        Game_Score += minigame_score;

        StopGameTimer();
        LoadNextMiniGame();
    }

    // Game State - Failure. End current run
    public void FailGame(int minigame_score) {
        // TODO: Collect Score, show to player / leaderboard

        StopGameTimer();
        CallDeferred("ChangeScene", "Scenes/MainMenu.tscn");
    }

    public void LoadNextMiniGame() {
        // Next Round
        if (++SceneIndex >= MiniGamePaths.Count) {
            SceneIndex = 0;

            // TODO: Update Game Timer
        }

        GD.Print($"LoadNextMiniGame - Next Scene Index = {SceneIndex}");

        // Load Scene
        StartGameTimer();
        CallDeferred("ChangeScene", MiniGamePaths[SceneIndex]);
    }
    async void ChangeScene(String path) {
        RemoveTimerOverlay();

        // Transition Scene
        GetTree().ChangeSceneToFile("res://Scenes/noise.tscn");
        await ToSignal(GetTree().CreateTimer(1f), "timeout");

        // Set Scene
        GetTree().ChangeSceneToFile(path);

        // Over Game Timer
        InjectTimerOverlay();
    }

    void RemoveTimerOverlay()
    {
        // Timer overlay assumed to be only child
        if (GetChildCount() > 0)
            GetChild<Control>(0).QueueFree();
    }
    void InjectTimerOverlay()
    {
        // Inject timer overlay as child
        PackedScene TimerOverlayPrefab = ResourceLoader.Load<PackedScene>(TimerOverlayPath);
        Control TimerOverlay = TimerOverlayPrefab.Instantiate<Control>();
        AddChild(TimerOverlay);
    }

    public void StartGameTimer() {
        Game_Timer = GetTree().CreateTimer(Game_TimeLimit);
        Game_Timer.Timeout += GameTimerTimeoutHandler;
    }
    void GameTimerTimeoutHandler() {
        EmitSignal(SignalName.GameTimerTimeout);
    }
    public double GetGameTimer()
    {
        if (Game_Timer == null)
            return 0;

        return Game_Timer.TimeLeft;
    }
    public void StopGameTimer() {
        if (Game_Timer != null)
        {
            Game_Timer.Timeout -= GameTimerTimeoutHandler;
            Game_Timer = null;
        }
    }
}
