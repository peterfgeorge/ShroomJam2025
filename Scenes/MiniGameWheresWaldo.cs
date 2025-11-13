using Godot;
using Godot.Collections;
using System;

public partial class MiniGameWheresWaldo : Control {
    [Export] public PackedScene MovingObjectScene;
    [Export] public int[] ObjectCount = [35,50,65,80,90];
    [Export] SpriteFrames sprites;
    [Export] TextureRect display;

    private GameController gameController => GameController.Instance;
    private int index;

    public static readonly Array<string> characters = [
       "Daphne",
        "Freddy",
        "Scooby",
        "Shaggy",
        "Velma"
   ];

    public static string currentWaldo = "Scooby";

    public override void _Ready() {
        GameController.Instance.GameTimerTimeout += Timeout;
        int round = gameController != null ? gameController.GameRound : 0;
        index = Mathf.Clamp(round, 0, ObjectCount.Length - 1);
        SpawnObjects();
        display.Texture = sprites.GetFrameTexture(currentWaldo + "Idle", 0);
    }

    private void SpawnObjects() {
        var ysort = GetNode<Node2D>("YSort");
        currentWaldo = characters.PickRandom();

        var screenSize = GetViewportRect().Size;
        float minY = 70;
        float maxY = screenSize.Y - 30;

        // Spawn main Waldo
        WaldoNpc waldo = MovingObjectScene.Instantiate<WaldoNpc>();
        waldo.SetSprite(currentWaldo);

        float x1 = (float)GD.RandRange(50, screenSize.X - 50);
        float y1 = (float)GD.RandRange(minY, maxY);
        waldo.Position = new Vector2(x1, y1);

        // Calculate depth scaling based on Y position
        float ratio1 = Mathf.InverseLerp(minY, maxY, y1);
        float scale1 = Mathf.Lerp(0.6f, 1.0f, ratio1);
        float speedMultiplier1 = Mathf.Lerp(0.5f, 1.0f, ratio1);

        waldo.Scale = new Vector2(scale1, scale1);
        waldo.Speed = (float)(GD.RandRange(60, 150) * speedMultiplier1);
        waldo.MoveDistance = (float)GD.RandRange(50, 100);

        ysort.AddChild(waldo);

        // Spawn other objects
        for (int i = 0; i < ObjectCount[index]; i++) {
            WaldoNpc obj = MovingObjectScene.Instantiate<WaldoNpc>();
            obj.RandomizeSprite(currentWaldo);
            obj.TryFlip(false);

            float x = (float)GD.RandRange(50, screenSize.X - 50);
            float y = (float)GD.RandRange(minY, maxY);
            obj.Position = new Vector2(x, y);

            float ratio = Mathf.InverseLerp(minY, maxY, y);
            float scale = Mathf.Lerp(0.6f, 1.0f, ratio);
            float speedMultiplier = Mathf.Lerp(0.5f, 1.0f, ratio);

            obj.Scale = new Vector2(scale, scale);
            obj.Speed = (float)(GD.RandRange(60, 150) * speedMultiplier);
            obj.MoveDistance = (float)GD.RandRange(50, 200);

            ysort.AddChild(obj);
        }
    }


    private void Timeout() {
        GameController.Instance.FailGame(0);
    }

    public override void _ExitTree() {
        GameController.Instance.GameTimerTimeout -= Timeout;
    }
}
