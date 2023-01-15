using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : SingletonMonoBehaviour<BulletManager>
{
    GameObjectPool<Bullet> m_bullet;
    Transform Bullet_Pool, Active_Bullet;
    int bulletCount = 20;
    void Start()
    {
        Bullet_Pool = transform.GetChild(0);
        Active_Bullet = GameObject.Find("ActiveBullet").transform;
        Bullet_Pool_Init();
    }

    void Update()
    {

    }

    void Bullet_Pool_Init()
    {
        m_bullet = new GameObjectPool<Bullet>(bulletCount, (int n) =>
        {
            var obj = Instantiate(Bullet_Pool.GetChild(0).gameObject);
            obj.SetActive(false);
            obj.transform.SetParent(Bullet_Pool);
            obj.transform.localPosition = Bullet_Pool.localPosition;
            string obj_name = obj.name;
            obj.name = obj_name + "_#" + n;
            var bullet = obj.GetComponent<Bullet>();
            return bullet;
        });
    }

    public void Attack()
    {
        Bullet tmp = m_bullet.pop();
        tmp.transform.SetParent(Active_Bullet);
        tmp.gameObject.SetActive(true);
        tmp.GetComponent<SphereCollider>().enabled = true;
        tmp.GetComponent<Bullet>()._enalbled = true;
    }

    public void Bullet_pushback(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        bullet.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        bullet.transform.SetParent(Bullet_Pool);
        bullet.transform.localPosition = Bullet_Pool.localPosition;
        m_bullet.push(bullet);
        
    }
}
