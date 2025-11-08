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
        LoadNextMiniGame();
    }

    // Game State - Progression
    public void PassGame(int minigame_score) {
        GD.Print("GameState - PassGame");
        Game_Score += minigame_score;

        LoadNextMiniGame();
    }

    // Game State - Failure. End current run
    public void FailGame(int minigame_score) {
        // TODO: Collect Score, show to player / leaderboard
        GD.Print("GameState - FailGame");

        CallDeferred("GameOverScene");
    }
    
    public void LoadNextMiniGame() {
        // Next Round
        if (SceneIndex >= MiniGamePaths.Count) {
            SceneIndex = 0;

            // TODO: Update Game Timer
        }

        // Remove Timer
        StopGameTimer();
        RemoveTimerOverlay();

        GD.Print($"LoadNextMiniGame - Next Scene: {MiniGamePaths[SceneIndex]}");

        // Load Scene
        CallDeferred("ChangeGameScene", MiniGamePaths[SceneIndex]);

        SceneIndex++;
    }
    async void ChangeGameScene(String path) {
        GD.Print("Change Scene");
        // Transition Scene
        GetTree().ChangeSceneToFile("res://Scenes/noise.tscn");
        await ToSignal(GetTree().CreateTimer(1f), "timeout");

        // Set Scene
        GetTree().ChangeSceneToFile(path);

        // Game Timer
        StartGameTimer();
        InjectTimerOverlay();
    }
    async void GameOverScene()
    {
        // Remove Timer
        StopGameTimer();
        RemoveTimerOverlay();

        // Transition Scene
        GetTree().ChangeSceneToFile("res://Scenes/noise.tscn");
        await ToSignal(GetTree().CreateTimer(1f), "timeout");

        // Set Scene
        GetTree().ChangeSceneToFile("Scenes/MainMenu.tscn");
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
        GD.Print("GameTimer - Start");
        Game_Timer = GetTree().CreateTimer(Game_TimeLimit);
        Game_Timer.Timeout += GameTimerTimeoutHandler;
    }
    void GameTimerTimeoutHandler() {
        GD.Print("GameTimer - TIMEOUT");
        EmitSignal(SignalName.GameTimerTimeout);
    }
    public double GetGameTimer()
    {
        if (Game_Timer == null)
            return 0;

        return Game_Timer.TimeLeft;
    }
    public void StopGameTimer() {
        GD.Print($"GameTimer - Stop - {Game_Timer}");
        if (Game_Timer != null)
        {
            Game_Timer.Timeout -= GameTimerTimeoutHandler;
            Game_Timer = null;
        }
    }
}
