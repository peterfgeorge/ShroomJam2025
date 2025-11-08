using Godot;
using System;

public partial class TimerOverlay : Control
{
    Label TimerLabel;


    
    public override void _Ready() {
        TimerLabel = GetChild<Label>(0);
    }

    public override void _Process(double delta)
    {
        // Retrieve time
        String TimeLeft = GameController.Instance.GetGameTimer().ToString();
        int DecimalIndex = TimeLeft.IndexOf(".");

        if (DecimalIndex >= 0)
            TimerLabel.Text = TimeLeft.Substring(0, DecimalIndex+2);
        else
            TimerLabel.Text = TimeLeft;
    }

}
