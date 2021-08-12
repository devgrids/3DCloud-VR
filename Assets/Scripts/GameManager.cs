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

    [SerializeField] GameObject loginCanvas = null;
    [SerializeField] GameObject lobbyDocenteCanvas = null;
    [SerializeField] GameObject lobbyEstudianteCanvas = null;

    private void Awake()
    {
        sharedInstance = this;
    }

    private void Start()
    {
        SetGameState(currentGameState);
    }

    private void Update()
    {

    }

    public void SetGameState(GameState newGameState)
    {
        if (newGameState == GameState.login)
        {
            loginCanvas.SetActive(true);
            lobbyDocenteCanvas.SetActive(false);
            lobbyEstudianteCanvas.SetActive(false);
        }
        else if (newGameState == GameState.lobbyDocente)
        {
            loginCanvas.SetActive(false);
            lobbyDocenteCanvas.SetActive(true);
            lobbyEstudianteCanvas.SetActive(false);
        }
        else if (newGameState == GameState.lobbyEstudiante)
        {
            loginCanvas.SetActive(false);
            lobbyDocenteCanvas.SetActive(false);
            lobbyEstudianteCanvas.SetActive(true);
        }

        this.currentGameState = newGameState;
    }
    
}