using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Fragment : MonoBehaviour
{
    GameManager gm;
    // ��ȹ���� �ִ� ���� LifeTime (źȯ ����) , FadeTime (���� ���� ����)
    float LifeTime; 
    float FadeTime = 4f;

    bool isCollect = false;

    // ���� ���� ���� ����
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
            // �÷��̾�� ���� �浹�� �ֿ� �� �ְ�
            case "Player":
                // �÷��̾� �浹�ߴµ� �����ڸ� ����X 
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
            // �÷��̾�� ���� ����� �ֿ������
            case "Player":
                StartCoroutine(changeCollectAble());
                break;
        }
    }

    // �÷��̾ �������� �����(��ó�� ������) ���� �ֿ� �� ���� ����
    IEnumerator changeCollectAble()
    {
        yield return new WaitForSeconds(0.1f);
        isCollectAble = false;
    }

    // Collecter(Catcher)�� R Shift ������ �������� ȹ��
    void collectFragment()
    {
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            // �Ÿ��� ���� ������ ȹ��... �� RayCast ��ߵǴµ� �����ϴµ� �ð��ɷ��� ��ŵ..
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
