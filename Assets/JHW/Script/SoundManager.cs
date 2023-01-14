using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    // 사운드 쓰실때
    // 아래 public으로 선언한 것들 다 싱글턴으로 빼서 쓰실 수 있습니다
    // ex :
    // SoundManager.Instance.playBGM("브금"); // 브금 재생
    // SoundManager.Instance.playSFX("점프"); // SFX 재생

    [SerializeField] Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    [SerializeField] private float volume_BGM = 1f;
    [SerializeField] private float volume_SFX = 1f; 

    private enum SoundType {
        BGM,
        SFX,
    }


    // 음악 또는 효과음 불러오기 또는 저장
    AudioClip GetOrAddAudioClip(string name, SoundType st)
    {
        AudioClip audioClip = null;
        

        // 배경음을 찾는다
        if(st == SoundType.BGM)
        {
            // 배경음 클립 없으면 딕셔너리에 붙이기
            if (_audioClips.TryGetValue(name, out audioClip) == false)
            {
                // 배경음 불러오고 딕셔너리 저장
                audioClip = Resources.Load<AudioClip>("Sounds/BGM/" + name);
                _audioClips.Add(name, audioClip);
            }

            // 찾지 못하면 에러
            if (audioClip == null)
                Debug.Log($"AudioClip Missing ! {name}");
        }

        // 효과음을 찾는다
        else if (st == SoundType.SFX)
        {
            // 효과음 클립 없으면 딕셔너리에 붙이기
            if (_audioClips.TryGetValue(name, out audioClip) == false)
            {
                // 효과음 불러오고 딕셔너리 저장
                audioClip = Resources.Load<AudioClip>("Sounds/SFX/" + name);
                _audioClips.Add(name, audioClip);
            }

            // 찾지 못하면 에러
            if (audioClip == null)
                Debug.Log($"AudioClip Missing ! {name}");
        }

        return audioClip;
    }
    
    // 사운드 재생 - 배경음
    public void playBGM(string soundName)
    {
        // 사운드 객체
        GameObject soundObject = null;

        // 사운드 
        soundObject = new GameObject("Sound");
        soundObject.transform.parent = GameObject.Find("SoundManager/BGM").transform;
        AudioSource audioSource = soundObject.AddComponent<AudioSource>(); // 컴포넌트 생성
        audioSource.clip = GetOrAddAudioClip(soundName, SoundType.BGM); // 음악 불러오기
        audioSource.loop = true; // 반복재생
        audioSource.Play(); // 음악 재생
    }

    // 사운드 정지 - 배경음
    public void pauseBGM()
    {
        // 사운드 객체 불러오기
        GameObject soundObject = GameObject.Find("SoundManager/BGM");
        if (soundObject == null) return;
        soundObject.GetComponent<AudioSource>().Pause();
    }

    // 사운드 중단 - 배경음
    public void stopBGM()
    {
        // 사운드 객체 불러오기
        GameObject soundObject = GameObject.Find("SoundManager/BGM");
        if (soundObject == null) return;
        soundObject.GetComponent<AudioSource>().Stop();
    }


    // 사운드 재생 - 효과음 (풀링 적용됨)
    public void playSFX(string soundName)
    {
        // 효과음 풀 없으면 생성해서 매니저에 붙임
        GameObject soundPool = GameObject.Find(soundName + "Pool");
        if (soundPool == null)
        {
            soundPool = new GameObject(soundName + "Pool");
            soundPool.transform.parent = GameObject.Find("SoundManager/SFX").transform;
        }

        // 사운드 오브젝트 선택
        GameObject soundObject;

        // 사운드 풀에 오브젝트가 없으면 새로 만들고 풀에 넣자
        if (soundPool.transform.childCount==0)
        {
            // 사운드 오브젝트 생성
            soundObject = new GameObject(soundName + "Sound");
            soundObject.transform.parent = soundPool.transform; // 사운드풀에 저장
            AudioSource audioSource = soundObject.AddComponent<AudioSource>(); // 컴포넌트 생성
            audioSource.clip = GetOrAddAudioClip(soundName, SoundType.SFX);
            soundObject.SetActive(false);
        }
        // 사운드 오브젝트 선택
        int idx = 0;
        while(idx<soundPool.transform.childCount)
        {
            // 사용중이지 않은 사운드 오브젝트 고를 때까지 idx 증가
            if (soundPool.transform.GetChild(idx).gameObject.activeSelf == true) idx++;
            else break;
        }
        // 만약 idx가 pool의 최대 인덱스까지 도착하면 오브젝트 새로 만들고 저장
        if (idx==soundPool.transform.childCount)
        {
            // 사운드 오브젝트 생성
            soundObject = new GameObject(soundName + "Sound");
            soundObject.transform.parent = soundPool.transform; // 사운드풀에 저장
            AudioSource audioSource = soundObject.AddComponent<AudioSource>(); // 컴포넌트 생성
            audioSource.clip = GetOrAddAudioClip(soundName, SoundType.SFX);
            soundObject.SetActive(false);
        }
            
        // 사운드오브젝트 오브젝트 선택 후 활성화
        soundObject = soundPool.transform.GetChild(idx).gameObject;
        soundObject.SetActive(true);

        // 사운드 재생 (PlayOneshot)
        soundObject.GetComponent<AudioSource>().PlayOneShot(GetOrAddAudioClip(soundName, SoundType.SFX),volume_SFX);

        // 사운드 다 재생되면 비활성화
        StartCoroutine(soundSetActive(soundObject));

    }

    IEnumerator soundSetActive(GameObject soundObject)
    {
        // 지연시간 뒤 사운드 오브젝트 비활성화
        yield return new WaitForSeconds(soundObject.GetComponent<AudioSource>().clip.length);

        soundObject.SetActive(false);
        soundObject.transform.SetAsFirstSibling(); // 자식 앞으로 이동
        StopCoroutine(soundSetActive(null));
    } 

    // 배경 볼륨조절
    public void BGM_volumeControl(float volume)
    {
        volume_BGM = volume;

        // 컴포넌트 불러와서 볼륨 조절
        GameObject soundObject = GameObject.Find("SoundManager/BGM").transform.GetChild(0).gameObject;
        AudioSource audioSource = soundObject.GetComponent<AudioSource>();
        audioSource.volume = volume_BGM;
    }

    // 효과음 볼륨조절
    public void SFX_volumeControl(float volume)
    {
        volume_SFX = volume;

        // SFX 풀에 있는 사운드들 불러와서 볼륨 조절
        GameObject soundPools = GameObject.Find("SoundManager/SFX");

        for(int i=0;i< soundPools.transform.childCount; i++)
        {
            GameObject soundPool = soundPools.transform.GetChild(i).gameObject;
            for(int j = 0; j < soundPool.transform.childCount; j++)
            {
                GameObject soundObject = soundPool.transform.GetChild(j).gameObject;
                AudioSource audioSource = soundObject.GetComponent<AudioSource>();
                audioSource.volume = volume_SFX;
            }
        }
    }
}