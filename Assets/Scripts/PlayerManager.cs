/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private string spawnTag;
    public PlayerStats PlayerStats { get; private set; }
    public GameState GameState { get; set; }
    void OnEnable()
    {
        LevelEvents.levelLoaded += SpawnPlayer;
        PlayerManagerStats = Instantiate(startingPlayerStats);
    }

    protected void SpawnPlayer(Transform defaultSpawnTransform)
    {
        if (GameState.playerSpawnLocation != "")
        {
            GameObject[] spawns = GameObject.FindGameObjectsWithTag(spawnTag);
            bool foundSpawn = false;
            foreach (GameObject spawn in spawns)
            {
                if (spawn.name == GameState.playerSpawnLocation)
                {
                    foundSpawn = true;
                    ActivePlayer = Instantiate(playerPrefab, spawn.transform.position, Quaternion.identity);
                    break;
                }
            }
            if (!foundSpawn)
            {
                throw new MissingReferenceException("Could not find the player spawn location with the name" + GameState.playerSpawnLocation);
            }
        }
        else
        {
            ActivePlayer = Instantiate(playerPrefab, defaultSpawnTransform.postion, Quaternion.identity);
            Debug.Log("Player spawned at default location: " + defaultSpawnTransform);
        }
        if (ActivePlayer)
        {
            PlayerEvents.onPlayerSpawned.Invoke(ActivePlayer.transform);
        }
        else
        {
            throw new MissingReferenceException("No ActivePlayer in PlayerManager. May have failed to spawn!");
        }
    }

    void OnDisable()
    {
        LevelEvents.levelLoaded -= SpawnPlayer;
    }
}*/

