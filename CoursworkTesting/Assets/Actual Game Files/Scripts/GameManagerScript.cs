using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameSettingsProfile gameSettingsProfile;
    private string _gameSettingsDirectory;

    [SerializeField] private int[] killsToProceed;
    [SerializeField] private GameObject[] doorsToProceed;


    private int _numberOfKills;
    private static readonly int GoDown = Animator.StringToHash("GoDown");

    private int _currentRoom;

    public int NumberOfKills
    {
        get => _numberOfKills;
        set { _numberOfKills = value; UpdateDoors(); }
    }

    private void Awake()
    {
        _gameSettingsDirectory = Application.dataPath + @"\Settings\GameSettings.json";
        gameSettingsProfile = JsonUtility.FromJson<GameSettingsProfile>(File.ReadAllText(_gameSettingsDirectory));
    }

    private void UpdateDoors()
    {
        Debug.Log("Yes");
        if (_numberOfKills != killsToProceed[_currentRoom]) return;
        doorsToProceed[_currentRoom].GetComponent<Animator>().SetTrigger(GoDown);
        _currentRoom++;
    }
}
