using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EventData
{
    private string _type;
    private string _data;

    public EventData(string type, string data)
    {
        _data = data;
        _type = type;
    }
}
