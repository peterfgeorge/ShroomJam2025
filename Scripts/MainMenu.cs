using Godot;
using System;

public partial class MainMenu : Control {
    [Export] AudioStreamPlayer player;
    [Export] Label scoreLabel;

    [Export] CanvasItem logo;
    [Export] CanvasItem credits;
    [Export] CanvasItem title;
    [Export] CanvasItem buttons;

    public override void _Ready() {
        scoreLabel.Text = SaveData.Instance.data["TOTAL_SCORE"].ToString();
        if (!GameController.Instance.gameStarted) {
            player.Play();
            PlayIntro();
        }
    }

    public void Menu_StartGame() {
        GameController.Instance.StartGame();
    }

    public void Menu_QuitGame() {
        GetTree().Quit();
    }

    private async void PlayIntro() {
        title.Modulate = new Color(1, 1, 1, 0);
        buttons.Hide();

        await FadeController.Instance.FadeIn(logo, 2f);
        await ToSignal(GetTree().CreateTimer(2f), "timeout");
        await FadeController.Instance.FadeOut(logo);
        await ToSignal(GetTree().CreateTimer(1f), "timeout");

        await FadeController.Instance.FadeIn(credits, 2f);
        await ToSignal(GetTree().CreateTimer(2f), "timeout");
        await FadeController.Instance.FadeOut(credits);
        await ToSignal(GetTree().CreateTimer(1f), "timeout");

        await FadeController.Instance.FadeIn(title, 2f);
        buttons.Show();
    }
}
