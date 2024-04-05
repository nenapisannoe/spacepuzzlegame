using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public enum DirectionEnum {NONE, LEFT, RIGHT}
        

    public DirectionEnum direction = DirectionEnum.NONE;
}
