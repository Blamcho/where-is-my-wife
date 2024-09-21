using System;
using UnityEngine;

[Serializable]
public class StringArray 
{
    [field:SerializeField] public string Note {get; private set;}
    [field:TextArea(1, 10)]
    [field:SerializeField] public string[] Text {get; private set;}
}
