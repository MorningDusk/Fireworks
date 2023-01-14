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
    [SerializeField]
    bool stopVar = false;

    void Update()
    {
        Time = UIManager.Instance.Get_Time();

        if (Time != bTime)
            if (!stopVar)
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
            string obj_name = obj.name;
            obj.name = obj_name + "_#" + n;
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
            string obj_name = obj.name;
            obj.name = obj_name + "_#" + n;
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
            string obj_name = obj.name;
            obj.name = obj_name + "_#" + n;
            var meteor = obj.GetComponent<Meteor>();
            //meteor.MeteorType = Meteor.Meteor_Type.SMALL;
            return meteor;
        });
    }

    void Get_Meteor()
    {
        System.Random r = new System.Random();

        if (Time < 180)
        {
            int n = r.Next(10);
            if (Time % 2 == 0)
            {
                if (n < 7)
                    Meteor_pop(m_Small);
                else
                    Meteor_pop(m_Medium);
            }
            bTime = Time;
        }
        else if (Time < 480)
        {
            int n = r.Next(10);
            if (Time % 2 == 0)
            {
                if (n < 7)
                    Meteor_pop(m_Small);
                else
                    Meteor_pop(m_Medium);
            }

            int n2 = r.Next(10);
            if (Time % 3 == 0)
            {
                if (n2 < 5)
                    Meteor_pop(m_Small);
                else if (n2 < 8)
                    Meteor_pop(m_Medium);
                else
                    Meteor_pop(m_Big);
            }
            bTime = Time;
        }
        else
        {
            int n = r.Next(10);
            if (n < 5)
                Meteor_pop(m_Small);
            else if (n < 8)
                Meteor_pop(m_Medium);
            else
                Meteor_pop(m_Big);

            int n2 = r.Next(10);
            if (Time % 2 == 0)
            {
                if (n2 < 5)
                    Meteor_pop(m_Small);
                else if (n2 < 8)
                    Meteor_pop(m_Medium);
                else
                    Meteor_pop(m_Big);
            }
            bTime = Time;

        }
    }

    public void Meteor_pop(GameObjectPool<Meteor> m_meteor)
    {
        Transform obj = m_meteor.pop().transform;
        obj.gameObject.SetActive(true);
        obj.SetParent(Active_parent);
        // 0~30 50% ±×¿Ü 50%   -30~60
        if (Random.Range(0, 10) / 3 == 0)
            obj.transform.position = new Vector3(Random.Range(0, 30), 60f, transform.position.z);
        else
            obj.transform.position = new Vector3(Random.Range(-30, 60), 60f, transform.position.z);

        //Debug.Log(obj + " " + obj.transform.position);
        obj.GetComponent<Meteor>().Meteor_Init();
    }

    public void Meteor_pop(Meteor.Meteor_Type _type, Vector3 pos)
    {
        // Overloading, use after split

        //Debug.Log(_type + " , " + pos);

        switch (_type)
        {
            case Meteor.Meteor_Type.BIG:
                for (int i = 0; i < 2; i++)
                {
                    Transform obj = m_Medium.pop().transform;
                    obj.gameObject.SetActive(true);
                    obj.SetParent(Active_parent);
                    obj.transform.position = pos;
                    obj.GetComponent<Meteor>().MetSpeed *= 0.75f;
                    obj.GetComponent<Meteor>().Meteor_Init(1);
                }
                break;
            case Meteor.Meteor_Type.MEDIUM:
                for (int i = 0; i < 2; i++)
                {
                    Transform obj = m_Small.pop().transform;
                    obj.gameObject.SetActive(true);
                    obj.SetParent(Active_parent);
                    obj.transform.position = pos;
                    obj.GetComponent<Meteor>().MetSpeed *= 0.75f;
                    obj.GetComponent<Meteor>().Meteor_Init(1);
                }

                break;
            case Meteor.Meteor_Type.SMALL:
                ;
                break;
        }
        
    }

    public void Meteor_pushback(Meteor meteor)
    {
        meteor.gameObject.SetActive(false);
        meteor.transform.localPosition = Vector3.zero;
        meteor._enabled = false;
        meteor._split = false;
        switch(meteor.MeteorType)
        {
            case Meteor.Meteor_Type.BIG:
                meteor.transform.SetParent(Big_parent);
                m_Big.push(meteor);
                break;
            case Meteor.Meteor_Type.MEDIUM:
                meteor.transform.SetParent(Medium_parent);
                m_Medium.push(meteor);
                break;
            case Meteor.Meteor_Type.SMALL:
                meteor.transform.SetParent(Small_parent);
                m_Small.push(meteor);
                break;
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



