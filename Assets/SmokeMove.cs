using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeMove : MonoBehaviour
{
    public Transform a, b;
    public float speed;
    public float delay=4f;

    private float curTime;

    private void Start()
    {
        curTime = Time.time;
    }

    void Update()
    {
        a.position -= new Vector3(speed*2* Time.deltaTime,0,0);
        b.position -= new Vector3(speed * Time.deltaTime, 0, 0);
        if (Time.time-curTime>=delay)
        {
            a.position = Vector3.zero;
            b.position = Vector3.zero;
            curTime = Time.time;
        }
    }
}
