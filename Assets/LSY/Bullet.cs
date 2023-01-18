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
        _effect.enabled= true;
        BulletSpeed = 6.0f;
        moveVec = new Vector3(0f, 1f, 0f);
    }

    void Update()
    {
        if (_enalbled)
        {
            if (transform.position.y <= 30f)
                transform.position += moveVec * BulletSpeed * Time.deltaTime;
            

            else if (transform.position.y > 30f)
            {
                //transform.GetChild(0).position = Vector3.zero;
                _bulletball.gameObject.SetActive(true);
                this.GetComponent<SphereCollider>().enabled = false;
                //bulletManager.Bullet_pushback(this);
                _effect.enabled = false;
                _enalbled=false;
                Debug.Log("Invoke A");
                Invoke("Invoke_pushback", 4.0f);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Meteor"))
        {
            Debug.Log(gameObject + " " + other.gameObject + " Trigger enter");
            this.GetComponent<SphereCollider>().enabled = false;
            _enalbled = false;
            _effect.enabled= false;
            _bulletball.gameObject.SetActive(true);

            Debug.Log("Invoke B");
            Invoke("Invoke_pushback", 4.0f);

            other.GetComponent<Meteor>().Meteor_Split();

            //this.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;

        }
    }

    private void Invoke_pushback()
    {
        this.GetComponent<SphereCollider>().enabled = true;
        _effect.enabled = false;

        _bulletball.gameObject.SetActive(false);
        bulletManager.Bullet_pushback(this);
    }


}
