using Godot;
using System;

public partial class JJManager : Node {
    [Export] JJPlayerController player;
    [Export] ObstacleSpawner spawner;

    public override void _Ready() {

    }

    public override void _Process(double delta) {
        if (Input.IsActionJustPressed("Up")) {
            player.enabled = true;
            spawner.enabled = true;
        }
    }



}
