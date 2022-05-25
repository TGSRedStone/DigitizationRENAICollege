using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Domitory_",menuName ="Inventory/New Domitory")]
public class DormitoryUISDormitoryUIScriptBase : ScriptableObject
{
    public int DormitoryNum;
    [Header("-----第一面板UI-----")]
    [Header("--左侧UI--")]
    [Tooltip("01 器材故障比，02 在寝比例")]
    public float []sliderValue = new float[3];
    [Tooltip("用电量")]
    public float Energy;
    [Tooltip("用水量")]
    public float Water;
    private float waterEnergy;
    [Header("--右侧UI--")]
    [Tooltip("温度")]
    public int temperature;
    [Tooltip("湿度")]
    public float humidity;
    [Tooltip("隔离人数")]
    public int IsolationNumber;
    [Tooltip("体温异常人数")]
    public int temperatureNumber;
    [Space(20)]
    [Header("-----第二面板UI-----")]
    [Header("--左侧UI--")]
    [Tooltip("管理人职位")]
    public string[] post = new string[3];
    [Tooltip("管理人名称")]
    public string[] Name=new string[3];
    [Tooltip("电话号码")]
    public string[] TelephoneNumber = new string[3];
    [Header("--右侧UI--")]
    [Tooltip("未解决，待处理，已解决个数")]
    public int[] eventNumber = new int[3];
    [Tooltip("事件列表")]
    public string[] eventString = new string [5];
    // Start is called before the first frame update
    private void Awake()
    {
        sliderValue[2] = waterEnergy = Water / Energy;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
