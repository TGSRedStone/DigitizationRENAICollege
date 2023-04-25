using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigitalRENAI : Architecture<DigitalRENAI>
{
    protected override void Init()
    {
        RegisterModel(new BuildingModel());
        RegisterModel(new BaseInfoModel());
    }
}
