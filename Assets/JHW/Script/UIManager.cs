using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    // ȭ�� ��� Ÿ�̸�
    public int totalSecond = 0;
    private bool isTimerAble = true;

    private TextMeshProUGUI timeMinText;
    private TextMeshProUGUI timeSecText;

    public int Get_Time() { return totalSecond; }

    private void Start() {
        timeMinText = GameObject.Find("TimeMin").GetComponent<TextMeshProUGUI>();
        timeSecText = GameObject.Find("TimeSec").GetComponent<TextMeshProUGUI>();

        // ȭ���� ���ѽð�
        StartCoroutine(SecondIncrease());

        // ������/������ ü�� ǥ��
        StartCoroutine(DisplayAttackerHP());
        StartCoroutine(DisplayCollecterHP());
    }

    IEnumerator DisplayAttackerHP()
    {
        while (true)
        {
            Vector3 locationHP = Camera.main.WorldToScreenPoint(GameObject.Find("Players").transform.GetChild(0).position);
            GameObject.Find("UI/Attacker").transform.position = new Vector3(locationHP.x,locationHP.y + 50f,locationHP.z);
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator DisplayCollecterHP()
    {
        while (true)
        {
            Vector3 locationHP = Camera.main.WorldToScreenPoint(GameObject.Find("Players").transform.GetChild(1).position);
            GameObject.Find("UI/Collecter").transform.position = new Vector3(locationHP.x, locationHP.y + 50f, locationHP.z); 
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator SecondIncrease()
    {
        while (isTimerAble)
        {
            totalSecond++;
            string timeMin = (totalSecond / 60).ToString();
            if (totalSecond / 60 < 10) timeMin = "0" + timeMin;
            string timeSec = (totalSecond % 60).ToString();
            if (totalSecond % 60 < 10) timeSec = "0" + timeSec;
            timeMinText.text = timeMin;
            timeSecText.text = timeSec;
            yield return new WaitForSeconds(1f);
        }
    }

    // � �浹�� �÷��̾ ����ٴϴ� HP UI �����ϴ� �Լ�
    public void UI_changeHP(PlayerScript.Player_Type _Type)
    {
        PlayerScript player;

        switch (_Type)
        {
            case PlayerScript.Player_Type.SHOOTER:
                player = GameObject.Find("Players").transform.GetChild(0).GetComponent<PlayerScript>();
                GameObject.Find("UI/Attacker").transform.GetChild(0).GetComponent<Slider>().value = (float) player.GetHealth() / player.GetMaxHealth();
                break;
            case PlayerScript.Player_Type.CATCHER:
                player = GameObject.Find("Players").transform.GetChild(1).GetComponent<PlayerScript>();
                GameObject.Find("UI/Collecter").transform.GetChild(0).GetComponent<Slider>().value = (float) player.GetHealth() / player.GetMaxHealth();
                break;
        }
    }


    // ������ ��� ���� ���� ȹ��� UI �����ϴ� �Լ�
    public void UI_changeFragment()
    {
        PlayerScript player = GameObject.Find("Players").transform.GetChild(0).GetComponent<PlayerScript>();

        string displayText = ""; // ������ �ؽ�Ʈ
        displayText = player.GetPiece().ToString() + "/" + player.GetPiecePerBullet().ToString();
        GameObject.Find("FragmentText").GetComponent<TextMeshProUGUI>().text = displayText;

        // UX
        DOTween.Sequence().Append(GameObject.Find("FragmentText").transform.DOScale(1.1f, 0.2f)).Append(GameObject.Find("FragmentText").transform.DOScale(1.0f, 0.2f));

    }
}
