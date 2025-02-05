using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceSignCreator : MonoBehaviour
{
    [SerializeField] GameObject distanceSignPrefab;
    [SerializeField]private bool createForward=true;
    [SerializeField] private int distanceBetween=100;
    [SerializeField] private int maxNum = 999;

    [ContextMenu("CreateDistanceSign")]
    private void CreateDistanceSign()
    {
        for (int i = 0; i < transform.childCount; i++)
            Destroy(transform.GetChild(i).gameObject);

        int _direction = createForward ? 1 : -1;
        for (int i = 0; i < maxNum; i++)
        {
            GameObject sign = Instantiate(distanceSignPrefab);
            sign.GetComponent<DistanceSign>().num =i;
            sign.GetComponent<DistanceSign>().UpdateDisplay();

            sign.transform.parent = transform;
            sign.transform.localPosition = new Vector3( _direction* distanceBetween * i, 0);
        }
    }

}
