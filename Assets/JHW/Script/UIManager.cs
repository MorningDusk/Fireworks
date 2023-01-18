using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    GameManager gm;

    public int totalSecond = 0;
    private bool isTimerAble = true;

    private TextMeshProUGUI timeMinText;
    private TextMeshProUGUI timeSecText;

    private int titleCurScreenIdx = 1;

    bool BtnisClicked = true;
    public int Get_Time() { return totalSecond; }

    private void Start() {
        gm = GameManager.Instance;

        timeMinText = GameObject.Find("TimeMin").GetComponent<TextMeshProUGUI>();
        timeSecText = GameObject.Find("TimeSec").GetComponent<TextMeshProUGUI>();

        GameObject.Find("UI").transform.GetChild(0).gameObject.SetActive(false);
        GameObject.Find("UI").transform.GetChild(1).gameObject.SetActive(true);
    }


    public void UI_GameStart()
    {
        // ���� ��� Ÿ�̸�
        totalSecond = 0;

        // UI ���� �� UX
        GameObject.Find("UI").transform.GetChild(0).gameObject.SetActive(true); // �ΰ��� UI Ȱ��ȭ
        GameObject.Find("UI").transform.GetChild(0).GetComponent<CanvasGroup>().DOFade(0, 0.0f); // ó���� �ΰ��� ȭ�� ����ȭ�� ����
        GameObject.Find("UI").transform.GetChild(0).GetComponent<CanvasGroup>().DOFade(1.0f, 0.5f); // �ΰ��� ���̵���
        GameObject.Find("UI").transform.GetChild(1).GetComponent<CanvasGroup>().DOFade(0, 0.5f); // ����ȭ�� ���̵�ƿ�
        GameObject.Find("UI").transform.GetChild(2).GetComponent<CanvasGroup>().DOFade(0, 0.5f); // ���ӿ���ȭ�� ���̵�ƿ�
        Invoke("deleteMainUIFade",0.5f); // 0.5�� �ڿ� ����ȭ�� ��Ȱ��ȭ

        StartCoroutine(SecondIncrease());
        StartCoroutine(DisplayLightHouseHP());
        StartCoroutine(DisplayAttackerHP());
        StartCoroutine(DisplayCollecterHP());
    }

    //Ÿ��Ʋ/���ӿ��� ȭ�� �����ð� �� ��Ȱ��ȭ
    public void deleteMainUIFade()
    {
        if (GameObject.Find("UI").transform.GetChild(1).gameObject.activeSelf == true) GameObject.Find("UI").transform.GetChild(1).gameObject.SetActive(false);
        if (GameObject.Find("UI").transform.GetChild(2).gameObject.activeSelf == true) GameObject.Find("UI").transform.GetChild(2).gameObject.SetActive(false);
    }

    public void UI_GameOver()
    {
        GameObject.Find("UI").transform.GetChild(2).gameObject.SetActive(true);
        GameObject.Find("UI").transform.GetChild(2).GetComponent<CanvasGroup>().DOFade(0, 0);
        GameObject.Find("UI").transform.GetChild(2).GetComponent<CanvasGroup>().DOFade(1, 0.5f);

        // ���ӿ��� ȭ�� Ÿ�̸� ǥ��
        int textHour = totalSecond / 3600;
        int textMin = (totalSecond / 60) % 3600;
        int textSec = totalSecond % 60;
        string toDisplayHour = textHour.ToString();
        string toDisplayMin = textMin.ToString();
        string toDisplaySec = textSec.ToString();
        if (textHour < 10) toDisplayHour = "0" + textHour;
        if (textMin < 10) toDisplayMin = "0" + textMin;
        if (textSec < 10) toDisplaySec = "0" + textSec;
        GameObject.Find("GameoverTimeText").GetComponent<TextMeshProUGUI>().text = toDisplayHour + " : " + toDisplayMin + " : " + toDisplaySec; // Ÿ�̸� ����

        // ȭ���� ���ѽð� �ߴ�
        StopCoroutine(SecondIncrease());

        // ������/������ ü�� ǥ�� �ߴ�
        StopCoroutine(DisplayLightHouseHP());

        StopCoroutine(DisplayAttackerHP());
        StopCoroutine(DisplayCollecterHP());
    }

    // ���ӿ��� ȭ�� - ����(Ÿ��Ʋ)ȭ�� ��ư
    public void Btn_ToMain()
    {
        // ���� Ÿ��Ʋ ȭ������
        GameObject.Find("UI").transform.GetChild(0).gameObject.SetActive(false);
        GameObject.Find("UI").transform.GetChild(1).gameObject.SetActive(true);
        GameObject.Find("UI").transform.GetChild(2).gameObject.SetActive(false);

        // ux
        GameObject.Find("UI").transform.GetChild(1).gameObject.GetComponent<CanvasGroup>().DOFade(1.0f,0.5f);
    }

    // ���ӿ��� ȭ�� - Replay ��ư

    IEnumerator DisplayLightHouseHP()
    {
        while (!gm.getGameOver())
        {
            Vector3 locationHP = Camera.main.WorldToScreenPoint(GameObject.Find("Lighthouse").transform.position);
            GameObject.Find("LightHouseHP").transform.position = new Vector3(locationHP.x, locationHP.y + 500f, locationHP.z);

            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator DisplayAttackerHP()
    {
        while (!gm.getGameOver())
        {
            Vector3 locationHP = Camera.main.WorldToScreenPoint(gm.PlayerManager.GetChild(0).position);
            GameObject.Find("UI/InGame/Attacker").transform.position = new Vector3(locationHP.x, locationHP.y + 125f, locationHP.z);
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator DisplayCollecterHP()
    {
        while (!gm.getGameOver())
        {
            Vector3 locationHP = Camera.main.WorldToScreenPoint(gm.PlayerManager.GetChild(1).position);
            GameObject.Find("UI/InGame/Collecter").transform.position = new Vector3(locationHP.x, locationHP.y + 125f, locationHP.z);
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

    // � �浹�� �÷��̾ ����ٴϴ�?HP UI �����ϴ� �Լ�
    public void UI_changeHP(PlayerScript.Player_Type _Type)
    {
        PlayerScript player;

        switch (_Type)
        {
            case PlayerScript.Player_Type.SHOOTER:
                player = gm.PlayerManager.GetChild(0).GetComponent<PlayerScript>();
                GameObject.Find("UI/InGame/Attacker").transform.GetChild(0).GetComponent<Slider>().value = (float)player.GetHealth() / player.GetMaxHealth();
                break;
            case PlayerScript.Player_Type.CATCHER:
                player = gm.PlayerManager.GetChild(1).GetComponent<PlayerScript>();
                GameObject.Find("UI/InGame/Collecter").transform.GetChild(0).GetComponent<Slider>().value = (float)player.GetHealth() / player.GetMaxHealth();
                break;
        }
    }

    public void UI_changeHP_Lighthouse()
    {
        GameObject.Find("LightHouseHP").transform.GetChild(0).GetComponent<Slider>().value = (float)LightHouse.Instance.get_Durability() / LightHouse.Instance.get_MaxDurability();
    }
    // ���� ���?�Ҳɳ��� ��ź�� UI �����ϴ� �Լ�
    public void UI_changeBullet()
    {
        PlayerScript player = gm.PlayerManager.GetChild(0).GetComponent<PlayerScript>();

        string displayText = ""; // ������ �ؽ�Ʈ
        displayText = player.GetBullet().ToString() + "/" + player.GetMaxBullet().ToString();
        GameObject.Find("BulletText").GetComponent<TextMeshProUGUI>().text = displayText;

        // UX
        DOTween.Sequence().Append(GameObject.Find("BulletText").transform.DOScale(1.1f, 0.2f)).Append(GameObject.Find("BulletText").transform.DOScale(1.0f, 0.2f));

    }

    // ������ ���?���� ���� ȹ���?UI �����ϴ� �Լ�
    public void UI_changeFragment()
    {
        PlayerScript player = gm.PlayerManager.GetChild(0).GetComponent<PlayerScript>();

        string displayText = ""; // ������ �ؽ�Ʈ
        displayText = player.GetPiece().ToString() + "/" + player.GetPiecePerBullet().ToString();
        GameObject.Find("FragmentText").GetComponent<TextMeshProUGUI>().text = displayText;

        // UX
        DOTween.Sequence().Append(GameObject.Find("FragmentText").transform.DOScale(1.1f, 0.2f)).Append(GameObject.Find("FragmentText").transform.DOScale(1.0f, 0.2f));
    }


    // �÷��̾� ��Ȱ UI
    int reviveCnt_SHOOTER = 10;
    int reviveCnt_CATCHER = 10;

    public void UI_revivePlayer(PlayerScript.Player_Type playerType)
    {
        // �÷��̾� Ÿ��
        PlayerScript.Player_Type type = playerType;

        switch (type)
        {
            case PlayerScript.Player_Type.SHOOTER:
                UI_increaseHealth_SHOOTER();
                break;

            case PlayerScript.Player_Type.CATCHER:
                UI_increaseHealth_CATCHER();
                break;
        }
        
    }

    
    public void UI_increaseHealth_SHOOTER()
    {
        if(GameObject.Find("Attacker/HP").GetComponent<Slider>().value == 1) return; //HP �������� ����X

        GameObject.Find("Attacker/HP").GetComponent<Slider>().value += 0.1f;
        Invoke("UI_increaseHealth_SHOOTER", 0.5f);
    }

    public void UI_increaseHealth_CATCHER()
    {

        if (GameObject.Find("Collecter/HP").GetComponent<Slider>().value == 1) return; //HP �������� ����X

        GameObject.Find("Collecter/HP").GetComponent<Slider>().value += 0.1f;
        Invoke("UI_increaseHealth_CATCHER", 0.5f);
    }

    public void PrevButton()
    {
        if (titleCurScreenIdx == 1) return;
        // ����ī�� ���� ��ġ�� �ѱ��?
        GameObject.Find("TitleTutorials/Image" + titleCurScreenIdx).transform.DOLocalMoveX(2000f, 0.5f).SetEase(Ease.InOutExpo);
        titleCurScreenIdx--;
        // ����ī�带 ����ī����ġ��
        GameObject.Find("TitleTutorials/Image" + titleCurScreenIdx).transform.DOLocalMoveX(0f, 0.5f).SetEase(Ease.InOutExpo);

    }

    public void NextButton()
    {
        if (titleCurScreenIdx == 3) return;
        // ����ī�� ���� ��ġ�� �ѱ��?
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
        //if (BtnIsCliced) return;
        float curX = selfObj.transform.position.x;
        selfObj.transform.DOMoveX(curX-20, 0);
        selfObj.transform.DOMoveX(curX, 0.3f).SetEase(Ease.InCirc);
    }
    public void Btn_Next_MouseClick(GameObject selfObj)
    {
        //if (BtnIsClicked) return;
        float curX = selfObj.transform.position.x;
        selfObj.transform.DOMoveX(curX+20, 0);
        selfObj.transform.DOMoveX(curX, 0.3f).SetEase(Ease.InCirc);
    }


    // ��ŸƮȭ�� ��ŸƮ ��ư
    public void StartBtn_MouseOver(GameObject selfObj)
    {
        selfObj.transform.DOScale(1.2f, 0.3f);
        selfObj.transform.GetComponent<Image>().DOColor(new Color(1f,0.9f,0.6f),0.3f);
    }
    public void StartBtn_MouseOut(GameObject selfObj)
    {
        selfObj.transform.DOScale(1.0f, 0.3f);
        selfObj.transform.GetComponent<Image>().DOColor(new Color(1f, 1, 1), 0.3f);
    }
}