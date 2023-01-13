using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorManager : SingletonMonoBehaviour<MeteorManager>
{
    public Transform Big_parent, Medium_parent, Small_parent, Active_parent;
    GameObjectPool<Meteor> m_Big, m_Medium, m_Small;
    int n_big, n_medium, n_small;
    [SerializeField]
    int Time, bTime;

    void Start()
    {
        Big_parent = transform.GetChild(0);
        Medium_parent = transform.GetChild(1);
        Small_parent = transform.GetChild(2);
        Active_parent = transform.GetChild(3);

        n_big = 30;
        n_medium = 50;
        n_small = 100;
        bTime = 0;

        Meteor_Pool_Init();

    }


    void Update()
    {
        Time = UIManager.Instance.Get_Time();

        if (Time != bTime)
            Get_Meteor();

    //
    }



    void Meteor_Pool_Init()
    {
        m_Big = new GameObjectPool<Meteor>(n_big, (int n) =>
        {
            var obj = Instantiate(Big_parent.GetChild(0).gameObject);
            obj.SetActive(false);
            obj.transform.SetParent(Big_parent);
            obj.transform.localPosition = Big_parent.localPosition;
            var meteor = obj.GetComponent<Meteor>();
            //meteor.MeteorType = Meteor.Meteor_Type.BIG;
            return meteor;
        });
        m_Medium = new GameObjectPool<Meteor>(n_medium, (int n) =>
        {
            var obj = Instantiate(Medium_parent.GetChild(0).gameObject);
            obj.SetActive(false);
            obj.transform.SetParent(Medium_parent);
            obj.transform.localPosition = Medium_parent.localPosition;
            var meteor = obj.GetComponent<Meteor>();
            //meteor.MeteorType = Meteor.Meteor_Type.MEDIUM;
            return meteor;
        });
        m_Small = new GameObjectPool<Meteor>(n_small, (int n) =>
        {
            var obj = Instantiate(Small_parent.GetChild(0).gameObject);
            obj.SetActive(false);
            obj.transform.SetParent(Small_parent);
            obj.transform.localPosition = Small_parent.localPosition;
            var meteor = obj.GetComponent<Meteor>();
            //meteor.MeteorType = Meteor.Meteor_Type.SMALL;
            return meteor;
        });
    }

    void Get_Meteor()
    {
        System.Random r = new System.Random();
        int n = r.Next(10);

        if (Time % 4 == 0 && Time < 180)
        {
            if (n < 8)
            {
                Transform obj = Small_parent.GetChild(1);
                obj.gameObject.SetActive(true);
                obj.SetParent(Active_parent);
                obj.GetComponent<Meteor>().Meteor_Init();

                Debug.Log(obj + " Active " + Time);
            }
            else
            {
                Transform obj = Medium_parent.GetChild(1);
                obj.gameObject.SetActive(true);
                obj.SetParent(Active_parent);
                obj.GetComponent<Meteor>().Meteor_Init(); 

                Debug.Log(obj + " Active " + Time);
            }
            bTime = Time;
        }
        else if (Time % 3 == 0 && Time < 560)
        {
            if (n < 5)
            {
                ;
            }
            else if (n < 9)
            {

            }
            else
            {

            }
                ;
        }
        else if (Time % 3 == 0)
        {
            if (n < 5)
            {
                ;
            }
            else if (n < 9)
            {

            }
            else
            {

            }

        }
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



