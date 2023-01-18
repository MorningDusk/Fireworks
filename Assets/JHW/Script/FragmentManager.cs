using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentManager : MonoBehaviour
{
    // ���ӸŴ��� �̱���
    GameManager gm;

    // ��ȹ���� �ִ� ���� LifeTime (źȯ ����) , FadeTime (���� ���� ����)
    float LifeTime = 3f; // źȯ �����ε� �̰� ���� bullet��ũ��Ʈ���� �����ؾ� �ɵ�??
    float FadeTime = 4f; // ���� �Ҹ� �ð�

    // ���� ���� ���� �ֱ�
    private float spawnTime = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
    }

    // ���� ���� �����Ǵ� �Լ�
    public IEnumerator randomCreateFragment()
    {
        // ���ӿ����� ���� X
        while (gm.getGameOver()==false)
        {
            // ���� ������Ʈ �ҷ����� Ȱ��ȭ
            //GameObject frag = GameObject.Find("FragmentPool").transform.GetChild(0).gameObject;
            int idx = 0;
            while (true) {
                if (this.transform.GetChild(idx).gameObject.activeSelf == true) idx++;
                else break;
            }
            GameObject frag = this.transform.GetChild(idx).gameObject;

            frag.SetActive(true);

            // ���� ��������
            frag.transform.position = new Vector3(Random.Range(-30, 30), 30f, transform.position.z);

            // ���� ���� �� 4�� �� �Ҹ�
            StartCoroutine(collectTimeout(frag));

            // ���� �����ð� ������
            spawnTime = Random.Range(1.0f, 2.0f);

            yield return new WaitForSeconds(spawnTime); // ���� �ֱ�� ���� ����
        }
    }

    // ���� �ݱ� ��ȿ�ð� ������ �Ҹ�Ǵ� �Լ� (Ǯ�� �����)
    IEnumerator collectTimeout(GameObject frag)
    {
        if (frag.activeSelf == false) yield return null; // �̹� �ֿ��� ������ ���¸� ����X

        yield return new WaitForSeconds(FadeTime); // ������ �ְ� Ǯ��
        frag.SetActive(false);
        frag.transform.SetAsLastSibling();
    }
}
