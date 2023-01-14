using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fragment : MonoBehaviour
{

    // 기획서에 있는 변수 LifeTime (탄환 변수) , FadeTime (유성 조각 변수)
    float LifeTime; 
    float FadeTime = 4f;

    // 조각 수집 가능 여부
    private bool isCollectAble = false;

    // getter
    public bool getCollectAble() { return isCollectAble; }

    // setter
    public void setCollectAble(bool flag) { isCollectAble = flag; }


    private void Update()
    {
        collectFragment();
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            // 플레이어와 조각 충돌시 주울 수 있게
            case "Player":
                // 플레이어 충돌했는데 공격자면 실행X 
                if (collision.gameObject.GetComponent<PlayerScript>().GetPlayerType() == PlayerScript.Player_Type.SHOOTER) return;
                isCollectAble = true;
                break;

            case "Walls":
                break;

            case "Floors":
                break;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            // 플레이어와 조각 벗어나면 주울수없게
            case "Player":
                isCollectAble = false;
                break;
        }
    }

    // Collecter(Catcher)가 R Shift 누르면 유성조각 획득
    void collectFragment()
    {
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            if (this.isCollectAble == true)
            {
                this.transform.SetAsLastSibling();
                this.gameObject.SetActive(false);
                GameObject.Find("Players").transform.GetChild(0).GetComponent<PlayerScript>().catchFragment();
            }
        }
    }
}
