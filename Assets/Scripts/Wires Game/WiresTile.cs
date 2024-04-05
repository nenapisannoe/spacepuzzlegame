using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiresTile : MonoBehaviour
{

    public int tileID;
    public Action<WiresTile> onTileSelected;
    public bool isPressed;
    
    void OnMouseDown()
    {
        isPressed = true;
        InvokeOnSelected();
    }
    
    void OnMouseUp()
    {
        isPressed = false;
        InvokeOnSelected();
    }

    void InvokeOnSelected()
    {
        if (onTileSelected != null) onTileSelected.Invoke(this);
    }
    
    
}
