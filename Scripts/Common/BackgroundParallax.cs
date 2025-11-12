using Godot;
using System;

public partial class BackgroundParallax : Control
{
    [Export] public Godot.Collections.Array<Control> ManagedNodes;

    [Export] float ParallaxSpeedX = -1;
    [Export] float ParallaxSpeedY = 0;

    [Export] float ParallaxOffsetX = 0;
    [Export] float ParallaxOffsetY = 0;

    [Export] float ParallaxBound = 258;

    public override void _Process(double delta)
    {
        foreach (Control node in ManagedNodes)
        {
            if (node.Position.X <= ParallaxOffsetX - ParallaxBound)
            {
                node.Position += new Vector2(
                    ManagedNodes.Count * ParallaxBound, 
                    0
                );
            }
            
            node.Position += new Vector2(
                ParallaxSpeedX, 
                ParallaxSpeedY
            );
            
        }
    }
}
