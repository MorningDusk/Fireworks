using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Fragment : MonoBehaviour
{
    GameManager gm;
    // 기획서에 있는 변수 LifeTime (탄환 변수) , FadeTime (유성 조각 변수)
    float LifeTime; 
    float FadeTime = 4f;

    bool isCollect = false;

    // 조각 수집 가능 여부
    private bool isCollectAble = false;

    // getter
    public bool getCollectAble() { return isCollectAble; }

    // setter
    public void setCollectAble(bool flag) { isCollectAble = flag; }

    private void Awake()
    {
        gm = GameManager.Instance;
    }

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
                StartCoroutine(changeCollectAble());
                break;
        }
    }

    // 플레이어가 유성조각 벗어나면(근처에 없으면) 조각 주울 수 없게 변경
    IEnumerator changeCollectAble()
    {
        yield return new WaitForSeconds(0.1f);
        isCollectAble = false;
    }

    // Collecter(Catcher)가 R Shift 누르면 유성조각 획득
    void collectFragment()
    {
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            // 거리에 따른 별조각 획득... 은 RayCast 써야되는데 삽질하는데 시간걸려서 스킵..
            //float distance = Vector3.Distance(this.transform.position, gm.PlayerManager.transform.GetChild(1).position);
            //if (distance < 2) isCollectAble = true;
            if (this.isCollectAble == true)
            {
                this.transform.SetAsLastSibling();
                this.gameObject.SetActive(false);
                this.isCollectAble = false;
                gm.PlayerManager.GetChild(0).GetComponent<PlayerScript>().catchFragment();
                gm.PlayerManager.GetChild(1).GetComponent<PlayerScript>().Anim.SetTrigger("doCollect");
            }
        }
    }

    //
}
