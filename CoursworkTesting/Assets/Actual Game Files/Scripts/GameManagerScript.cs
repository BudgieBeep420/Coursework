using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEditor.Rendering.PostProcessing;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameSettingsProfile gameSettingsProfile;
    private string _gameSettingsDirectory;

    [Header("This is cumulative btw")]
    [SerializeField] private int[] killsToProceed;
    [Space]
    [SerializeField] private GameObject[] doorsToProceed;
    [SerializeField] private bool disableDoorCheck;


    private int _numberOfKills;
    private static readonly int GoDown = Animator.StringToHash("GoDown");

    private int _currentRoom;

    public int NumberOfKills
    {
        get => _numberOfKills;
        set { _numberOfKills = value; if(!disableDoorCheck) UpdateDoors(); }
    }

    private void Awake()
    {
        _gameSettingsDirectory = Application.dataPath + @"\Settings\GameSettings.json";
        gameSettingsProfile = JsonUtility.FromJson<GameSettingsProfile>(File.ReadAllText(_gameSettingsDirectory));
    }

    private void UpdateDoors()
    {
        Debug.Log(_numberOfKills);
        if (_numberOfKills != killsToProceed[_currentRoom]) return;
        doorsToProceed[_currentRoom].GetComponent<Animator>().SetTrigger(GoDown);
        _currentRoom++;
    }
}
