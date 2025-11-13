using Godot;
using System;
using System.Collections.Generic;

public partial class EggPool : Node2D
{
    public static EggPool Instance { get; private set; }

    [Export] public PackedScene EggPrefab;
    [Export] public int PoolSize = 15;

    private List<Area2D> pool = new();

    public override void _Ready()
    {
        Instance = this;

        for (int i = 0; i < PoolSize; i++)
        {
            FruitDrop_Fruit egg = EggPrefab.Instantiate<FruitDrop_Fruit>();
            egg.Visible = false;
            egg.SetDeferred("monitorable", false);
            AddChild(egg);
            pool.Add(egg);
        }
    }

    public Area2D SpawnEgg(Vector2 spawnPosition)
    {
        if(!Name.ToString().Contains("Evil")) {
             foreach (var eggNode in pool) {
                // Cast to your custom egg type (if you have one)
                if (eggNode.Visible) {
                    // Check if Y is below 90
                    if (eggNode.Position.Y < 90f) {
                        // Check horizontal distance
                        float distanceX = MathF.Abs(eggNode.Position.X - spawnPosition.X);
                        if (distanceX > 75f) {
                            GD.Print("Skipped spawn — too far from another active egg below Y=90");
                            return null; // cancel spawn
                        }
                    }
                }
            }
        }
        
        FruitDrop_Fruit egg = GetInactiveEgg() as FruitDrop_Fruit;
        if (egg == null)
            return null;

        egg.ResetFruit(spawnPosition);

        return egg;
    }

    private Area2D GetInactiveEgg()
    {
        foreach (var egg in pool)
        {
            if (!egg.Visible)
                return egg;
        }
        return null;
    }
}
