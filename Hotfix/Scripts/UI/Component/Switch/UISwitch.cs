using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class UISwitch : MonoBehaviour, IUISwitch
{
    public abstract void Switch(int index);
    public abstract int GetSwitchCount();
}