using Godot;
using System;

public partial class MiniGameWheresWaldo : Control {
    [Export] public PackedScene MovingObjectScene;
    [Export] public int ObjectCount = 50;

    public override void _Ready() {
        GameController.Instance.GameTimerTimeout += Timeout;
        SpawnObjects();
    }

    private void SpawnObjects() {
        var screenSize = GetViewportRect().Size;

        for (int i = 0; i < ObjectCount; i++) {
            var obj = MovingObjectScene.Instantiate<WaldoNpc>();

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
