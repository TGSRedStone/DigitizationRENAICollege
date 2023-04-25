using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Config;

public class BuildingModel : AbstractModel
{
    public List<Building> BuildingList { get; private set; } = new List<Building>();

    protected override void OnInit()
    {
        BuildingConfig config = Resources.Load<BuildingConfig>("Configs/BuildingConfig");

        for (int i = 0; i < config.buildingSettings.Count; i++)
        {
            BuildingList.Add(
                new Building(
                    name: config.buildingSettings[i].BuildingName,
                    pos: config.buildingSettings[i].BuildingPos,
                    dir: config.buildingSettings[i].Direction,
                    distance: config.buildingSettings[i].CameraDistance
                )
            );
        }
    }
}

public class Building
{
    public string Name { get; private set; }

    public Vector3 BuildingPos { get; private set; }
    public Direction Direction { get; private set; }
    public float CameraDistance { get; private set; }

    public bool IsSmoke { get; private set; } = false;

    private const int MAX_CACHE = 10;
    public float Temperature { get; private set; }
    public Queue<double> TemperatureQueue { get; private set; } = new Queue<double>();
    public float Humidity { get; private set; }
    public Queue<double> HumidityQueue { get; private set; } = new Queue<double>();
    public float Power { get; private set; }
    public Queue<double> PowerQueue { get; private set; } = new Queue<double>();
    public float Water { get; private set; }
    public Queue<double> WaterQueue { get; private set; } = new Queue<double>();

    private const int THRESHOLD_COUNT = 10;

    private Building() { }
    public Building(string name, Vector3 pos, Direction dir, float distance)
    {
        Name = name;
        BuildingPos = pos;
        Direction = dir;
        CameraDistance = distance;
    }

    public void SetTemperature(float temperature)
    {
        if (TemperatureQueue.Count >= MAX_CACHE) TemperatureQueue.Dequeue();
        Temperature = temperature;
        TemperatureQueue.Enqueue(temperature);
    }

    public void SetHumidity(float humidity)
    {
        if (HumidityQueue.Count >= MAX_CACHE) HumidityQueue.Dequeue();
        Humidity = humidity;
        HumidityQueue.Enqueue(humidity);
    }

    public bool SmokeDebugging { get; set; } = false;
    int smokeActive = 0;
    int smokeCool = 0;
    public void SetSmoke(bool isSmoke)
    {
        IsSmoke = isSmoke;

        if (isSmoke)
        {
            smokeActive += 1;
            smokeCool = 0;
        }
        else
        {
            if (smokeCool < THRESHOLD_COUNT)
                smokeCool += 1;
            else
                smokeActive = 0;
        }
        if (smokeActive == 1)
        {
            SchoolEventManager.Instance.CreatEvent(
                type: SchoolEventType.Error,
                title: $"{Name}烟雾报警器触发",
                info: $"{Name}烟雾报警器触发!{Name}发生火灾，请尽快拨打消防报警电话，并联系相关人员前往组织人员疏散。",
                isDebug: SmokeDebugging);
        }
    }

    public bool PowerDebugging { private get; set; } = false;
    int powerWarning = 0;
    int powerError = 0;
    int powerCool = 0;
    public void SetPower(float power)
    {
        if (PowerQueue.Count >= MAX_CACHE) PowerQueue.Dequeue();
        Power = power;
        PowerQueue.Enqueue(power);

        Check(
            value: power,
            warningThreshold: SettingManager.Instance.Setting.PowerWarning,
            errorThreshold: SettingManager.Instance.Setting.PowerError,
            warningCount: ref powerWarning,
            errorCount: ref powerError,
            coolCount: ref powerCool,
            isDebug: PowerDebugging,
            DataType.Power);
    }

    public bool WaterDebugging { private get; set; } = false;
    int waterWarning = 0;
    int waterError = 0;
    int waterCool = 0;
    public void SetWater(float water)
    {
        if (WaterQueue.Count >= MAX_CACHE) WaterQueue.Dequeue();
        Water = water;
        WaterQueue.Enqueue(water);

        Check(
            value: water,
            warningThreshold: SettingManager.Instance.Setting.WaterWarning,
            errorThreshold: SettingManager.Instance.Setting.WaterError,
            warningCount: ref waterWarning,
            errorCount: ref waterError,
            coolCount: ref waterCool,
            isDebug: WaterDebugging,
            DataType.Water);
    }

    private void Check(float value,
        float warningThreshold,
        float errorThreshold,
        ref int warningCount,
        ref int errorCount,
        ref int coolCount,
        bool isDebug,
        DataType type)
    {
        if (value < warningThreshold)
        {
            if (coolCount < THRESHOLD_COUNT)
            {
                coolCount += 1;
            }
            else
            {
                warningCount = 0;
                errorCount = 0;
            }
        }
        else if (value > errorThreshold)
        {
            errorCount += 1;
            warningCount = 0;
            coolCount = 0;
        }
        else
        {
            warningCount += 1;
            errorCount = 0;
            coolCount = 0;
        }

        SchoolEventType temp = SchoolEventType.Empty;
        if (warningCount == THRESHOLD_COUNT) temp = SchoolEventType.Warning;
        else if (errorCount == THRESHOLD_COUNT) temp = SchoolEventType.Error;
        if (temp != SchoolEventType.Empty)
        {
            string tempType = type switch
            {
                DataType.Power => "耗电量",
                DataType.Water => "耗水量",
                _ => "传入类型错误"
            };
            string tempInfo = temp switch
            {
                SchoolEventType.Warning => "注意",
                SchoolEventType.Error => "警报",
                _ => "传入类型错误"
            };
            string tempThreshold = temp switch
            {
                SchoolEventType.Warning => $"{warningThreshold}",
                SchoolEventType.Error => $"{errorThreshold}",
                _ => "传入类型错误"
            };
            string unit = type switch
            {
                DataType.Power => "W",
                DataType.Water => "T/h",
                _ => "传入类型错误"
            };
            string title = $"{Name}{tempType}异常";
            string info = $"{Name}{tempType}超过{tempInfo}阈值（{tempThreshold}{unit}），请联系相关人员前往检查与处理。";
            SchoolEventManager.Instance.CreatEvent(
                type: temp,
                title: title,
                info: info,
                isDebug: isDebug);
        }
    }
}