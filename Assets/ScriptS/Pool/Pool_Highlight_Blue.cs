using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool_Highlight_Blue : Pool_Object
{
    [SerializeField] GameObject Highlight_Blue;
    
    public void Start()
    {
        Object = Highlight_Blue;
        Quantity = 30;
    }
}