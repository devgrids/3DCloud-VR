using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    login,
    lobbyDocente,
    lobbyEstudiante,
}

public class GameManager : MonoBehaviour
{
    public static GameManager sharedInstance;
    public GameState currentGameState = GameState.login;

    [SerializeField] Canvas loginCanvas = null;
    [SerializeField] Canvas lobbyDocenteCanvas = null;
    [SerializeField] Canvas lobbyEstudianteCanvas = null;

    private void Awake()
    {
        sharedInstance = this;
    }

    private void Start()
    {
        SetGameState(GameState.login);
    }

    private void Update()
    {

    }

    public void SetGameState(GameState newGameState)
    {
        if (newGameState == GameState.login)
        {
            loginCanvas.enabled = true;
            lobbyDocenteCanvas.enabled = false;
            lobbyEstudianteCanvas.enabled = false;
        }
        else if (newGameState == GameState.lobbyDocente)
        {
            loginCanvas.enabled = false;
            lobbyDocenteCanvas.enabled = true;
            lobbyEstudianteCanvas.enabled = false;
        }
        else if (newGameState == GameState.lobbyEstudiante)
        {
            loginCanvas.enabled = false;
            lobbyDocenteCanvas.enabled = false;
            lobbyEstudianteCanvas.enabled = true;
        }

        this.currentGameState = newGameState;
    }
    
}