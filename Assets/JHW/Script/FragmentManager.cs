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
    private float spawnTime = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;

        // ���� ������ġ ����
        StartCoroutine(randomCreateFragment());
    }

    // ���� ���� �����Ǵ� �Լ�
    IEnumerator randomCreateFragment()
    {
        // ���ӿ����� ���� X
        while (gm.getGameOver()==false)
        {
            // ���� ������Ʈ �ҷ����� Ȱ��ȭ
            GameObject frag = GameObject.Find("FragmentPool").transform.GetChild(0).gameObject;
            frag.SetActive(true);

            // ���� ��������
            frag.transform.position = new Vector3(Random.Range(-30, 30), 30f, 0f);

            // ���� ���� �� 4�� �� �Ҹ�
            StartCoroutine(collectTimeout(frag));

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
