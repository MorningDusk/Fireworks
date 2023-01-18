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
        // °ÔÀÓ »ó´Ü Å¸ÀÌ¸Ó
        totalSecond = 0;

        // UI º¯°æ ¹× UX
        GameObject.Find("UI").transform.GetChild(0).gameObject.SetActive(true); // ÀÎ°ÔÀÓ UI È°¼ºÈ­
        GameObject.Find("UI").transform.GetChild(0).GetComponent<CanvasGroup>().DOFade(0, 0.0f); // Ã³À½¿¡ ÀÎ°ÔÀÓ È­¸é Åõ¸íÈ­µÈ »óÅÂ
        GameObject.Find("UI").transform.GetChild(0).GetComponent<CanvasGroup>().DOFade(1.0f, 0.5f); // ÀÎ°ÔÀÓ ÆäÀÌµåÀÎ
        GameObject.Find("UI").transform.GetChild(1).GetComponent<CanvasGroup>().DOFade(0, 0.5f); // ¸ÞÀÎÈ­¸é ÆäÀÌµå¾Æ¿ô
        GameObject.Find("UI").transform.GetChild(2).GetComponent<CanvasGroup>().DOFade(0, 0.5f); // °ÔÀÓ¿À¹öÈ­¸é ÆäÀÌµå¾Æ¿ô
        Invoke("deleteMainUIFade",0.5f); // 0.5ÃÊ µÚ¿¡ ¸ÞÀÎÈ­¸é ºñÈ°¼ºÈ­

        StartCoroutine(SecondIncrease());
        StartCoroutine(DisplayLightHouseHP());
        StartCoroutine(DisplayAttackerHP());
        StartCoroutine(DisplayCollecterHP());
    }

    //Å¸ÀÌÆ²/°ÔÀÓ¿À¹ö È­¸é Áö¿¬½Ã°£ µÚ ºñÈ°¼ºÈ­
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

        // °ÔÀÓ¿À¹ö È­¸é Å¸ÀÌ¸Ó Ç¥½Ã
        int textHour = totalSecond / 3600;
        int textMin = (totalSecond / 60) % 3600;
        int textSec = totalSecond % 60;
        string toDisplayHour = textHour.ToString();
        string toDisplayMin = textMin.ToString();
        string toDisplaySec = textSec.ToString();
        if (textHour < 10) toDisplayHour = "0" + textHour;
        if (textMin < 10) toDisplayMin = "0" + textMin;
        if (textSec < 10) toDisplaySec = "0" + textSec;
        GameObject.Find("GameoverTimeText").GetComponent<TextMeshProUGUI>().text = toDisplayHour + " : " + toDisplayMin + " : " + toDisplaySec; // Å¸ÀÌ¸Ó º¯°æ

        // È­¸é»ó´Ü ¹«ÇÑ½Ã°£ Áß´Ü
        StopCoroutine(SecondIncrease());

        // ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½/ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ Ã¼ï¿½ï¿½ Ç¥ï¿½ï¿½ ï¿½ß´ï¿½
        StopCoroutine(DisplayLightHouseHP());

        StopCoroutine(DisplayAttackerHP());
        StopCoroutine(DisplayCollecterHP());
    }

    // °ÔÀÓ¿À¹ö È­¸é - ¸ÞÀÎ(Å¸ÀÌÆ²)È­¸é ¹öÆ°
    public void Btn_ToMain()
    {
        // ¸ÞÀÎ Å¸ÀÌÆ² È­¸éÀ¸·Î
        GameObject.Find("UI").transform.GetChild(0).gameObject.SetActive(false);
        GameObject.Find("UI").transform.GetChild(1).gameObject.SetActive(true);
        GameObject.Find("UI").transform.GetChild(2).gameObject.SetActive(false);

        // ux
        GameObject.Find("UI").transform.GetChild(1).gameObject.GetComponent<CanvasGroup>().DOFade(1.0f,0.5f);
    }

    // °ÔÀÓ¿À¹ö È­¸é - Replay ¹öÆ°

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

    // ï¿½î¼® ï¿½æµ¹ï¿½ï¿½ ï¿½Ã·ï¿½ï¿½Ì¾î¸¦ ï¿½ï¿½ï¿½ï¿½Ù´Ï´ï¿?HP UI ï¿½ï¿½ï¿½ï¿½ï¿½Ï´ï¿½ ï¿½Ô¼ï¿½
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
    // ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿?ï¿½Ò²É³ï¿½ï¿½ï¿½ ï¿½ï¿½Åºï¿½ï¿½ UI ï¿½ï¿½ï¿½ï¿½ï¿½Ï´ï¿½ ï¿½Ô¼ï¿½
    public void UI_changeBullet()
    {
        PlayerScript player = gm.PlayerManager.GetChild(0).GetComponent<PlayerScript>();

        string displayText = ""; // ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½Ø½ï¿½Æ®
        displayText = player.GetBullet().ToString() + "/" + player.GetMaxBullet().ToString();
        GameObject.Find("BulletText").GetComponent<TextMeshProUGUI>().text = displayText;

        // UX
        DOTween.Sequence().Append(GameObject.Find("BulletText").transform.DOScale(1.1f, 0.2f)).Append(GameObject.Find("BulletText").transform.DOScale(1.0f, 0.2f));

    }

    // ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿?ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ È¹ï¿½ï¿½ï¿?UI ï¿½ï¿½ï¿½ï¿½ï¿½Ï´ï¿½ ï¿½Ô¼ï¿½
    public void UI_changeFragment()
    {
        PlayerScript player = gm.PlayerManager.GetChild(0).GetComponent<PlayerScript>();

        string displayText = ""; // ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½Ø½ï¿½Æ®
        displayText = player.GetPiece().ToString() + "/" + player.GetPiecePerBullet().ToString();
        GameObject.Find("FragmentText").GetComponent<TextMeshProUGUI>().text = displayText;

        // UX
        DOTween.Sequence().Append(GameObject.Find("FragmentText").transform.DOScale(1.1f, 0.2f)).Append(GameObject.Find("FragmentText").transform.DOScale(1.0f, 0.2f));
    }


    // ÇÃ·¹ÀÌ¾î ºÎÈ° UI
    int reviveCnt_SHOOTER = 10;
    int reviveCnt_CATCHER = 10;

    public void UI_revivePlayer(PlayerScript.Player_Type playerType)
    {
        // ï¿½Ã·ï¿½ï¿½Ì¾ï¿½ Å¸ï¿½ï¿½
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
        if(GameObject.Find("Attacker/HP").GetComponent<Slider>().value == 1) return; //HP ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½X

        GameObject.Find("Attacker/HP").GetComponent<Slider>().value += 0.1f;
        Invoke("UI_increaseHealth_SHOOTER", 0.5f);
    }

    public void UI_increaseHealth_CATCHER()
    {

        if (GameObject.Find("Collecter/HP").GetComponent<Slider>().value == 1) return; //HP ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½X

        GameObject.Find("Collecter/HP").GetComponent<Slider>().value += 0.1f;
        Invoke("UI_increaseHealth_CATCHER", 0.5f);
    }

    public void PrevButton()
    {
        if (titleCurScreenIdx == 1) return;
        // ï¿½ï¿½ï¿½ï¿½Ä«ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½Ä¡ï¿½ï¿½ ï¿½Ñ±ï¿½ï¿?
        GameObject.Find("TitleTutorials/Image" + titleCurScreenIdx).transform.DOLocalMoveX(2000f, 0.5f).SetEase(Ease.InOutExpo);
        titleCurScreenIdx--;
        // ï¿½ï¿½ï¿½ï¿½Ä«ï¿½å¸¦ ï¿½ï¿½ï¿½ï¿½Ä«ï¿½ï¿½ï¿½ï¿½Ä¡ï¿½ï¿½
        GameObject.Find("TitleTutorials/Image" + titleCurScreenIdx).transform.DOLocalMoveX(0f, 0.5f).SetEase(Ease.InOutExpo);

    }

    public void NextButton()
    {
        if (titleCurScreenIdx == 3) return;
        // ï¿½ï¿½ï¿½ï¿½Ä«ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½Ä¡ï¿½ï¿½ ï¿½Ñ±ï¿½ï¿?
        GameObject.Find("TitleTutorials/Image" + titleCurScreenIdx).transform.DOLocalMoveX(-2000f, 0.5f).SetEase(Ease.InOutExpo);
        titleCurScreenIdx++;
        // ï¿½ï¿½ï¿½ï¿½Ä«ï¿½å¸¦ ï¿½ï¿½ï¿½ï¿½Ä«ï¿½ï¿½ï¿½ï¿½Ä¡ï¿½ï¿½
        GameObject.Find("TitleTutorials/Image" + titleCurScreenIdx).transform.DOLocalMoveX(0f, 0.5f).SetEase(Ease.InOutExpo);
    }

    IEnumerator checkButtonAble()
    {
        while (gm.getIsStart() == false)
        {  // ï¿½ï¿½ï¿½Ó½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ß´Ù¸ï¿½ ï¿½Ë»ï¿½
            if (titleCurScreenIdx == 1) GameObject.Find("prev").GetComponent<Image>().DOFade(0.2f, 0.4f);
            if (titleCurScreenIdx == 3) GameObject.Find("next").GetComponent<Image>().DOFade(0.2f, 0.4f);
            yield return new WaitForSeconds(0.5f);
        }
        if (gm.getIsStart() == true) StopCoroutine(checkButtonAble());
    }

    // ï¿½ï¿½ï¿½ï¿½ Å¸ï¿½ï¿½Æ²È­ï¿½ï¿½ prev/next ï¿½ï¿½Æ°
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


    // ï¿½ï¿½Å¸Æ®È­ï¿½ï¿½ ï¿½ï¿½Å¸Æ® ï¿½ï¿½Æ°
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