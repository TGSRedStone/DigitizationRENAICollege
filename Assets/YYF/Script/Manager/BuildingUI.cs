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
   
    #region 第一面板左边变量
    [SerializeField]
    [Tooltip("用电量")]
    public float Energy;
    [SerializeField]
    [Tooltip("用水量")]
    public float Water;
    public float waterEnergy;
    public ChartSlider chartSlider;
    [SerializeField]
    public Slider[] SliderCountData = new Slider[3];
    [Tooltip("温度")]
    public TextMeshProUGUI temperature;
    [Tooltip("湿度")]
    public TextMeshProUGUI humidity;
    [Tooltip("隔离人数")]
    public TextMeshProUGUI IsolationNumber;
    [Tooltip("体温异常人数")]
    public TextMeshProUGUI temperatureNumber;
    [Space(20)]
    public TextMeshProUGUI[] sliderText = new TextMeshProUGUI[3];
    [Tooltip("管理人职位")]
    public TextMeshProUGUI[] post = new TextMeshProUGUI[3];
    [Tooltip("管理人名称")]
    public TextMeshProUGUI[] Name = new TextMeshProUGUI[3];
    [Tooltip("电话号码")]
    public TextMeshProUGUI[] TelephoneNumber = new TextMeshProUGUI[3];
    [Tooltip("未解决，待处理，已解决个数")]
    public TextMeshProUGUI[] eventNumber = new TextMeshProUGUI[3];
    [Tooltip("事件列表")]
    public TextMeshProUGUI[] eventString = new TextMeshProUGUI[5];
    
    #endregion
    #region 第一面板右边变量

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
        Debug.Log("成功执行outToCamera1");
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
