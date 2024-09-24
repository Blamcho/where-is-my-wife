using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
     private float length, startpos;
     public GameObject _MainCamera;
     public float _ParallaxEfect;
    
    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    
    void Update()
    {
        float _Tempo = (_MainCamera.transform.position.x * (1 - _ParallaxEfect));
        float _Distance = (_MainCamera.transform.position.x * _ParallaxEfect);
        transform.position = new Vector3(startpos + _Distance, transform.position.y, transform.position.z);

        if (_Tempo > startpos + length) startpos += length;
        else if (_Tempo < startpos - length) startpos -= length;
       
    }
}
