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
            this.transform.Translate(new Vector3(-3.0f, -3.0f, 0f) * 10.0f * Time.deltaTime);
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
                break;

            case "Floor":
                break;

            case "Wall":
                break;
        }
    }

}
