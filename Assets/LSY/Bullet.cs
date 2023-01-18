using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Bullet : MonoBehaviour
{
    BulletManager bulletManager;
    VisualEffect _effect;

    public bool _enalbled = false;
    Transform _bulletball;

    [SerializeField]
    float BulletSpeed;
    [SerializeField]
    Vector3 pos, moveVec;

    void Start()
    {
        _bulletball = transform.GetChild(0);
        bulletManager = BulletManager.Instance;
        _effect = GetComponent<VisualEffect>();
        BulletSpeed = 10.0f;
        moveVec = new Vector3(0f, 1, 0f);
    }

    void Update()
    {
        if (_enalbled)
        {
            //this.transform.Translate(Vector3.up * BulletSpeed * Time.deltaTime);

            transform.position += moveVec * BulletSpeed * Time.deltaTime;
            

            if (_bulletball.position.y > 60f)
            {
                //transform.GetChild(0).position = Vector3.zero;
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

            //this.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;


        }
    }


}
