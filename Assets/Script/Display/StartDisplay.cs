using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelName;
    [SerializeField]private SignalInfoDisplay signalInfoDisplay;
    [SerializeField]private SpeedLimitDisplay speedLimitDisplay;
    [SerializeField]private TimeTable timeTable;


    private void Start()
    {
        UI.instance.pause.ChangePause(false);
        UI.instance.pause.PauseOnly(true);

        Level level = LevelInfo.instance.level;
        signalInfoDisplay.SetSignal(SignalColor.yellow);
        //speedLimitDisplay.SetText(level.signalSpeedLimit, true);

        //levelName.text = level.routeName +"\n"+ level.levelName;

        timeTable.CreateTimeTable();
    }
}
