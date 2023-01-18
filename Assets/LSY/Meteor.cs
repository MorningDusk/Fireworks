using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using System.Transactions;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Meteor : MonoBehaviour
{
    MeteorManager meteorManager;
    public enum Meteor_Type
    {
        BIG,
        MEDIUM,
        SMALL,
        END
    }
    [SerializeField]
    Meteor_Type _Type;

    [SerializeField]
    public float MetSpeed;

    int Damage = 0;


    public bool _enabled = false, _split = false;
    // Property
    public Meteor_Type MeteorType { get { return _Type; } set { MeteorType = value; } }


    void Start()
    {
        switch(_Type)
        {
            case Meteor_Type.BIG:
                Damage = 3;
                break;
            case Meteor_Type.MEDIUM:
                Damage = 2;
                break;
            case Meteor_Type.SMALL:
                Damage = 1;
                break;
        }
    }
    private void Awake()
    {
        meteorManager = MeteorManager.Instance;
        MetSpeed = 10.0f;
        System.Random r = new System.Random();
        int n = r.Next(7, 15);
        MetSpeed *= (float)n / 10.0f;
    }

    void Update()
    {
        if(_split)
        {
            float angle = Random.Range(-0.5f, 0.5f);
            this.transform.Translate(new Vector3(angle, -1.0f, 0f) * MetSpeed * Time.deltaTime);

        }
        else if(_enabled)
        {
            this.transform.Translate(new Vector3(-0.3f, -1.0f, 0f) * MetSpeed * Time.deltaTime);
        }

        if (this.transform.localPosition.y < -100.0f)
        {
            //switch (_Type)
            //{
            //    case Meteor_Type.BIG:
            //        this.transform.SetParent(MeteorManager.Instance.Big_parent);
            //        break;
            //    case Meteor_Type.MEDIUM:
            //        this.transform.SetParent(MeteorManager.Instance.Medium_parent);
            //        break;
            //    case Meteor_Type.SMALL:
            //        this.transform.SetParent(MeteorManager.Instance.Small_parent);
            //        break;
            //}

            //this.gameObject.SetActive(false);
            //this.transform.localPosition = Vector3.zero;
            //_enabled = false;
            meteorManager.Meteor_pushback(this);
        }
    }

    public void Meteor_Init()
    {
        _enabled= true;
    }
    public void Meteor_Init(int i)
    {
        _enabled = true;
        _split = true;
    }


    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log(collision.gameObject + " collision with " + this.gameObject);
    //}

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("[Meteor] TriggerEnter " + other.gameObject);
        switch (other.tag)
        {
            case "Player":
                if (GameManager.Instance.getGameOver()) return;

                switch (this.MeteorType)
                {
                    case Meteor_Type.BIG:
                        other.GetComponent<PlayerScript>().DecreaseHealth(3);
                        break;

                    case Meteor_Type.MEDIUM:
                        other.GetComponent<PlayerScript>().DecreaseHealth(2);
                        break;

                    case Meteor_Type.SMALL:
                        other.GetComponent<PlayerScript>().DecreaseHealth(1);
                        break;
                }
                // �÷��̾� HP ���ҿ� ���� UI ����
                UIManager.Instance.UI_changeHP(other.GetComponent<PlayerScript>().GetPlayerType());
                UIManager.Instance.UI_changeHP_Lighthouse();
                break;

            case "Floor":

                meteorManager.Meteor_pushback(this);



                break;

            case "Wall":

                meteorManager.Meteor_pushback(this);


                break;

            case "LightHouse":
                meteorManager.Meteor_pushback(this);
                other.GetComponent<LightHouse>().DecreaseHealth(Damage);


                break;
        }
    }


    public void Meteor_Split()
    {
        Vector3 pos;
        switch (_Type)
        {
            case Meteor_Type.BIG:
                pos = this.transform.position;
                //meteorManager.Meteor_pushback(this);
                meteorManager.Meteor_pop(Meteor_Type.BIG, pos);
                break;

            case Meteor_Type.MEDIUM:
                //Debug.Log(this + " Split");
                pos = this.transform.position;
                //meteorManager.Meteor_pushback(this);
                meteorManager.Meteor_pop(Meteor_Type.MEDIUM, pos);
                break;

            case Meteor_Type.SMALL:
                //meteorManager.Meteor_pushback(this);
                break;

        }
    }

}
