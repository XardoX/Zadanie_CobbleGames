using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField]
    private GameModel model;

    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private CameraController cameraController;

    public static PlayerController Player => Instance.playerController; 

    public static CameraController Camera => Instance.cameraController;

    public void StartGame()
    {

    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
