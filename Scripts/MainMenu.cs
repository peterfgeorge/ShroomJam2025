using Godot;
using System;

public partial class MainMenu : Node2D
{
    public void Menu_StartGame()
    {
        GameController.Instance.StartGame();
    }
}
