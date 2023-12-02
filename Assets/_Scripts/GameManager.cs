using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] public GameState State { get; private set; }


    [SerializeField] private GameObject _canvasesContainer;
    [SerializeField] private Menu _mainMenuPrefab;
    [SerializeField] private Win _winMenuPrefab;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private PlayerController _playerController;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        UpdateGameState(GameState.MainMenu);
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.MainMenu:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                Instantiate(_mainMenuPrefab, _canvasesContainer.transform);
                break;
            case GameState.GenerateLevel:
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                _playerController.enabled = true;
                _cameraController.enabled = true;
                break;
            case GameState.Win:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                _playerController.enabled = false;
                _cameraController.enabled = false;
                Instantiate(_winMenuPrefab, _canvasesContainer.transform);
                break;
            case GameState.Lose:
                Cursor.visible = true;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

    }

    public enum GameState
    {
        MainMenu,
        GenerateLevel,
        Win,
        Lose
    }
}
