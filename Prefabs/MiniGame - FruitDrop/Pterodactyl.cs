using Godot;
using System;

public partial class Pterodactyl : Area2D
{
    private Vector2 direction = Vector2.Left;
    [Export] float speed = 200f;
    [Export] AnimatedSprite2D anim;
    [Export] public EggPool eggPool;
    private float[] dropCooldown = [2f, 1.6f, 1.35f, 1.1f, .8f];
    private float dropTimer = 0f;
    public bool Active { get; private set; } = false;
    private GameController gameController => GameController.Instance;
    private int index;

    public CollisionShape2D CollisionShape => GetNode<CollisionShape2D>("CollisionShape2D");

    public void Initialize(Vector2 dir)
    {
        direction = dir;
        Scale = new Vector2(dir == Vector2.Left ? 1 : -1, 1);
        if(!Name.ToString().Contains("Red")) {
            dropTimer = GD.Randf() * 2f;
        } else {
            int round = gameController != null ? gameController.GameRound : 0;
            int index = Mathf.Clamp(round, 0, dropCooldown.Length - 1);
            dropTimer = GD.Randf() * dropCooldown[index];
        }
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
                if(!Name.ToString().Contains("Red")) {
                    dropTimer = 2f + GD.Randf() * 2f; // random 2–4 sec cooldown
                } else {
                    GD.Print("Dropping egg from evil ptero");
                    dropTimer = .5f + GD.Randf() * dropCooldown[index];
                }
            }
        }

        // Hide offscreen
        if (Position.X < -100 || Position.X > GetViewportRect().Size.X + 100)
            Deactivate();
    }

    private void DropEgg() {
        if (eggPool != null && Active) {
            eggPool.SpawnEgg(GlobalPosition);
        }
    }
    
    
    public void Activate(Vector2 startPosition)
    {
        Position = startPosition;
        Visible = true;
        Monitoring = true; // enables Area2D collision checks
        Active = true;
    }

    public void Deactivate()
    {
        Visible = false;
        Monitoring = false; // disables Area2D collision checks
        Active = false;
    }
}