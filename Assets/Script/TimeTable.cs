using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI.TableUI;

public class TimeTable : MonoBehaviour
{
    [SerializeField] private Transform pasteParent;
    private TableUI table;

    public void CreateTimeTable()
    {
        table = GetComponent<TableUI>();
        table.Columns = 3;

        int _layerMask = 1 << (int)SignType.stop;
        //LayerMask
        RaycastHit2D[] _hitArray = 
            Physics2D.RaycastAll(Manager.instance.frontCheck.position, Vector2.right,Mathf.Infinity, _layerMask);

        List<Stop> _stopInfo = new List<Stop>();
        foreach(RaycastHit2D _hit in _hitArray)
        {
            _stopInfo.Add(_hit.collider.gameObject.GetComponent<Stop>());
        }

        //table.GetCell(0,0).text = "Stop";
        //table.GetCell(0,1).text = "Arr.";
        //table.GetCell(0,2).text = "Dep.";
        table.Rows = _stopInfo.Count + 1;

        for (int i = 0; i < _stopInfo.Count; i++)
        {
            _stopInfo[i].arrivalTime += LevelInfo.instance.departureTime;
            _stopInfo[i].departureTime += LevelInfo.instance.departureTime;

            table.GetCell(i+1,0).text = _stopInfo[i].GetLocalizedText();
            table.GetCell(i+1,1).text = _stopInfo[i].arrivalTime.ToString();
            table.GetCell(i+1,2).text = _stopInfo[i].passing ? "Non-Stop" : _stopInfo[i].departureTime.ToString();
        }

        Instantiate(gameObject, pasteParent).transform.localPosition = Vector3.zero;
    }

}
