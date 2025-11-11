using Godot;
using System;
using System.Threading.Tasks;

public partial class ControlsPopup : Control
{
    public override void _Ready()
    {
        if (GameController.Instance.GameRound > 0)
        {
            this.Hide();
        }
        else
        {
            PopupFadeController();
        }
    }

    public async void PopupFadeController()
    {
        await FadeIn(this, 0.25f);
        await ToSignal(GetTree().CreateTimer(1f), "timeout");
        await FadeOut(this, 1.25f);
        await FadeIn(this, 0.25f);
        await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
        await FadeOut(this, 0.25f);
    }

	public async Task FadeOut(CanvasItem canvas, float duration = 1f) {
		double counter = 0;
		Color og = canvas.Modulate;
		Color startColor = new(og.R, og.G, og.B, 1f);
		Color endColor = new(og.R, og.G, og.B, 0f);

		while (counter < duration) {
			Color lerp = startColor.Lerp(endColor, (float)counter / duration);
			counter += GetProcessDeltaTime();
			canvas.Modulate = lerp;
			await ToSignal(GetTree(), "process_frame");
		}

		canvas.Modulate = endColor;
	}

	public async Task FadeIn(CanvasItem canvas, float duration = 1f) {
		double counter = 0;
		Color og = canvas.Modulate;
		Color startColor = new(og.R, og.G, og.B, 0f);
		Color endColor = new(og.R, og.G, og.B, 1f);

		while (counter < duration) {
			Color lerp = startColor.Lerp(endColor, (float)counter / duration);
			counter += GetProcessDeltaTime();
			canvas.Modulate = lerp;
			await ToSignal(GetTree(), "process_frame");
		}

		canvas.Modulate = endColor;
	}

}
