using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorManager : MonoBehaviour
{
    Transform Big_parent, Medium_parent, Small_parent;
    GameObjectPool<Meteor> m_Big, m_Medium, m_Small;
    int n_big, n_medium, n_small;

    void Start()
    {
        Big_parent = transform.GetChild(0);
        Medium_parent = transform.GetChild(1);
        Small_parent = transform.GetChild(2);

        n_big = 30;
        n_medium = 50;
        n_small = 100;

        Meteor_Pool_Init();

    }


    void Update()
    {
        
    }



    void Meteor_Pool_Init()
    {
        m_Big = new GameObjectPool<Meteor>(n_big, (int n) =>
        {
            var obj = Instantiate(Big_parent.GetChild(0).gameObject);
            obj.SetActive(false);
            obj.transform.SetParent(Big_parent);
            var meteor = obj.GetComponent<Meteor>();
            //meteor.MeteorType = Meteor.Meteor_Type.BIG;
            return meteor;
        });
        m_Medium = new GameObjectPool<Meteor>(n_medium, (int n) =>
        {
            var obj = Instantiate(Medium_parent.GetChild(0).gameObject);
            obj.SetActive(false);
            obj.transform.SetParent(Medium_parent);
            var meteor = obj.GetComponent<Meteor>();
            //meteor.MeteorType = Meteor.Meteor_Type.MEDIUM;
            return meteor;
        });
        m_Small = new GameObjectPool<Meteor>(n_small, (int n) =>
        {
            var obj = Instantiate(Small_parent.GetChild(0).gameObject);
            obj.SetActive(false);
            obj.transform.SetParent(Small_parent);
            var meteor = obj.GetComponent<Meteor>();
            //meteor.MeteorType = Meteor.Meteor_Type.SMALL;
            return meteor;
        });
    }


}


public class GameObjectPool<T> where T : class
{
    int count;
    public delegate T Func(int i);
    Func create_fn;
    // Instances.  
    Stack<T> objects;
    // Construct  
    public GameObjectPool(int count, Func fn)
    {
        this.count = count;
        this.create_fn = fn;
        this.objects = new Stack<T>(this.count);
        allocate();

    }
    void allocate()
    {
        for (int i = 0; i < this.count; ++i)
        {
            this.objects.Push(this.create_fn(i));
        }
    }
    public T pop()
    {
        if (this.objects.Count <= 0)
        {
            Debug.Log(this + " allocate more");
            allocate();
        }
        return this.objects.Pop();
    }
    public void push(T obj)
    {
        this.objects.Push(obj);
    }
    public int get_Count()
    {
        return objects.Count;
    }
    public Stack<T> get_Stack()
    {
        return objects;
    }

}
