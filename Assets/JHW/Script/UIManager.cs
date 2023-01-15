using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    // ���ӸŴ���, Start���� ��ü �޾ƿ�
    GameManager gm;

    // ȭ�� ��� Ÿ�̸�
    public int totalSecond = 0;
    private bool isTimerAble = true;

    private TextMeshProUGUI timeMinText;
    private TextMeshProUGUI timeSecText;

    // Ÿ��Ʋȭ�� �ʱ� ȭ�� �ε���
    private int titleCurScreenIdx = 1;

    public int Get_Time() { return totalSecond; }

    private void Start() {
        // �̱��� �ҷ�����
        gm = GameManager.Instance;

        // �ؽ�Ʈ �ҷ�����
        timeMinText = GameObject.Find("TimeMin").GetComponent<TextMeshProUGUI>();
        timeSecText = GameObject.Find("TimeSec").GetComponent<TextMeshProUGUI>();

        // ó���� �ΰ��� UI ��Ȱ��ȭ
        GameObject.Find("UI").transform.GetChild(0).gameObject.SetActive(false);
        // ����ȭ�� UI Ȱ��ȭ
        GameObject.Find("UI").transform.GetChild(1).gameObject.SetActive(true);

        // ����Ÿ��Ʋ ����/���� ��ư Ȱ��ȭ/��Ȱ��ȭ üũ
        //StartCoroutine(checkButtonAble());
    }


    // ���� ���� �� UI, GameManager���� ȣ��
    public void UI_GameStart()
    {
        // UI ���� �� UX
        GameObject.Find("UI").transform.GetChild(0).gameObject.SetActive(true); // �ΰ��� UI Ȱ��ȭ
        GameObject.Find("UI").transform.GetChild(0).GetComponent<CanvasGroup>().DOFade(0, 0.0f); // ó���� �ΰ��� ȭ�� ����ȭ�� ����
        GameObject.Find("UI").transform.GetChild(0).GetComponent<CanvasGroup>().DOFade(1.0f, 0.5f); // �ΰ��� ���̵���
        GameObject.Find("UI").transform.GetChild(1).GetComponent<CanvasGroup>().DOFade(0, 0.5f); // ����ȭ�� ���̵�ƿ�
        StartCoroutine(deleteMainUIFade(0.5f)); // 0.5�� �ڿ� ����ȭ�� ��Ȱ��ȭ

        // ȭ���� ����Ÿ�̸� ����
        StartCoroutine(SecondIncrease());

        // ������/������ ü�� ǥ�� ����
        StartCoroutine(DisplayAttackerHP());
        StartCoroutine(DisplayCollecterHP());
    }

    // �����ð� �ڿ� ����ȭ�� ��Ȱ��ȭ
    IEnumerator deleteMainUIFade(float time)
    {
        yield return new WaitForSeconds(time);
        GameObject.Find("UI").transform.GetChild(1).gameObject.SetActive(false);
    }

    // ���� ���� �� UI, GameManager���� ȣ��
    public void UI_GameOver()
    {
        // UI ���� �� UX
        GameObject.Find("UI").transform.GetChild(2).gameObject.SetActive(true);
        GameObject.Find("UI").transform.GetChild(2).GetComponent<CanvasGroup>().DOFade(0, 0); // ���ӿ��� ȭ�� ó���� �Ⱥ��̰�
        GameObject.Find("UI").transform.GetChild(2).GetComponent<CanvasGroup>().DOFade(1, 0.5f); // ���ӿ��� ȭ�� ���̵���

        // ȭ���� ���ѽð� �ߴ�
        StopCoroutine(SecondIncrease());

        // ������/������ ü�� ǥ�� �ߴ�
        StopCoroutine(DisplayAttackerHP());
        StopCoroutine(DisplayCollecterHP());
    }

    IEnumerator DisplayAttackerHP()
    {
        while (!gm.getGameOver())
        {
            Vector3 locationHP = Camera.main.WorldToScreenPoint(GameObject.Find("Players").transform.GetChild(0).position);
//<<<<<<< HEAD
            GameObject.Find("UI/InGame/Attacker").transform.position = new Vector3(locationHP.x, locationHP.y + 100f, locationHP.z);
//=======
//            GameObject.Find("UI/Attacker").transform.position = new Vector3(locationHP.x,locationHP.y + 120f,locationHP.z);
//>>>>>>> Dev_LSY
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator DisplayCollecterHP()
    {
        while (!gm.getGameOver())
        {
            Vector3 locationHP = Camera.main.WorldToScreenPoint(GameObject.Find("Players").transform.GetChild(1).position);
//<<<<<<< HEAD
            GameObject.Find("UI/InGame/Collecter").transform.position = new Vector3(locationHP.x, locationHP.y + 100f, locationHP.z);
//=======
//            GameObject.Find("UI/Collecter").transform.position = new Vector3(locationHP.x, locationHP.y + 120f, locationHP.z); 
//>>>>>>> Dev_LSY
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator SecondIncrease()
    {
        while (isTimerAble && !gm.getGameOver())
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
                GameObject.Find("UI/Attacker").transform.GetChild(0).GetComponent<Slider>().value = (float)player.GetHealth() / player.GetMaxHealth();
                break;
            case PlayerScript.Player_Type.CATCHER:
                player = GameObject.Find("Players").transform.GetChild(1).GetComponent<PlayerScript>();
                GameObject.Find("UI/Collecter").transform.GetChild(0).GetComponent<Slider>().value = (float)player.GetHealth() / player.GetMaxHealth();
                break;
        }
    }

    // ���� ��� �Ҳɳ��� ��ź�� UI �����ϴ� �Լ�
    public void UI_changeBullet()
    {
        PlayerScript player = GameObject.Find("Players").transform.GetChild(0).GetComponent<PlayerScript>();

        string displayText = ""; // ������ �ؽ�Ʈ
        displayText = player.GetBullet().ToString() + "/" + player.GetMaxBullet().ToString();
        GameObject.Find("BulletText").GetComponent<TextMeshProUGUI>().text = displayText;

        // UX
        DOTween.Sequence().Append(GameObject.Find("BulletText").transform.DOScale(1.1f, 0.2f)).Append(GameObject.Find("BulletText").transform.DOScale(1.0f, 0.2f));

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


    public void PrevButton()
    {
        if (titleCurScreenIdx == 1) return;
        // ����ī�� ���� ��ġ�� �ѱ��
        GameObject.Find("TitleTutorials/Image" + titleCurScreenIdx).transform.DOLocalMoveX(2000f, 0.5f).SetEase(Ease.InOutExpo);
        titleCurScreenIdx--;
        // ����ī�带 ����ī����ġ��
        GameObject.Find("TitleTutorials/Image" + titleCurScreenIdx).transform.DOLocalMoveX(0f, 0.5f).SetEase(Ease.InOutExpo);

    }

    public void NextButton()
    {
        if (titleCurScreenIdx == 3) return;
        // ����ī�� ���� ��ġ�� �ѱ��
        GameObject.Find("TitleTutorials/Image" + titleCurScreenIdx).transform.DOLocalMoveX(-2000f, 0.5f).SetEase(Ease.InOutExpo);
        titleCurScreenIdx++;
        // ����ī�带 ����ī����ġ��
        GameObject.Find("TitleTutorials/Image" + titleCurScreenIdx).transform.DOLocalMoveX(0f, 0.5f).SetEase(Ease.InOutExpo);
    }

    IEnumerator checkButtonAble()
    {
        while (gm.getIsStart() == false)
        {  // ���ӽ��� ���ߴٸ� �˻�
            if (titleCurScreenIdx == 1) GameObject.Find("prev").GetComponent<Image>().DOFade(0.2f, 0.4f);
            if (titleCurScreenIdx == 3) GameObject.Find("next").GetComponent<Image>().DOFade(0.2f, 0.4f);
            yield return new WaitForSeconds(0.5f);
        }
        if (gm.getIsStart() == true) StopCoroutine(checkButtonAble());
    }

    // ���� Ÿ��Ʋȭ�� prev/next ��ư
    public void Btn_MouseOver(GameObject selfObj)
    {
        selfObj.transform.DOScale(1.2f, 0.3f);
    }
    public void Btn_MouseOut(GameObject selfObj)
    {
        selfObj.transform.DOScale(1.0f, 0.3f);
    }
    public void Btn_Prev_MouseClick(GameObject selfObj)
    {
        float curX = selfObj.transform.position.x;
        selfObj.transform.DOMoveX(curX-20, 0);
        selfObj.transform.DOMoveX(curX, 0.3f).SetEase(Ease.InCirc);
    }
    public void Btn_Next_MouseClick(GameObject selfObj)
    {
        float curX = selfObj.transform.position.x;
        selfObj.transform.DOMoveX(curX+20, 0);
        selfObj.transform.DOMoveX(curX, 0.3f).SetEase(Ease.InCirc);
    }


    // ��ŸƮȭ�� ��ŸƮ ��ư
    public void StartBtn_MouseOver(GameObject selfObj)
    {
        selfObj.transform.DOScale(1.2f, 0.3f);
        selfObj.transform.GetComponent<Image>().DOColor(new Color(1f,0.9f,0.6f),0.3f);
        SoundManager.Instance.playSFX("����");
    }
    public void StartBtn_MouseOut(GameObject selfObj)
    {
        selfObj.transform.DOScale(1.0f, 0.3f);
        selfObj.transform.GetComponent<Image>().DOColor(new Color(1f, 1, 1), 0.3f);
    }
}