using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class UIManager : MonoBehaviour
{
    /// <summary>
    /// 1宿舍楼 Num为0
    /// </summary>
    public static UIManager instance;
    public List<DormitoryUISDormitoryUIScriptBase> Dormitory = new List<DormitoryUISDormitoryUIScriptBase>();
    DormitoryUISDormitoryUIScriptBase NowDormitory;
    public static int DormitoryNumber ;
    public BuildingUI buildingUI;
    public GameObject builing;
    public bool builingActive = true;
    private void Awake()
    {
        instance = this;     
    }
    public enum BulidingType
    {
        LivingArea,
        TeachingArea,
        LaboratoryBuilding,
        FunctionalArea,
    }
    // Start is called before the first frame update
    void Start()
    {
        FindData();
        SetData();
    }

    // Update is called once per frame
    private void Update()
    {
        FindData();
        SetData();
       // Debug.Log("SliderValue的值为:" + NowDormitory.sliderValue);
        if (Input.GetKeyDown(KeyCode.E)) {
            //UIBase[] UIout = this.GetComponentsInChildren<UIBase>();
            //foreach (var ui in UIout) {
            //    //ui.downUICanOut = true;
            //    //ui.SwitchUI();

            //}
            DormitoryNumber+=1;
            if (DormitoryNumber >= 3) {
                DormitoryNumber = 0;
            }
          
                builing.SetActive(false);
         
                builing.SetActive(true);
              
            
           
           
        }
    }
    public void FindData() {
      NowDormitory  = Dormitory.Find(x => x.DormitoryNum == DormitoryNumber);
    //    Debug.Log("Find成功"+NowDormitory.name);
    }
    public void SetData()
    {
        for (int i = 0; i < 3; i++)
        {
            buildingUI.sliderText[i].text = ((int)(NowDormitory.sliderValue[i]*100)).ToString()+"%";
            buildingUI.SliderCountData[i].value = NowDormitory.sliderValue[i];
            buildingUI.post[i].text = NowDormitory.post[i];
            buildingUI.Name[i].text = NowDormitory.Name[i];
            buildingUI.TelephoneNumber[i].text = NowDormitory.TelephoneNumber[i];
            buildingUI.eventNumber[i].text = NowDormitory.eventNumber[i].ToString();
        }
        buildingUI.Energy = NowDormitory.Energy;
        buildingUI.Water = NowDormitory.Water;
        buildingUI.temperature.text = NowDormitory.temperature.ToString();
        buildingUI.humidity.text = NowDormitory.humidity.ToString();
        buildingUI.IsolationNumber.text = NowDormitory.IsolationNumber.ToString();
        buildingUI.temperatureNumber.text = NowDormitory.temperatureNumber.ToString();
        for (int i = 0; i < 5; i++)
        {
            buildingUI.eventString[i].text = NowDormitory.eventString[i].ToString();
        }
    }
}
