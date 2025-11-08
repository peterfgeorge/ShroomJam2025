using Godot;
using System;

public partial class MainMenu : Node2D
{
    public override void _Ready()
    {
        ((Label)GetChild(1)).Text = SaveData.Instance.data["TOTAL_SCORE"].ToString();
    }

    public void Menu_StartGame()
    {
        GameController.Instance.StartGame();
    }
}
