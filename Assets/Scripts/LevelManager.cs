using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "LevelManager", menuName = "Manager/LevelManager", order = 1)]
public class LevelManager : ScriptableObject
{
    public GameState GameState { get; set; }
    void OnEnable()
    {
        LevelEvents.levelExit += OnLevelExit;
    }

    private void OnLevelExit(SceneAsset nextLevel, string playerSpawnTransformName)
    {
        GameState.playerSpawnLocation = playerSpawnTransformName;
        SceneManager.LoadScene(nextLevel.name, LoadSceneMode.Single);
    }
    void OnDisable()
    {
        LevelEvents.levelExit -= OnLevelExit;
    }
}
