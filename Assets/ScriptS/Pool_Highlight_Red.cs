using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool_Highlight_Red : Pool_Object
{
    [SerializeField] GameObject Highlight_Red;
    
    public void Start()
    {
        Object = Highlight_Red;
        Quantity = 10;
    }
}