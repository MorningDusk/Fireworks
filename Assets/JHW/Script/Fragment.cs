using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fragment : MonoBehaviour
{

    // ��ȹ���� �ִ� ���� LifeTime (źȯ ����) , FadeTime (���� ���� ����)
    float LifeTime; 
    float FadeTime = 4f;

    // ���� ���� ���� ����
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
                isCollectAble = false;
                break;
        }
    }

    // Collecter(Catcher)�� R Shift ������ �������� ȹ��
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
