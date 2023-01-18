using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public Transform PlayerManager, BulletManager, MeteorManager, FragmentManager, MapManager;

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

        UIManager.Instance.UI_GameOver();
    }

    public void GameRestart()
    {
        isGameOver = false;
        isStart = true;

        UIManager.Instance.UI_GameStart();
    }

    public void gameOverCheck()
    {
        // �÷��̾� 2�� �� �׾������� ���ӿ���
        if(PlayerManager.GetChild(0).GetComponent<PlayerScript>().isPlayerDead()
            && PlayerManager.GetChild(1).GetComponent<PlayerScript>().isPlayerDead())
        {
            GameOver();
        }


    }
}
