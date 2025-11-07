using Godot;
using System;

public partial class MiniGameWheresWaldo : Node2D {
    [Export] public PackedScene MovingObjectScene;
    [Export] public int ObjectCount = 50;

    public override void _Ready()
    {
        SpawnObjects();
    }

    private void SpawnObjects()
    {
        var screenSize = GetViewportRect().Size;

        for (int i = 0; i < ObjectCount; i++)
        {
            var obj = MovingObjectScene.Instantiate<WaldoNpc>();

            // Random position on screen
            float x = (float)GD.RandRange(50, screenSize.X - 50);
            float y = (float)GD.RandRange(50, screenSize.Y - 50);
            obj.Position = new Vector2(x, y);

            // Random speed and range variation
            obj.Speed = (float)GD.RandRange(60, 150);
            obj.MoveDistance = (float)GD.RandRange(50, 200);

            AddChild(obj);
        }
    }
}
