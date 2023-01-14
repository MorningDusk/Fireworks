using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : SingletonMonoBehaviour<GameManager>
{

    // ���ӿ��� ����
    [SerializeField]
    private bool isGameOver = false;
    // ���ӽ��� ����
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
        if (isStart == true) return; // ���� ������ ���¸� ����X
        isStart = true; // ���� ���� ���·� ����

        // ���ӽ��� UI �� UX
        UIManager.Instance.UI_GameStart();

        // ���ӽ��� ���
        SoundManager.Instance.playBGM("���"); // ��� ���
        SoundManager.Instance.BGM_volumeControl(0.2f); // ��� �Ҹ�
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
