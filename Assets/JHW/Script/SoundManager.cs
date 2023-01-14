using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    // ���� ���Ƕ�
    // �Ʒ� public���� ������ �͵� �� �̱������� ���� ���� �� �ֽ��ϴ�
    // ex :
    // SoundManager.Instance.playBGM("���"); // ��� ���
    // SoundManager.Instance.playSFX("����"); // SFX ���

    [SerializeField] Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    [SerializeField] private float volume_BGM = 1f;
    [SerializeField] private float volume_SFX = 1f; 

    private enum SoundType {
        BGM,
        SFX,
    }


    // ���� �Ǵ� ȿ���� �ҷ����� �Ǵ� ����
    AudioClip GetOrAddAudioClip(string name, SoundType st)
    {
        AudioClip audioClip = null;
        

        // ������� ã�´�
        if(st == SoundType.BGM)
        {
            // ����� Ŭ�� ������ ��ųʸ��� ���̱�
            if (_audioClips.TryGetValue(name, out audioClip) == false)
            {
                // ����� �ҷ����� ��ųʸ� ����
                audioClip = Resources.Load<AudioClip>("Sounds/BGM/" + name);
                _audioClips.Add(name, audioClip);
            }

            // ã�� ���ϸ� ����
            if (audioClip == null)
                Debug.Log($"AudioClip Missing ! {name}");
        }

        // ȿ������ ã�´�
        else if (st == SoundType.SFX)
        {
            // ȿ���� Ŭ�� ������ ��ųʸ��� ���̱�
            if (_audioClips.TryGetValue(name, out audioClip) == false)
            {
                // ȿ���� �ҷ����� ��ųʸ� ����
                audioClip = Resources.Load<AudioClip>("Sounds/SFX/" + name);
                _audioClips.Add(name, audioClip);
            }

            // ã�� ���ϸ� ����
            if (audioClip == null)
                Debug.Log($"AudioClip Missing ! {name}");
        }

        return audioClip;
    }
    
    // ���� ��� - �����
    public void playBGM(string soundName)
    {
        // ���� ��ü
        GameObject soundObject = null;

        // ���� 
        soundObject = new GameObject("Sound");
        soundObject.transform.parent = GameObject.Find("SoundManager/BGM").transform;
        AudioSource audioSource = soundObject.AddComponent<AudioSource>(); // ������Ʈ ����
        audioSource.clip = GetOrAddAudioClip(soundName, SoundType.BGM); // ���� �ҷ�����
        audioSource.loop = true; // �ݺ����
        audioSource.Play(); // ���� ���
    }

    // ���� ���� - �����
    public void pauseBGM()
    {
        // ���� ��ü �ҷ�����
        GameObject soundObject = GameObject.Find("SoundManager/BGM");
        if (soundObject == null) return;
        soundObject.GetComponent<AudioSource>().Pause();
    }

    // ���� �ߴ� - �����
    public void stopBGM()
    {
        // ���� ��ü �ҷ�����
        GameObject soundObject = GameObject.Find("SoundManager/BGM");
        if (soundObject == null) return;
        soundObject.GetComponent<AudioSource>().Stop();
    }


    // ���� ��� - ȿ���� (Ǯ�� �����)
    public void playSFX(string soundName)
    {
        // ȿ���� Ǯ ������ �����ؼ� �Ŵ����� ����
        GameObject soundPool = GameObject.Find(soundName + "Pool");
        if (soundPool == null)
        {
            soundPool = new GameObject(soundName + "Pool");
            soundPool.transform.parent = GameObject.Find("SoundManager/SFX").transform;
        }

        // ���� ������Ʈ ����
        GameObject soundObject;

        // ���� Ǯ�� ������Ʈ�� ������ ���� ����� Ǯ�� ����
        if (soundPool.transform.childCount==0)
        {
            // ���� ������Ʈ ����
            soundObject = new GameObject(soundName + "Sound");
            soundObject.transform.parent = soundPool.transform; // ����Ǯ�� ����
            AudioSource audioSource = soundObject.AddComponent<AudioSource>(); // ������Ʈ ����
            audioSource.clip = GetOrAddAudioClip(soundName, SoundType.SFX);
            soundObject.SetActive(false);
        }
        // ���� ������Ʈ ����
        int idx = 0;
        while(idx<soundPool.transform.childCount)
        {
            // ��������� ���� ���� ������Ʈ �� ������ idx ����
            if (soundPool.transform.GetChild(idx).gameObject.activeSelf == true) idx++;
            else break;
        }
        // ���� idx�� pool�� �ִ� �ε������� �����ϸ� ������Ʈ ���� ����� ����
        if (idx==soundPool.transform.childCount)
        {
            // ���� ������Ʈ ����
            soundObject = new GameObject(soundName + "Sound");
            soundObject.transform.parent = soundPool.transform; // ����Ǯ�� ����
            AudioSource audioSource = soundObject.AddComponent<AudioSource>(); // ������Ʈ ����
            audioSource.clip = GetOrAddAudioClip(soundName, SoundType.SFX);
            soundObject.SetActive(false);
        }
            
        // ���������Ʈ ������Ʈ ���� �� Ȱ��ȭ
        soundObject = soundPool.transform.GetChild(idx).gameObject;
        soundObject.SetActive(true);

        // ���� ��� (PlayOneshot)
        soundObject.GetComponent<AudioSource>().PlayOneShot(GetOrAddAudioClip(soundName, SoundType.SFX),volume_SFX);

        // ���� �� ����Ǹ� ��Ȱ��ȭ
        StartCoroutine(soundSetActive(soundObject));

    }

    IEnumerator soundSetActive(GameObject soundObject)
    {
        // �����ð� �� ���� ������Ʈ ��Ȱ��ȭ
        yield return new WaitForSeconds(soundObject.GetComponent<AudioSource>().clip.length);

        soundObject.SetActive(false);
        soundObject.transform.SetAsFirstSibling(); // �ڽ� ������ �̵�
        StopCoroutine(soundSetActive(null));
    } 

    // ��� ��������
    public void BGM_volumeControl(float volume)
    {
        volume_BGM = volume;

        // ������Ʈ �ҷ��ͼ� ���� ����
        GameObject soundObject = GameObject.Find("SoundManager/BGM").transform.GetChild(0).gameObject;
        AudioSource audioSource = soundObject.GetComponent<AudioSource>();
        audioSource.volume = volume_BGM;
    }

    // ȿ���� ��������
    public void SFX_volumeControl(float volume)
    {
        volume_SFX = volume;

        // SFX Ǯ�� �ִ� ����� �ҷ��ͼ� ���� ����
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