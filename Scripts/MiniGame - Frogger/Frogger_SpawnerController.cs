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

    public float LaneHeight;

    [Export] PackedScene FroggerSpawnerPrefab;
    [Export] PackedScene FroggerObstaclePrefab;

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
        float CurrentLaneY = MarginTop + LaneHeight + LaneHeight / 2;

        // Randomize Lane Layout
        Array<bool> Spawn = new Array<bool> {true, true, true, false};
        Spawn.Shuffle();

        // Instantiate Lanes
        for (int i = 0; i < Spawn.Count; i++)
        {
            // Edge Spawning
            if (Spawn[i])
            {
                // Coin flip for spawner side
                bool LeftSpawner = GD.Randf() < 0.5;

                // Instantiate Spawner
                Node2D Spawner = FroggerSpawnerPrefab.Instantiate<Node2D>();
                ((Frogger_Spawner)Spawner).TargetDirection_Right = LeftSpawner;

                // Adjust X position for side and margin
                // Adjust Y position for current lane
                AddChild(Spawner);
                Spawner.Position = new Vector2(
                    LeftSpawner ? -MarginRight : ScreenWidth + MarginLeft,
                    CurrentLaneY
                );
            }
            // Static
            else
            {
                Array<bool> StaticLocations = new Array<bool> {true, true, true, true, true, false, false, false, false, false, false};
                StaticLocations.Shuffle();

                float CurrentGridX = MarginLeft + LaneHeight / 2;
                for (int j = 0; j < StaticLocations.Count; j++)
                {
                    if (StaticLocations[j])
                    {
                        Node2D Obstacle = FroggerObstaclePrefab.Instantiate<Node2D>();
                        ((Frogger_Obstacle) Obstacle).Static = true;

                        AddChild(Obstacle);
                        float centerX = (ScreenWidth + MarginLeft - MarginRight) / 2;
                        Obstacle.GlobalPosition = new Vector2(
                            centerX + (j-StaticLocations.Count/2) * LaneHeight,
                            CurrentLaneY
                        );
                    }

                    CurrentGridX += LaneHeight;
                }
            }

            // Increment Lane Y
            CurrentLaneY += LaneHeight;
        }
    }

}
