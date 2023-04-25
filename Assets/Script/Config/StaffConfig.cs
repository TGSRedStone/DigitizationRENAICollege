using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "StaffConfig", menuName = "ScriptObjects/Configs/StaffConfig")]
public class StaffConfig : ScriptableObject
{
    public List<Staff> Staffs;
}

[Serializable]
public class Staff
{
    public Sprite Avatar;
    public string Name;
    public string Office;
    public string PhoneNumber;
}