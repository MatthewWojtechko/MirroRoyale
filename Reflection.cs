using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflection : MonoBehaviour
{
    /*
     *  Player - reflects all lasers, can speed up based on point of impact
     *  None - no reflection
     *  White - reflects all
     *  Red - reflects red lasers only
     *  Blue - reflects blue lasers only
     *  Green - reflects green lasers only
     *  Yellow - reflects yellow lasers only  
     */
    public enum ReflectionType { WHITE, RED, BLUE, GREEN, YELLOW, NONE };

    public ReflectionType Type = ReflectionType.NONE;

    public Color getColor()
    {
        switch (Type)
        {
            case ReflectionType.WHITE:
                return Color.white;

            case ReflectionType.RED:
                return Color.red;

            case ReflectionType.BLUE:
                return Color.cyan;

            case ReflectionType.GREEN:
                return Color.green;

            case ReflectionType.YELLOW:
                return Color.yellow;

            default:
                return Color.black;
        }
    }
}
