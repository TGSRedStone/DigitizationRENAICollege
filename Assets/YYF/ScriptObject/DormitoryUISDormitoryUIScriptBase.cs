using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Domitory_",menuName ="Inventory/New Domitory")]
public class DormitoryUISDormitoryUIScriptBase : ScriptableObject
{
    public int DormitoryNum;
    [Header("-----��һ���UI-----")]
    [Header("--���UI--")]
    [Tooltip("01 ���Ĺ��ϱȣ�02 ���ޱ���")]
    public float []sliderValue = new float[3];
    [Tooltip("�õ���")]
    public float Energy;
    [Tooltip("��ˮ��")]
    public float Water;
    private float waterEnergy;
    [Header("--�Ҳ�UI--")]
    [Tooltip("�¶�")]
    public int temperature;
    [Tooltip("ʪ��")]
    public float humidity;
    [Tooltip("��������")]
    public int IsolationNumber;
    [Tooltip("�����쳣����")]
    public int temperatureNumber;
    [Space(20)]
    [Header("-----�ڶ����UI-----")]
    [Header("--���UI--")]
    [Tooltip("������ְλ")]
    public string[] post = new string[3];
    [Tooltip("����������")]
    public string[] Name=new string[3];
    [Tooltip("�绰����")]
    public string[] TelephoneNumber = new string[3];
    [Header("--�Ҳ�UI--")]
    [Tooltip("δ������������ѽ������")]
    public int[] eventNumber = new int[3];
    [Tooltip("�¼��б�")]
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
