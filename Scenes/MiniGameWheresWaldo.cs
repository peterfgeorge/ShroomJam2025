using Godot;
using Godot.Collections;
using System;

public partial class MiniGameWheresWaldo : Control {
    [Export] public PackedScene MovingObjectScene;
    [Export] public int ObjectCount = 50;
    [Export] SpriteFrames sprites;
    [Export] TextureRect display;

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
        SpawnObjects();
        display.Texture = sprites.GetFrameTexture(currentWaldo, 0);
    }

    private void SpawnObjects() {
        currentWaldo = characters.PickRandom();

        var screenSize = GetViewportRect().Size;

        WaldoNpc waldo = MovingObjectScene.Instantiate<WaldoNpc>();
        waldo.SetSprite(currentWaldo);

        // Random position on screen
        float x1 = (float)GD.RandRange(50, screenSize.X - 50);
        float y1 = (float)GD.RandRange(70, screenSize.Y - 30);
        waldo.Position = new Vector2(x1, y1);

        // Random speed and range variation
        waldo.Speed = (float)GD.RandRange(60, 150);
        waldo.MoveDistance = (float)GD.RandRange(50, 100);

        AddChild(waldo);

        for (int i = 0; i < ObjectCount; i++) {
            WaldoNpc obj = MovingObjectScene.Instantiate<WaldoNpc>();
            obj.RandomizeSprite(currentWaldo);

            // Random position on screen
            float x = (float)GD.RandRange(50, screenSize.X - 50);
            float y = (float)GD.RandRange(70, screenSize.Y - 30);
            obj.Position = new Vector2(x, y);

            // Random speed and range variation
            obj.Speed = (float)GD.RandRange(60, 150);
            obj.MoveDistance = (float)GD.RandRange(50, 200);

            AddChild(obj);
        }
    }

    private void Timeout() {
        GameController.Instance.FailGame(0);
    }

    public override void _ExitTree() {
        GameController.Instance.GameTimerTimeout -= Timeout;
    }
}
