using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class Meteor : MonoBehaviour
{
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
    float MetSpeed = 30.0f;


    bool _enabled = false;

    // Property
    public Meteor_Type MeteorType { get { return _Type; } set { MeteorType = value; } }


    void Start()
    {
        
    }

    void Update()
    {
        if(_enabled)
        {
            this.transform.Translate(new Vector3(-1.0f, -1.0f, 0f) * MetSpeed * Time.deltaTime);
        }

        if (this.transform.localPosition.y < -50.0f)
        {
            switch (_Type)
            {
                case Meteor_Type.BIG:
                    this.transform.SetParent(MeteorManager.Instance.Big_parent);
                    break;
                case Meteor_Type.MEDIUM:
                    this.transform.SetParent(MeteorManager.Instance.Medium_parent);
                    break;
                case Meteor_Type.SMALL:
                    this.transform.SetParent(MeteorManager.Instance.Small_parent);
                    break;
            }

            this.gameObject.SetActive(false);
            this.transform.localPosition = Vector3.zero;
            _enabled = false;
        }
    }

    public void Meteor_Init()
    {
        _enabled= true;
    }


    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log(collision.gameObject + " collision with " + this.gameObject);
    //}

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Player":
                // �÷��̾� HP ����
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
                break;

            case "Floor":
                break;

            case "Wall":
                break;
        }
    }

}
