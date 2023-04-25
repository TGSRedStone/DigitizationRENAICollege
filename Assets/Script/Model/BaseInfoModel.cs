using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInfoModel : AbstractModel
{
    public int InSchool { get; private set; }
    public int OutSchool { get; private set; }
    public int LeaveOfAbsence { get; private set; }

    private const int MAX_CACHE = 10;
    private float totalPower = 0f;
    public float TotalPower { get; private set; }
    public Queue<double> TotalPowerQueue { get; private set; } = new Queue<double>();
    private float totalWater = 0f;
    public float TotalWater { get; private set; }
    public Queue<double> TotalWaterQueue { get; private set; } = new Queue<double>();

    protected override void OnInit() { }

    public void SetInSchool(int value)
    {
        InSchool = value;
    }

    public void SetOutSchool(int value)
    {
        OutSchool = value;
    }

    public void SetLeaveOfAbsence(int value)
    {
        LeaveOfAbsence = value;
    }

    public BaseInfoModel AddData(float power, float water)
    {
        totalPower += power;
        totalWater += water;
        return this;
    }

    public void Complete()
    {
        if (TotalPowerQueue.Count >= MAX_CACHE) TotalPowerQueue.Dequeue();
        TotalPowerQueue.Enqueue(totalPower);
        if (TotalWaterQueue.Count >= MAX_CACHE) TotalWaterQueue.Dequeue();
        TotalWaterQueue.Enqueue(totalWater);
        TotalPower = totalPower;
        TotalWater = totalWater;

        totalPower = 0f;
        totalWater = 0f;
    }
}
