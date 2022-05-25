using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public  class BuildingUI :UIBase,ChartLinInterFace
{
  
    public int thisBuildingNum;
    public DormitoryUISDormitoryUIScriptBase DormistoryList;
    private int UINum=0;
    private UIManager uiManager;
   
    #region ��һ�����߱���
    [SerializeField]
    [Tooltip("�õ���")]
    public float Energy;
    [SerializeField]
    [Tooltip("��ˮ��")]
    public float Water;
    public float waterEnergy;
    public ChartSlider chartSlider;
    [SerializeField]
    public Slider[] SliderCountData = new Slider[3];
    [Tooltip("�¶�")]
    public TextMeshProUGUI temperature;
    [Tooltip("ʪ��")]
    public TextMeshProUGUI humidity;
    [Tooltip("��������")]
    public TextMeshProUGUI IsolationNumber;
    [Tooltip("�����쳣����")]
    public TextMeshProUGUI temperatureNumber;
    [Space(20)]
    public TextMeshProUGUI[] sliderText = new TextMeshProUGUI[3];
    [Tooltip("������ְλ")]
    public TextMeshProUGUI[] post = new TextMeshProUGUI[3];
    [Tooltip("����������")]
    public TextMeshProUGUI[] Name = new TextMeshProUGUI[3];
    [Tooltip("�绰����")]
    public TextMeshProUGUI[] TelephoneNumber = new TextMeshProUGUI[3];
    [Tooltip("δ������������ѽ������")]
    public TextMeshProUGUI[] eventNumber = new TextMeshProUGUI[3];
    [Tooltip("�¼��б�")]
    public TextMeshProUGUI[] eventString = new TextMeshProUGUI[5];
    
    #endregion
    #region ��һ����ұ߱���

    #endregion
    private void Awake()
    {
        uiManager = this.GetComponentInParent<UIManager>();
    }
    private void OnEnable()
    {
     
        SwitchUI();
        StartToCamera_1();
        downUICanOut = false;
    }
   
    public void  DrawChartline() {
    
    }
    public void OutToCamera_1()
    {
        LiftUI.DOPlayBackwards();
        RightUI.DOPlayBackwards();
        //DownUI.DOPlayBackwards();
        MoveOutDownUI();
        Debug.Log("�ɹ�ִ��outToCamera1");
    }

    public void StartToCamera_1()
    {
        L_B1.GetComponent<Button>().interactable = false;
        L_B2.GetComponent<Button>().interactable = true;
        L_B3.GetComponent<Button>().interactable = true;
        UINum = 0;
       Tweener Ltweener =  LiftUI.DOLocalMove(LiftUIIn.localPosition, MoveTime);
       Tweener Rtweener = RightUI.DOLocalMove(RightUIIn.localPosition, MoveTime);
       Tweener Dtweener = DownUI.DOLocalMove(DownUIIn.localPosition, MoveTime);
        Ltweener.SetAutoKill(false);
        Rtweener.SetAutoKill(false);
        Dtweener.SetAutoKill(false); 
    }
    public void OutToCamera_2()
    {
        LiftUI_2.DOPlayBackwards();
        RightUI_2.DOPlayBackwards();
        MoveOutDownUI();
    }

    public void StartToCamera_2()
    {
        L_B2.GetComponent<Button>().interactable = false;
        L_B1.GetComponent<Button>().interactable = true;
        L_B3.GetComponent<Button>().interactable = true;
        UINum = 1;
        Tweener Ltweener2 = LiftUI_2.DOLocalMove(LiftUIIn.localPosition, MoveTime);
        Tweener Rtweener2 = RightUI_2.DOLocalMove(RightUIIn.localPosition, MoveTime);
        Ltweener2.SetAutoKill(false);
        Rtweener2.SetAutoKill(false);
       

    }
    public void OutToCamera_3()
    {
        LiftUI_3.DOPlayBackwards();
        RightUI_3.DOPlayBackwards();
        MoveOutDownUI();
    }

    public void StartToCamera_3()
    {
        L_B3.GetComponent<Button>().interactable = false;
        L_B1.GetComponent<Button>().interactable = true;
        L_B2.GetComponent<Button>().interactable = true;
        UINum = 2;
        Tweener Ltweener3 = LiftUI_3.DOLocalMove(LiftUIIn.localPosition, MoveTime);
        Tweener Rtweener3 = RightUI_3.DOLocalMove(RightUIIn.localPosition, MoveTime);
        Ltweener3.SetAutoKill(false);
        Rtweener3.SetAutoKill(false);
    }
   
    public void SwitchUI() {
        if (UINum == 0) OutToCamera_1();
        if (UINum == 1) OutToCamera_2();
        if (UINum == 2) OutToCamera_3();
    }
}
