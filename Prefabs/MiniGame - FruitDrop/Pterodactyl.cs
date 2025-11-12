using Godot;
using System;

public partial class Pterodactyl : Area2D
{
    private Vector2 direction = Vector2.Left;
    [Export] float speed = 200f;
    [Export] AnimatedSprite2D anim;
    [Export] public EggPool eggPool;
    private float dropCooldown = 2f;
    private float dropTimer = 0f;

    public CollisionShape2D CollisionShape => GetNode<CollisionShape2D>("CollisionShape2D");

    public void Initialize(Vector2 dir)
    {
        direction = dir;
        Scale = new Vector2(dir == Vector2.Left ? 1 : -1, 1);
        dropTimer = GD.Randf() * 2f;
    }

    public override void _Process(double delta) {
        if (!IsInsideTree()) return;

        // Move
        Position += direction * speed * (float)delta;

        anim.FlipH = true;

        // Only drop eggs if within X range
        float x = GlobalPosition.X; // world X
        if (x >= 25f && x <= 275f) {
            dropTimer -= (float)delta;
            if (dropTimer <= 0f) {
                DropEgg();
                dropTimer = 2f + GD.Randf() * 2f; // random 2–4 sec cooldown
            }
        }

        // Hide offscreen
        if (Position.X < -100 || Position.X > GetViewportRect().Size.X + 100)
            Visible = false;
    }
    
    private void DropEgg()
    {
        if (eggPool != null)
        {
            eggPool.SpawnEgg(GlobalPosition);
        }
    }
}
