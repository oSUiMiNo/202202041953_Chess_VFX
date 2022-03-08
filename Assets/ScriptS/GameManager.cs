using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        Component(); 
    }

    public float Timer;
    private void Update()
    {
        Timer += Time.deltaTime;
        if(!GameObject.Find("White_King0") || !GameObject.Find("Black_King0"))
        {
            Invoke("CHECKMATE", 1.0f);
        }
    }

    //インスタンスたちを取得
    Text Inform;
    Controller controller;
    ChangeScene changeScene;
    ChessAgent chessAgent;
    public void Component()
    {
        Inform = GameObject.Find("Inform").GetComponent<Text>();
        controller = GameObject.Find("Controller").GetComponent<Controller>();
        changeScene = this.gameObject.GetComponent<ChangeScene>();
        chessAgent = GameObject.Find("ChessAgent").GetComponent<ChessAgent>();
    }


    //チェックメイト時の処理
    public void CHECKMATE()
    {
        controller.enabled = false;
        chessAgent.enabled = false;
        Inform.text = "CHECKMATE";
        StartCoroutine(changeScene.Scene("Menu", 5.0f));
    }

}

