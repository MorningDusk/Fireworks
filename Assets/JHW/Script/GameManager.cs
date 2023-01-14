using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : SingletonMonoBehaviour<GameManager>
{

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

    public void GameStart()
    {
        if (isStart == true) return; // 게임 시작한 상태면 실행X
        isStart = true; // 게임 시작 상태로 변경

        // 게임시작 UI 및 UX
        UIManager.Instance.UI_GameStart();

        // 게임시작 브금
        SoundManager.Instance.playBGM("브금"); // 브금 재생
        SoundManager.Instance.BGM_volumeControl(0.2f); // 브금 소리
    }

    public void GameOver()
    {
        isGameOver = true;
        isStart = false;
    }

    public void GameRestart()
    {
        isGameOver = false;
        isStart = true;

        UIManager.Instance.UI_GameStart();
    }
}
