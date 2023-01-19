using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public Transform PlayerManager, BulletManager, MeteorManager, FragmentManager, MapManager;

    // 게임오버 변수
    [SerializeField]
    private bool isGameOver = false;
    // 게임시작 변수
    [SerializeField]
    private bool isStart = false;

    // getter
    public bool getGameOver() { return isGameOver; }
    public bool getIsStart() { return isStart; }

    // setter
    public void setGameOver(bool flag) { isGameOver = flag; }
    public void setIsStart(bool flag) { isStart = flag;}

    private void Start()
    {

        PlayerManager= transform.GetChild(0);
        BulletManager= transform.GetChild(1);
        MeteorManager= transform.GetChild(2);
        FragmentManager= transform.GetChild(3);
        MapManager= transform.GetChild(4);
    }

    public void GameStart()
    {
        if (isStart == true) return; // 게임 시작한 상태면 실행X
        isStart = true; // 게임 시작 상태로 변경

        // 게임시작 UI 및 UX, GameRestart 로 변경
        GameRestart();

        // 게임시작 브금
        SoundManager.Instance.playBGM("브금"); // 브금 재생
        SoundManager.Instance.BGM_volumeControl(0.2f); // 브금 소리
    }

    public void GameOver()
    {
        isGameOver = true;
        isStart = false;

        UIManager.Instance.UI_GameOver();

        // 게임브금 중단
        SoundManager.Instance.pauseBGM();
    }

    public void GameRestart()
    {
        isGameOver = false;
        isStart = true;

        UIManager.Instance.UI_GameStart();
//<<<<<<< HEAD

//        GameObject player1 = PlayerManager.transform.GetChild(0).gameObject;
//        GameObject player2 = PlayerManager.transform.GetChild(1).gameObject;
//=======
        
        GameObject player1 = PlayerManager.GetChild(0).gameObject;
        GameObject player2 = PlayerManager.GetChild(1).gameObject;
//>>>>>>> Dev_LSY2

        player1.GetComponent<PlayerScript>().playerInit();
        player2.GetComponent<PlayerScript>().playerInit();

//<<<<<<< HEAD
        player1.transform.localPosition = new Vector3(Camera.main.transform.localPosition.x+15f, 5f);
        player2.transform.localPosition = new Vector3(Camera.main.transform.localPosition.x-15f, 5f,0);

        LightHouse.Instance.set_Durability();

        // 조각 랜덤위치 생성
        StartCoroutine(FragmentManager.GetComponent<FragmentManager>().randomCreateFragment());
//=======
        //player1.transform.localPosition = new Vector3(-5f, 5f,0);
        //player2.transform.localPosition = new Vector3(5f, 5f,0);
//>>>>>>> Dev_LSY2

        // UI
        GameObject.Find("Attacker/HP").GetComponent<Slider>().value = 1;
        GameObject.Find("Collecter/HP").GetComponent<Slider>().value = 1;
        GameObject.Find("LightHouseHP/HP").GetComponent<Slider>().value = 1;

        UIManager.Instance.UI_changeBullet(); // 총알 개수UI 변경
        UIManager.Instance.UI_changeFragment(); // 조각 개수UI 변경
    }

    public void gameOverCheck()
    {
        // 플레이어 2명 다 죽어있으면 게임오버
        if(PlayerManager.GetChild(0).GetComponent<PlayerScript>().isPlayerDead()
            && PlayerManager.GetChild(1).GetComponent<PlayerScript>().isPlayerDead())
        {
            GameOver();
        }


    }
}
