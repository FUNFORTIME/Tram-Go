using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomParallaxBackground : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float length = 18f;
    [SerializeField] private float maxLength;
    [SerializeField] private int sortingOrder;

    void Start()
    {
    }

    [ContextMenu("GenerateRandomBackground")]
    public void GenerateRandomBackground()
    {
        float _x = 0;
        while(_x < maxLength)
        {
            GameObject _obj=Instantiate(new GameObject(), transform);
            _obj.AddComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
            _obj.GetComponent<SpriteRenderer>().sortingLayerName = "BackGround";
            _obj.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
            _obj.transform.localPosition=new Vector3(_x,0,0);

            _x += length;
        }
    }
}
