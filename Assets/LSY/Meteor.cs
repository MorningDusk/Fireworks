using System.Collections;
using System.Collections.Generic;
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

    // Property
    public Meteor_Type MeteorType { get { return _Type; } set { MeteorType = value; } }


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
