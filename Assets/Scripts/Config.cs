using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Config
{
    public Dictionary<string, Color> Colors = new Dictionary<string, Color>()
    {
        {"red", Color.red},
        {"blue", Color.blue}
    };

    public enum Teams
    {
        RED,
        BLUE
    }
}