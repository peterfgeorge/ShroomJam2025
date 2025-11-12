using Godot;
using System;
using System.Collections.Generic;

public partial class PterodactylSpawner : Node2D
{
    [Export] public PackedScene PterodactylScene;
    [Export] public int PoolSize = 10;

    [Export] public float MinSpawnInterval = 1.5f;
    [Export] public float MaxSpawnInterval = 3.5f;

    [Export] public float MinHeight = 50f;
    [Export] public float MaxHeight = 200f;
    [Export] EggPool eggPool;

    private List<Area2D> pteroPool = new();
    private Timer spawnTimer;
    private Random random = new();

    public override void _Ready()
    {
        spawnTimer = GetNode<Timer>("Timer");
        spawnTimer.Timeout += OnSpawnTimerTimeout;

        // Create object pool
        for (int i = 0; i < PoolSize; i++)
        {
            Area2D pterodactyl = (Area2D)PterodactylScene.Instantiate();
            pterodactyl.Visible = false;
            AddChild(pterodactyl);
            pteroPool.Add(pterodactyl);
        }

        SetRandomSpawnTime();
    }

    private void SetRandomSpawnTime()
    {
        spawnTimer.WaitTime = (float)(MinSpawnInterval + random.NextDouble() * (MaxSpawnInterval - MinSpawnInterval));
        spawnTimer.Start();
    }

    private void OnSpawnTimerTimeout()
    {
        SpawnPterodactyl();
        SetRandomSpawnTime();
    }

    private void SpawnPterodactyl()
    {
        Area2D pteroNode = GetInactivePterodactyl();
        if (pteroNode == null)
            return;

        // Get the Pterodactyl script attached to this node
        Pterodactyl ptero = pteroNode as Pterodactyl;
        if (ptero == null)
            return; // safety check

        float screenWidth = GetViewportRect().Size.X;
        bool spawnLeft = random.Next(0, 2) == 0;
        float x = spawnLeft ? 0 : screenWidth;
        float y = (float)(MinHeight + random.NextDouble() * (MaxHeight - MinHeight));

        ptero.Position = new Vector2(x, y);
        ptero.Visible = true;

        if (IsOverlapping(ptero))
        {
            GD.Print("Skipped spawn — overlapping another ptero");
            ptero.Visible = false;
            return;
        }

        // Assign the egg pool
        ptero.eggPool = eggPool;

        // Call existing initialization method
        ptero.Initialize(spawnLeft ? Vector2.Right : Vector2.Left);
    }

    private Area2D GetInactivePterodactyl() {
        foreach (var p in pteroPool) {
            if (!p.Visible)
                return p;
        }
        return null;
    }
    
    private bool IsOverlapping(Area2D ptero)
    {
        var spaceState = GetWorld2D().DirectSpaceState;

        // Get the shape resource from the CollisionShape2D
        var shape = ptero.GetNode<CollisionShape2D>("CollisionShape2D").Shape;

        // Create transform for the test position
        var transform = ptero.GlobalTransform;

        var shapeParams = new PhysicsShapeQueryParameters2D
        {
            Shape = shape,
            Transform = transform,
            CollisionMask = ptero.CollisionMask,
            CollideWithAreas = true,
            CollideWithBodies = true
        };

        var results = spaceState.IntersectShape(shapeParams, 1);
        return results.Count > 0;
    }
}
