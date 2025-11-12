using Godot;
using System;

public partial class MainMenu : Control {
    [Export] AudioStreamPlayer player;
    [Export] Label RecentScoreLabel;
    [Export] Label HighScoreLabel;

    [Export] CanvasItem logo;
    [Export] CanvasItem credits;
    [Export] CanvasItem title;
    [Export] CanvasItem buttons;

    [Export] PackedScene noise;
    [Export] CanvasLayer crt;
    [Export] TextureRect titleImage;

    public override void _Ready() {
        // Update Score Labels
        HighScoreLabel.Text = SaveData.Instance.data["TOTAL_SCORE"].ToString();
        RecentScoreLabel.Text = SaveData.Instance.data["RECENT_SCORE"].ToString();

        if (SaveData.Instance.data["TOTAL_SCORE"] >= 500) {
            titleImage.Texture = GD.Load<Texture2D>("res://Art/title_old.png");
            titleImage.CustomMinimumSize = new Vector2(200, titleImage.CustomMinimumSize.Y);
        }

        if (!GameController.Instance.gameStarted) {
            // Hide recent score - game not played yet
            ((Control)RecentScoreLabel.GetParent()).Hide();

            // Play Intro Sequence
            PlayIntro();
            GetTree().CreateTimer(23f).Timeout += Wink;
            player.Finished += PlayIntroLoop;
        } else {
            PlayIntroLoop();
        }
    }

    private void Wink() {
        titleImage.Texture = GD.Load<Texture2D>("res://Art/title_wink.png");
    }


    public void Menu_StartGame() {
        GameController.Instance.StartGame();
    }

    public void Menu_QuitGame() {
        GetTree().Quit();
    }

    private async void PlayIntro() {
        ColorRect cover1 = CreateCover();
        ColorRect cover2 = CreateCover();
        crt.AddChild(cover1);
        AddChild(cover2);

        await ToSignal(GetTree().CreateTimer(1f), "timeout");
        await FadeController.Instance.FadeOut(cover1, 2f);
        Node n = noise.Instantiate();
        AddChild(n);
        await ToSignal(GetTree().CreateTimer(1f), "timeout");
        n.QueueFree();
        cover2.Hide();

        player.Play();
        title.Modulate = new Color(1, 1, 1, 0);
        buttons.Modulate = new Color(1, 1, 1, 0);

        await FadeController.Instance.FadeIn(logo, 2f);
        await ToSignal(GetTree().CreateTimer(2f), "timeout");
        await FadeController.Instance.FadeOut(logo);
        await ToSignal(GetTree().CreateTimer(1f), "timeout");

        await FadeController.Instance.FadeIn(credits, 2f);
        await ToSignal(GetTree().CreateTimer(2f), "timeout");
        await FadeController.Instance.FadeOut(credits);
        await ToSignal(GetTree().CreateTimer(1f), "timeout");

        await FadeController.Instance.FadeIn(title, 2f);
        buttons.Modulate = new Color(1, 1, 1, 1);
        cover1.QueueFree();
        cover2.QueueFree();
    }

    private void PlayIntroLoop() {
        player.Finished -= PlayIntroLoop;
        player.Stream = GD.Load<AudioStream>("res://Audio/Music/CartoonIntroLoop.mp3");
        player.Play();
    }

    private ColorRect CreateCover() {
        ColorRect rect = new();
        rect.Color = new Color(0, 0, 0, 1);
        rect.SetAnchorsPreset(LayoutPreset.FullRect);
        return rect;
    }
}
