using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform tf;
    private Rigidbody2D rb;
    private Tram tram;
    private Camera cam;

    public Vector3 camOffset=new Vector3(0f,1.5f,-10f);
    public float camMinSize = 1f;
    public float camMaxSize = 2f;

    public float accelerationWeight=.1f;
    public float speedWeight =1f;

    private float OffsetFunction(float _value,float _maxValue)
    {
        float _scale = 10f;
        float _x = _value / _maxValue * _scale - _scale / 3;
        float _y = Mathf.Atan(_x);
        return _y / 3.14f + 0.407f;
    }//根据atan函数得到平滑曲线

    void Start()
    {
        tram = Manager.instance.tram;
        tf = tram.gameObject.GetComponent<Transform>();
        rb = tram.gameObject.GetComponent<Rigidbody2D>();
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        float _proportion = OffsetFunction(tram.speed, tram.maxSpeed);
        float _offset = tram.length *_proportion*speedWeight +accelerationWeight*tram.acceleration;
        transform.position = tf.position + camOffset + new Vector3(_offset,0);
        cam.orthographicSize = Mathf.Lerp(camMaxSize,camMinSize,_proportion);
    }
}
