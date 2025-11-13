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
    [Export] Control achievementScreen;
    [Export] TextureRect score100;
    [Export] TextureRect score200;
    [Export] TextureRect score300;
    [Export] TextureRect score400;

    public override void _Ready() {
        // Update Score Labels
        HighScoreLabel.Text = SaveData.Instance.data["TOTAL_SCORE"].ToString();
        RecentScoreLabel.Text = SaveData.Instance.data["RECENT_SCORE"].ToString();

        PopulateAchievements();

        if (SaveData.Instance.data["TOTAL_SCORE"] >= 400) {
            titleImage.Texture = GD.Load<Texture2D>("res://Art/title_old.png");
            titleImage.CustomMinimumSize = new Vector2(200, titleImage.CustomMinimumSize.Y);
        }

        if (!GameController.Instance.gameStarted) {
            // Hide recent score - game not played yet
            ((Control)RecentScoreLabel.GetParent()).Hide();

            if (SaveData.Instance.data["TOTAL_SCORE"] < 400) {
                titleImage.Texture = GD.Load<Texture2D>("res://Art/title.png");
                GetTree().CreateTimer(23f).Timeout += Wink;
            }

            // Play Intro Sequence
            PlayIntro();
            player.Finished += PlayIntroLoop;
        } else {
            PlayIntroLoopNoSignal();
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

    private void ShowAchievements() {
        achievementScreen.Show();
        title.Hide();
    }

    private void HideAchievements() {
        achievementScreen.Hide();
        title.Show();
    }

    private void PopulateAchievements() {
        if (SaveData.Instance.data["TOTAL_SCORE"] < 400) {
            GD.Print("Scoreless");
            score400.Modulate = new Color(1, 1, 1, 0.5f);
        }

        if (SaveData.Instance.data["TOTAL_SCORE"] < 300) {
            score300.Modulate = new Color(1, 1, 1, 0.5f);
        }

        if (SaveData.Instance.data["TOTAL_SCORE"] < 200) {
            score200.Modulate = new Color(1, 1, 1, 0.5f);
        }

        if (SaveData.Instance.data["TOTAL_SCORE"] < 100) {
            score100.Modulate = new Color(1, 1, 1, 0.5f);
        }
    }

    private void PlayIntroLoop() {
        player.Finished -= PlayIntroLoop;
        player.Stream = GD.Load<AudioStream>("res://Audio/Music/CartoonIntroLoop.mp3");
        player.Play();
    }

    private void PlayIntroLoopNoSignal() {
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
