using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool_Highlight_Yellow : Pool_Object
{
    [SerializeField] GameObject Highlight_Yellow;
    
    public void Start()
    {
        Object = Highlight_Yellow;
        Quantity = 25;
    }
}