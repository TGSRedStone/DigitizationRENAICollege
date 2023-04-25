using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataGenerationManager : MonoBehaviour, IController
{
    public static DataGenerationManager Instance { get; private set; }

    private BuildingModel buildingModel;
    private BaseInfoModel baseInfoModel;

    private Dictionary<DebugMarking, SchoolDebugBase> debugs = new Dictionary<DebugMarking, SchoolDebugBase>();

    private float tempTime = 0f;

    private void Awake()
    {
        if (Instance is null)
        {
            Instance = this;
        }

        buildingModel = this.GetModel<BuildingModel>();
        baseInfoModel = this.GetModel<BaseInfoModel>();
    }

    private void Update()
    {
        if (tempTime > 1f)
        {
            if (SettingManager.Instance.Setting.IsAnalog)
                AnalogGenerateData();
            else
                GenerateData();

            tempTime = 0f;
        }
        tempTime += Time.deltaTime;
    }

    public IArchitecture GetArchitecture()
    {
        return DigitalRENAI.Interface;
    }

    private void AnalogGenerateData()
    {
        for (int i = 0; i < buildingModel.BuildingList.Count; i++)
        {
            SchoolDebugBase temp;
            DebugMarking marking = new DebugMarking() { buildingID = i, dataType = DataType.Empty };

            buildingModel.BuildingList[i].SetTemperature(Random.Range(25f, 30f));
            buildingModel.BuildingList[i].SetHumidity(Random.Range(40f, 50f));

            marking.dataType = DataType.Smoke;
            if (debugs.TryGetValue(marking, out temp))
            {
                buildingModel.BuildingList[i].SmokeDebugging = true;
                buildingModel.BuildingList[i].SetSmoke((temp as SchoolDebugBool).Value);
            }
            else
            {
                buildingModel.BuildingList[i].SmokeDebugging = false;
                buildingModel.BuildingList[i].SetSmoke(false);
            }

            marking.dataType = DataType.Power;
            if (debugs.TryGetValue(marking, out temp))
            {
                buildingModel.BuildingList[i].PowerDebugging = true;
                buildingModel.BuildingList[i].SetPower((temp as SchoolDebugNumber).Number);
            }
            else
            {
                buildingModel.BuildingList[i].PowerDebugging = false;
                buildingModel.BuildingList[i].SetPower(
                    Random.Range(
                        minInclusive: SettingManager.Instance.Setting.PowerWarning * SettingManager.Instance.Setting.MinScale,
                        maxInclusive: SettingManager.Instance.Setting.PowerWarning * SettingManager.Instance.Setting.MaxScale
                    )
                );
            }

            marking.dataType = DataType.Water;
            if (debugs.TryGetValue(marking, out temp))
            {
                buildingModel.BuildingList[i].WaterDebugging = true;
                buildingModel.BuildingList[i].SetWater((temp as SchoolDebugNumber).Number);
            }
            else
            {
                buildingModel.BuildingList[i].WaterDebugging = false;
                buildingModel.BuildingList[i].SetWater(
                    Random.Range(
                        minInclusive: SettingManager.Instance.Setting.WaterWarning * SettingManager.Instance.Setting.MinScale,
                        maxInclusive: SettingManager.Instance.Setting.WaterWarning * SettingManager.Instance.Setting.MaxScale
                    )
                );
            }
            baseInfoModel.AddData(power: buildingModel.BuildingList[i].Power, water: buildingModel.BuildingList[i].Water);
        }

        int tempIn = Random.Range(775, 825);
        int tempAbsence = Random.Range(10, 30);
        baseInfoModel.SetInSchool(tempIn);
        baseInfoModel.SetOutSchool(1000 - tempIn - tempAbsence);
        baseInfoModel.SetLeaveOfAbsence(tempAbsence);
        baseInfoModel.Complete();
    }

    private void GenerateData()
    {
        for (int i = 0; i < buildingModel.BuildingList.Count; i++)
        {
            buildingModel.BuildingList[i].SetTemperature(0f);
            buildingModel.BuildingList[i].SetHumidity(0f);

            buildingModel.BuildingList[i].SmokeDebugging = false;
            buildingModel.BuildingList[i].SetSmoke(false);

            buildingModel.BuildingList[i].PowerDebugging = false;
            buildingModel.BuildingList[i].SetPower(0f);
            buildingModel.BuildingList[i].WaterDebugging = false;
            buildingModel.BuildingList[i].SetWater(0f);

            baseInfoModel.AddData(power: buildingModel.BuildingList[i].Power, water: buildingModel.BuildingList[i].Water);
        }

        baseInfoModel.SetInSchool(0);
        baseInfoModel.SetOutSchool(0);
        baseInfoModel.SetLeaveOfAbsence(0);
        baseInfoModel.Complete();
    }

    public bool CreateDebug(SchoolDebugBase debug)
    {
        DebugMarking temp = new DebugMarking()
        {
            buildingID = debug.BuildingID,
            dataType = debug.DataType
        };
        if (debugs.ContainsKey(temp)) return false;
        debugs.Add(temp, debug);
        return true;
    }

    public void RemoveDebug(DebugMarking marking)
    {
        if (debugs.ContainsKey(marking)) debugs.Remove(marking);
    }
}


public abstract class SchoolDebugBase
{
    public int BuildingID { get; protected set; }
    public DataType DataType { get; protected set; }
    public int count;
}

public sealed class SchoolDebugNumber : SchoolDebugBase
{
    private float number;
    public float Number
    {
        get
        {
            if (count-- <= 0)
            {
                DataGenerationManager.Instance.RemoveDebug(
                    new DebugMarking()
                    {
                        buildingID = BuildingID,
                        dataType = DataType
                    });
            }
            return number;
        }
    }

    private SchoolDebugNumber() { }
    public SchoolDebugNumber(int id, DataType type, float number, int count)
    {
        BuildingID = id;
        DataType = type;
        this.number = number;
        this.count = count;
    }
}

public sealed class SchoolDebugBool : SchoolDebugBase
{
    private bool value;
    public bool Value
    {
        get
        {
            if (count-- <= 0)
            {
                DataGenerationManager.Instance.RemoveDebug(
                    new DebugMarking()
                    {
                        buildingID = BuildingID,
                        dataType = DataType
                    });
            }
            return value;
        }
    }

    private SchoolDebugBool() { }
    public SchoolDebugBool(int id, DataType type, bool value, int count)
    {
        BuildingID = id;
        DataType = type;
        this.value = value;
        this.count = count;
    }
}

public enum DataType
{
    Empty,
    Power,
    Water,
    Smoke
}

public struct DebugMarking
{
    public int buildingID;
    public DataType dataType;
}