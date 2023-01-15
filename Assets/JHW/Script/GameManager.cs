using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

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

        // ���ӽ��� UI �� UX, GameRestart �� ����
        GameRestart();

        // ���ӽ��� ���
        SoundManager.Instance.playBGM("���"); // ��� ���
        SoundManager.Instance.BGM_volumeControl(0.2f); // ��� �Ҹ�
    }

    public void GameOver()
    {
        isGameOver = true;
        isStart = false;

        UIManager.Instance.UI_GameOver();

        // ���Ӻ�� �ߴ�
        SoundManager.Instance.pauseBGM();
    }

    public void GameRestart()
    {
        isGameOver = false;
        isStart = true;

        UIManager.Instance.UI_GameStart();

        GameObject player1 = GameObject.Find("Players").transform.GetChild(0).gameObject;
        GameObject player2 = GameObject.Find("Players").transform.GetChild(1).gameObject;

        player1.GetComponent<PlayerScript>().playerInit();
        player2.GetComponent<PlayerScript>().playerInit();

        player1.transform.localPosition = new Vector3(-5f, 5f,0);
        player2.transform.localPosition = new Vector3(5f, 5f,0);

        // UI
        GameObject.Find("Attacker/HP").GetComponent<Slider>().value = 1;
        GameObject.Find("Collecter/HP").GetComponent<Slider>().value = 1;
    }

    public void gameOverCheck()
    {
        // �÷��̾� 2�� �� �׾������� ���ӿ���
        if (GameObject.Find("Players").transform.GetChild(0).GetComponent<PlayerScript>().isPlayerDead()
            && GameObject.Find("Players").transform.GetChild(1).GetComponent<PlayerScript>().isPlayerDead())
        {
            GameOver();
        }


    }
}
