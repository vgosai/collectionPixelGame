using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;  } //singleton rest are scriptable objects that change per level
    [SerializeField]
    private GameState startingState;
    public GameState Gamestate { get; private set; }
    public LevelManager levelManager;
    //public PlayerManager playerManager;
    public UIManager uiManager;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        Gamestate = Instantiate(startingState);
        levelManager.GameState = Gamestate;
        //playerManager.GameState = Gamestate;
    }
}
