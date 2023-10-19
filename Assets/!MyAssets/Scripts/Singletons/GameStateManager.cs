using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviorSingleton<GameStateManager>
{
    private GameStates currentGameState = GameStates.Init;
    public GameStates CurrentGameState { get { return currentGameState; } set { currentGameState = value; } }

    protected override void Awake()
    {
        base.Awake();

        //run the base awake method in the MonoBehaviorSingleton script
        //if we make it here, we are the only singleton and should not be destroyed
        DontDestroyOnLoad(gameObject);
    }

    public enum GameStates
    {
        Init,
        Worldhub,
        Loading,
        GeneratingDungeon,
        GameInProgress,
        Paused
    }
}
