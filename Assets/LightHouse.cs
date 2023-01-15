using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightHouse : SingletonMonoBehaviour<LightHouse>
{
    [SerializeField]
    int Durability, Max_Durability = 10;

    public int get_Durability() {return Durability;}
    public int get_MaxDurability() {return Max_Durability;}
    void Start()
    {
        Durability = Max_Durability;
    }

    void Update()
    {
        
    }

    public void DecreaseHealth(int Value)
    {
        Durability -= Value;
    
    }

}
