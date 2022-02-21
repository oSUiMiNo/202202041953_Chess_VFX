using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Pool_Object : MonoBehaviour
{
    public List<GameObject> Pool;
    protected GameObject Object;
    protected int Quantity;
    
    public Pool_Object()
    {   
        Pool = new List<GameObject>();
        Pool_Initialize();
    }



    public virtual void Pool_Initialize()
    {
        for (int i = 0; i < Quantity; i++)
        {
            GameObject Object_Initial = Object_Create();
            Pool.Add(Object_Initial);
        }
    }



    public virtual GameObject Object_Discharge(Vector3 Position)
    {
        foreach (var Object_Existing in Pool)  //戻り値はリターン文なので、返せるオブジェクトを1つ見つけて返した時点でループを抜ける。
        {
            if (Object_Existing.activeSelf == false)
            {
                Object_Existing.SetActive(true);
                Object_Existing.transform.position = Position;

                return Object_Existing;
            }
        }

        GameObject Object_Additional = Object_Create();
        Object_Additional.SetActive(true);
        Pool.Add(Object_Additional);
        Object_Additional.transform.position = Position;

        return Object_Additional;
    }

    

    public virtual GameObject Object_Create()
    {
        GameObject Object_New = Instantiate(Object);
        Object_New.name = Object.name + (Pool.Count + 1);
        Object_New.SetActive(false);
        Object_New.transform.position = new Vector3(10 , 30, 10);

        return Object_New;
    }



    public void Object_Hide()
    {
        foreach(var Object_Existing in Pool)
        {
            Object_Existing.SetActive(false);
        }
    }
}