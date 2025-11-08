using Godot;
using System;

public partial class MainMenu : Control {
    [Export] AudioStreamPlayer player;
    [Export] Label scoreLabel;

    public override void _Ready() {
        scoreLabel.Text = SaveData.Instance.data["TOTAL_SCORE"].ToString();
        player.Play();
    }

    public void Menu_StartGame() {
        GameController.Instance.StartGame();
    }

    public void Menu_QuitGame() {
        GetTree().Quit();
    }
}
