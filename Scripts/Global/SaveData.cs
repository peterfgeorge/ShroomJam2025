using Godot;
using System;

enum SaveDataID 
{
    TOTAL_SCORE,
    RECENT_SCORE,
    FROGGER,
    JETPACK,
    WALDO
}

public partial class SaveData : Node
{
    public Godot.Collections.Dictionary<string, int> data;

    public static SaveData Instance { get; private set; }

    public static readonly String FilePath = "user://HighScores.save";

    public override void _Ready()
    {
        // Initialize Singleton
        Instance = this;

        // Load Data
        data = new Godot.Collections.Dictionary<string, int>();
        foreach (String name in Enum.GetNames(typeof(SaveDataID)))
        {
            data[name] = 0;
        }
        Load();
    }

    public void SetValue(String name, int value)
    {
        data[name] = value;
        Save();
    }

    public void SetHighValue(String name, int value)
    {
        if (value > data[name])
            data[name] = value;
        Save();
    }

    public void ResetScores()
    {
        foreach (String name in Enum.GetNames(typeof(SaveDataID)))
        {
            data[name] = 0;
        }
        Save();
    }
    
    public void Load()
    {
        // Check for Save File
        if (!FileAccess.FileExists(FilePath))
        {
            return;
        }

        // Read First Line of File
        var saveFile = FileAccess.Open(FilePath, FileAccess.ModeFlags.Read);
        String jsonString = saveFile.GetLine();

        // Parse to Json
        Json json = new Json();
        Error parseResult = json.Parse(jsonString);
        if (parseResult != Error.Ok)
        {
            GD.Print($"JSON Parse Error: {json.GetErrorMessage()} in {jsonString} at line {json.GetErrorLine()}");
            
            return;
        }

        foreach (var element in (Godot.Collections.Dictionary<string, int>)json.Data)
        {
            data[element.Key] = element.Value;
        }
    }

    public void Save()
    {
        // Open File
        var saveFile = FileAccess.Open(FilePath, FileAccess.ModeFlags.Write);

        // Save Json as String
        String jsonString = Json.Stringify(data);
        saveFile.StoreLine(jsonString);

        saveFile.Flush();
    }

}
