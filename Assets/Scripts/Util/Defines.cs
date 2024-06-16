using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defines
{
    public enum DirType
    {
        None,
        Left = -1,
        Right = 1,
        Up = 2,
        Down = -2,
    }

    public enum UiEventType
    {
        PointDown,
        PointUp,
    }
}
