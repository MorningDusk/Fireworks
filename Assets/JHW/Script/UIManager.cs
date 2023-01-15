using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    // 게임매니저, Start에서 객체 받아옴
    GameManager gm;

    // 화면 상단 타이머
    public int totalSecond = 0;
    private bool isTimerAble = true;

    private TextMeshProUGUI timeMinText;
    private TextMeshProUGUI timeSecText;

    // 타이틀화면 초기 화면 인덱스
    private int titleCurScreenIdx = 1;

    public int Get_Time() { return totalSecond; }

    private void Start() {
        // 싱글톤 불러오기
        gm = GameManager.Instance;

        // 텍스트 불러오기
        timeMinText = GameObject.Find("TimeMin").GetComponent<TextMeshProUGUI>();
        timeSecText = GameObject.Find("TimeSec").GetComponent<TextMeshProUGUI>();

        // 처음에 인게임 UI 비활성화
        GameObject.Find("UI").transform.GetChild(0).gameObject.SetActive(false);
        // 메인화면 UI 활성화
        GameObject.Find("UI").transform.GetChild(1).gameObject.SetActive(true);

        // 메인타이틀 이전/다음 버튼 활성화/비활성화 체크
        //StartCoroutine(checkButtonAble());
    }


    // 게임 시작 시 UI, GameManager에서 호출
    public void UI_GameStart()
    {
        // UI 변경 및 UX
        GameObject.Find("UI").transform.GetChild(0).gameObject.SetActive(true); // 인게임 UI 활성화
        GameObject.Find("UI").transform.GetChild(0).GetComponent<CanvasGroup>().DOFade(0, 0.0f); // 처음에 인게임 화면 투명화된 상태
        GameObject.Find("UI").transform.GetChild(0).GetComponent<CanvasGroup>().DOFade(1.0f, 0.5f); // 인게임 페이드인
        GameObject.Find("UI").transform.GetChild(1).GetComponent<CanvasGroup>().DOFade(0, 0.5f); // 메인화면 페이드아웃
        StartCoroutine(deleteMainUIFade(0.5f)); // 0.5초 뒤에 메인화면 비활성화

        // 화면상단 무한타이머 시작
        StartCoroutine(SecondIncrease());

        // 공격자/수집자 체력 표시 시작
        StartCoroutine(DisplayAttackerHP());
        StartCoroutine(DisplayCollecterHP());
    }

    // 지연시간 뒤에 메인화면 비활성화
    IEnumerator deleteMainUIFade(float time)
    {
        yield return new WaitForSeconds(time);
        GameObject.Find("UI").transform.GetChild(1).gameObject.SetActive(false);
    }

    // 게임 오버 시 UI, GameManager에서 호출
    public void UI_GameOver()
    {
        // UI 변경 및 UX
        GameObject.Find("UI").transform.GetChild(2).gameObject.SetActive(true);
        GameObject.Find("UI").transform.GetChild(2).GetComponent<CanvasGroup>().DOFade(0, 0); // 게임오버 화면 처음에 안보이게
        GameObject.Find("UI").transform.GetChild(2).GetComponent<CanvasGroup>().DOFade(1, 0.5f); // 게임오버 화면 페이드인

        // 화면상단 무한시간 중단
        StopCoroutine(SecondIncrease());

        // 공격자/수집자 체력 표시 중단
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

    // 운석 충돌시 플레이어를 따라다니는 HP UI 변경하는 함수
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

    // 왼쪽 상단 불꽃놀이 총탄수 UI 변경하는 함수
    public void UI_changeBullet()
    {
        PlayerScript player = GameObject.Find("Players").transform.GetChild(0).GetComponent<PlayerScript>();

        string displayText = ""; // 보여질 텍스트
        displayText = player.GetBullet().ToString() + "/" + player.GetMaxBullet().ToString();
        GameObject.Find("BulletText").GetComponent<TextMeshProUGUI>().text = displayText;

        // UX
        DOTween.Sequence().Append(GameObject.Find("BulletText").transform.DOScale(1.1f, 0.2f)).Append(GameObject.Find("BulletText").transform.DOScale(1.0f, 0.2f));

    }

    // 오른쪽 상단 유성 조각 획득시 UI 변경하는 함수
    public void UI_changeFragment()
    {
        PlayerScript player = GameObject.Find("Players").transform.GetChild(0).GetComponent<PlayerScript>();

        string displayText = ""; // 보여질 텍스트
        displayText = player.GetPiece().ToString() + "/" + player.GetPiecePerBullet().ToString();
        GameObject.Find("FragmentText").GetComponent<TextMeshProUGUI>().text = displayText;

        // UX
        DOTween.Sequence().Append(GameObject.Find("FragmentText").transform.DOScale(1.1f, 0.2f)).Append(GameObject.Find("FragmentText").transform.DOScale(1.0f, 0.2f));
    }


    public void PrevButton()
    {
        if (titleCurScreenIdx == 1) return;
        // 현재카드 다음 위치로 넘긴다
        GameObject.Find("TitleTutorials/Image" + titleCurScreenIdx).transform.DOLocalMoveX(2000f, 0.5f).SetEase(Ease.InOutExpo);
        titleCurScreenIdx--;
        // 이전카드를 현재카드위치로
        GameObject.Find("TitleTutorials/Image" + titleCurScreenIdx).transform.DOLocalMoveX(0f, 0.5f).SetEase(Ease.InOutExpo);

    }

    public void NextButton()
    {
        if (titleCurScreenIdx == 3) return;
        // 현재카드 이전 위치로 넘긴다
        GameObject.Find("TitleTutorials/Image" + titleCurScreenIdx).transform.DOLocalMoveX(-2000f, 0.5f).SetEase(Ease.InOutExpo);
        titleCurScreenIdx++;
        // 다음카드를 현재카드위치로
        GameObject.Find("TitleTutorials/Image" + titleCurScreenIdx).transform.DOLocalMoveX(0f, 0.5f).SetEase(Ease.InOutExpo);
    }

    IEnumerator checkButtonAble()
    {
        while (gm.getIsStart() == false)
        {  // 게임시작 안했다면 검사
            if (titleCurScreenIdx == 1) GameObject.Find("prev").GetComponent<Image>().DOFade(0.2f, 0.4f);
            if (titleCurScreenIdx == 3) GameObject.Find("next").GetComponent<Image>().DOFade(0.2f, 0.4f);
            yield return new WaitForSeconds(0.5f);
        }
        if (gm.getIsStart() == true) StopCoroutine(checkButtonAble());
    }

    // 메인 타이틀화면 prev/next 버튼
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


    // 스타트화면 스타트 버튼
    public void StartBtn_MouseOver(GameObject selfObj)
    {
        selfObj.transform.DOScale(1.2f, 0.3f);
        selfObj.transform.GetComponent<Image>().DOColor(new Color(1f,0.9f,0.6f),0.3f);
        SoundManager.Instance.playSFX("점프");
    }
    public void StartBtn_MouseOut(GameObject selfObj)
    {
        selfObj.transform.DOScale(1.0f, 0.3f);
        selfObj.transform.GetComponent<Image>().DOColor(new Color(1f, 1, 1), 0.3f);
    }
}