using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Initialize_Game();
    }


    public bool Turn_Black;
    public bool Turn_White;
    public void Initialize_Game()
    {
        Turn_Black = false;
        Turn_White = true;
    }




}

