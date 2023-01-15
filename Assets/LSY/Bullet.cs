using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Bullet : MonoBehaviour
{
    BulletManager bulletManager;
    public bool _enalbled = false;
    bool _first = true;
    VisualEffect _effect;

    [SerializeField]
    float BulletSpeed;
    [SerializeField]
    Vector3 pos;

    void Start()
    {
        bulletManager = BulletManager.Instance;
        _effect = GetComponent<VisualEffect>();
        BulletSpeed = 13.0f;
        pos = transform.position;
    }

    void Update()
    {
        if (_enalbled)
        {
            //if (_first)
            //    Debug.Log("Bullet:" + transform.localPosition + ", Player:" + transform.parent.parent.GetChild(0).localPosition);
            //_first= false;

            //this.transform.Translate(new Vector3(0f, 1f, 0f) * BulletSpeed * Time.deltaTime);
            this.transform.Translate(Vector3.up * BulletSpeed * Time.deltaTime);
            if (transform.localPosition.y > 60f)
            {
                bulletManager.Bullet_pushback(this);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Meteor"))
        {
            Debug.Log(gameObject + " " + other.gameObject + " Trigger enter");
            this.GetComponent<SphereCollider>().enabled = false;
            other.GetComponent<Meteor>().Meteor_Split();

            this.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;


        }
    }


}
