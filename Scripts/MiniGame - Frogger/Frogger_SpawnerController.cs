using Godot;
using Godot.Collections;
using System;

public partial class Frogger_SpawnerController : Node2D
{
    [Export] public int ScreenWidth = 320;
    [Export] public int ScreenHeight = 180;
    [Export] public int MarginTop = 20;
    [Export] public int MarginBottom = 20;
    [Export] public int MarginLeft = 20;
    [Export] public int MarginRight = 42;

    public int LaneHeight;

    [Export] PackedScene FroggerSpawnerPrefab;

    // Objective: Create Spawners to map to Frogger size.
    // Lane Layout:
    // == Finish ==
    // Lane 1 - Random of {LeftSpawner, RightSpawner, Empty Space}
    // Lane 2 - "
    // Lane 3 - " 
    // Lane 4 - "
    // == Start ==
    public override void _Ready()
    {
        // Calculate Allotment for each lane
        LaneHeight = (ScreenHeight - MarginTop - MarginBottom) / 6;
        int CurrentLaneY = MarginTop + LaneHeight + LaneHeight / 2;

        // Randomize Lane Layout
        Array<bool> Spawn = new Array<bool> {true, true, true, false};
        Spawn.Shuffle();

        // Instantiate Lanes
        for (int i = 0; i < Spawn.Count; i++)
        {
            if (Spawn[i] || GameController.Instance.GameRound > 2)
            {
                // Coin flip for spawner side
                bool LeftSpawner = GD.Randf() < 0.5;

                // Instantiate Spawner
                Node2D Spawner = FroggerSpawnerPrefab.Instantiate<Node2D>();
                AddChild(Spawner);
                ((Frogger_Spawner)Spawner).TargetDirection_Right = LeftSpawner;

                // Adjust X position for side and margin
                // Adjust Y position for current lane
                Spawner.Position = new Vector2(
                    LeftSpawner ? -MarginRight : ScreenWidth + MarginLeft,
                    CurrentLaneY
                );
            }

            // Increment Lane Y
            CurrentLaneY += LaneHeight;
        }
    }

}
