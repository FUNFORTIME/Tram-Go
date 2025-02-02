using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform tf;
    private Rigidbody2D rb;
    private Tram tramData;

    public GameObject tram;
    public Vector3 camOffset=new Vector3(0f,1.5f,-10f);

    public float accelerationWeight=.1f;

    private float OffsetFunction(float _value,float _maxValue)
    {
        float _scale = 10f;
        float _x = _value / _maxValue * _scale - _scale / 3;
        float _y = Mathf.Atan(_x);
        return _y / 3.14f + 0.5f;
    }//根据atan函数得到平滑曲线

    void Start()
    {
        tf = tram.GetComponent<Transform>();
        rb = tram.GetComponent<Rigidbody2D>();
        tramData = tram.GetComponent<Tram>();
    }

    void Update()
    {
        float _offset = tramData.length * OffsetFunction(tramData.speed,tramData.maxSpeed)+accelerationWeight*tramData.acceleration;
        transform.position = tf.position + camOffset + new Vector3(_offset,0);
    }
}
