using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

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

        // ���� ������ġ ����
        StartCoroutine(FragmentManager.GetComponent<FragmentManager>().randomCreateFragment());
//=======
        //player1.transform.localPosition = new Vector3(-5f, 5f,0);
        //player2.transform.localPosition = new Vector3(5f, 5f,0);
//>>>>>>> Dev_LSY2

        // UI
        GameObject.Find("Attacker/HP").GetComponent<Slider>().value = 1;
        GameObject.Find("Collecter/HP").GetComponent<Slider>().value = 1;
        GameObject.Find("LightHouseHP/HP").GetComponent<Slider>().value = 1;

        UIManager.Instance.UI_changeBullet(); // �Ѿ� ����UI ����
        UIManager.Instance.UI_changeFragment(); // ���� ����UI ����
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
