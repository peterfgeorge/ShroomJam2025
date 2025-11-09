using Godot;
using System;

public partial class FruitDrop_Spawner : Node2D
{
    [Export] public PackedScene FruitPrefab;
    [Export] public PackedScene BombPrefab;
    
    [Export] float SpawnXScale = 250f;
    [Export] float SpawnDelay = 0.2f;
    [Export] float SpawnBombChance = 0.3f;
    
    Area2D[] FruitPool;
    int poolIndex = 0;

    public override void _Ready()
    {
        // Update Config for Game Round
        SpawnDelay -= GameController.Instance.GameRound * 0.02f;

        // Instantiate Pool for Spawning
        FruitPool = new Area2D[15];
        for (int i = 0; i < FruitPool.Length; i++)
        {
            Area2D Object;
            if (GD.Randf() < SpawnBombChance)
            {
                Object = BombPrefab.Instantiate<Area2D>();
            }
            else
            {
                Object = FruitPrefab.Instantiate<Area2D>();
            }

            AddChild(Object);
            Object.Hide();
            Object.Monitorable = false;

            FruitPool[i] = Object;
        }

        // Begin Spawn loop
        SpawnLoop();
    }

    private async void SpawnLoop()
    {
        // Grab next object in object pool
        Node2D target = FruitPool[poolIndex++];
        if (poolIndex >= FruitPool.Length)
            poolIndex = 0;

        // Reset and make visible
        target.Position = new Vector2(
            (0.5f - GD.Randf()) * SpawnXScale,
            Position.Y
        );
        target.Show();
        ((Area2D) target).Monitorable = true;

        // Wait for spawn delay plus random offset
        await ToSignal(GetTree().CreateTimer(SpawnDelay + GD.Randf()), "timeout");

        // Recurse
        SpawnLoop();
    }

}
