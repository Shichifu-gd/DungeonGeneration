using UnityEngine.UI;
using UnityEngine;
using System.IO;

public class GenerationManagement : MonoBehaviour
{
    public LevelRestart levelRestart;
    public ListOfRoomsThatAppeared listOfRoomsThatAppeared;
    public RoomTemplates roomTemplates;
    public SpawnRoom spawnRoom;

    public Text TextRestartTime;
    public Text TextMaxRoom;
    public Text TextMinRoom;
    public Text TextTime;
    public Text TextWaitTime;

    public float RestartTime { get; set; }
    public int MaxRoom { get; set; }
    public int MinRoom { get; set; }
    public float Time { get; set; }
    public float WaitTime { get; set; }

    private void Awake()
    {
        if (File.Exists(Application.dataPath + "/data/SaveSettings.dt")) LoadingSettings();
        levelRestart.EndTime = RestartTime;
        listOfRoomsThatAppeared.MaximumNumberOfRooms = MaxRoom;
        listOfRoomsThatAppeared.MinimumNumberOfRooms = MinRoom;
        roomTemplates.WaitingForTimeToExit = Time;
        spawnRoom.ExtraTime = WaitTime;
    }

    public void OnPauseSpawn()
    {
        spawnRoom.PauseSpawnRoom();
        levelRestart.PauseLevelRestart();
    }

    private void SaveSettings()
    {
        SaveSettingsSpawn.SaveLocalizationTheGame(this);
    }

    private void LoadingSettings()
    {
        string[] dataSettings = SaveSettingsSpawn.LoadLocalizationTheGame();
        RestartTime = float.Parse(dataSettings[0]);
        MaxRoom = int.Parse(dataSettings[1]);
        MinRoom = int.Parse(dataSettings[2]);
        Time = float.Parse(dataSettings[3]);
        WaitTime = float.Parse(dataSettings[4]);
        UpdateText();
    }

    private void UpdateText()
    {
        SettingRestartTime("");
        SettingMaxRoom("");
        SettingMinRoom("");
        SettingTime("");
        SettingWaitTime("");
    }

    #region Settings

    public void SettingRestartTime(string operatorForSettin)
    {
        if (operatorForSettin == "+") RestartTime += 1;
        if (operatorForSettin == "-") RestartTime -= 1;
        TextRestartTime.text = RestartTime.ToString();
        SaveSettings();
    }

    public void SettingMaxRoom(string operatorForSettin)
    {
        if (operatorForSettin == "+") MaxRoom += 1;
        if (operatorForSettin == "-") MaxRoom -= 1;
        TextMaxRoom.text = MaxRoom.ToString();
        SaveSettings();
    }

    public void SettingMinRoom(string operatorForSettin)
    {
        if (operatorForSettin == "+") MinRoom += 1;
        if (operatorForSettin == "-") MinRoom -= 1;
        TextMinRoom.text = MinRoom.ToString();
        SaveSettings();
    }

    public void SettingTime(string operatorForSettin)
    {
        if (operatorForSettin == "+") Time += 0.1f;
        if (operatorForSettin == "-") Time -= 0.1f;
        TextTime.text = Time.ToString();
        SaveSettings();
    }

    public void SettingWaitTime(string operatorForSettin)
    {
        if (operatorForSettin == "+") WaitTime += 0.1f;
        if (operatorForSettin == "-") WaitTime -= 0.1f;
        TextWaitTime.text = WaitTime.ToString();
        SaveSettings();
    }
    #endregion

    private void OnApplicationQuit()
    {
        SaveSettings();
    }
}