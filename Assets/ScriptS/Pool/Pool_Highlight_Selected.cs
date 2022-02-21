using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool_Highlight_Selected : Pool_Object
{
    [SerializeField] GameObject Highlight_Selected;

    public void Start()
    {
        Object = Highlight_Selected;
        Quantity = 25;
    }
}