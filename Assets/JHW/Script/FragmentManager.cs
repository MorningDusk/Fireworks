using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentManager : MonoBehaviour
{
    // 게임매니저 싱글톤
    GameManager gm;

    // 기획서에 있는 변수 LifeTime (탄환 변수) , FadeTime (유성 조각 변수)
    float LifeTime = 3f; // 탄환 변수인데 이건 따로 bullet스크립트에서 관리해야 될듯??
    float FadeTime = 4f; // 유성 소멸 시간

    // 조각 랜덤 생성 주기
    private float spawnTime = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;

        // 조각 랜덤위치 생성
        StartCoroutine(randomCreateFragment());
    }

    // 조각 랜덤 생성되는 함수
    IEnumerator randomCreateFragment()
    {
        // 게임오버시 생성 X
        while (gm.getGameOver()==false)
        {
            // 조각 오브젝트 불러오고 활성화
            GameObject frag = GameObject.Find("FragmentPool").transform.GetChild(0).gameObject;
            frag.SetActive(true);

            // 조각 랜덤스폰
            frag.transform.position = new Vector3(Random.Range(-30, 30), 30f, 0f);

            // 조각 스폰 후 4초 뒤 소멸
            StartCoroutine(collectTimeout(frag));

            yield return new WaitForSeconds(spawnTime); // 일정 주기로 조각 생성

            
        }
    }

    // 조각 줍기 유효시간 지나면 소멸되는 함수 (풀링 적용됨)
    IEnumerator collectTimeout(GameObject frag)
    {
        if (frag.activeSelf == false) yield return null; // 이미 주워서 없어진 상태면 실행X

        yield return new WaitForSeconds(FadeTime); // 딜레이 주고 풀링
        frag.SetActive(false);
        frag.transform.SetAsLastSibling();
    }
}
