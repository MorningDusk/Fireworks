using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : SingletonMonoBehaviour<BulletManager>
{
    GameManager gm;
    GameObjectPool<Bullet> m_bullet;
    Transform Bullet_Pool, Active_Bullet;
    int bulletCount = 20;
    void Start()
    {
        gm = GameManager.Instance;
        Bullet_Pool = transform.GetChild(0);
        //Active_Bullet = GameObject.Find("ActiveBullet").transform;
        Active_Bullet= transform.GetChild(1);
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
            //obj.transform.localPosition = Bullet_Pool.localPosition;
            //obj.transform.position = Bullet_Pool.position;
            obj.transform.localPosition = Vector3.zero;
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
        tmp.transform.position = gm.PlayerManager.GetChild(0).position; // Shooter position
        tmp.transform.GetChild(0).position = new Vector3(tmp.transform.position.x, tmp.transform.position.y + 5.7f, tmp.transform.position.z);
        tmp.gameObject.SetActive(true);
        //tmp.GetComponent<SphereCollider>().enabled = true;
        tmp.GetComponent<Bullet>()._enalbled = true;
    }

    public void Bullet_pushback(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        //bullet.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        bullet.transform.SetParent(Bullet_Pool);
        //bullet.transform.localPosition = Bullet_Pool.localPosition;
        bullet.transform.position = Bullet_Pool.position;
        bullet.transform.GetChild(0).position = bullet.transform.position;
        m_bullet.push(bullet);
        
    }
}
