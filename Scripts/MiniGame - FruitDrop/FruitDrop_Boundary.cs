using Godot;
using System;

public partial class FruitDrop_Boundary : Area2D
{
    [Export] PlayerController_FruitDrop Player;

    public void Collision(Area2D s)
    {
        // Call Player Miss
        Player.Miss(s);
    } 
    
    public override void _Ready()
    {
        // Bind Collision Event Handler
        AreaEntered += Collision;
    }
}
