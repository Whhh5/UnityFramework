using UnityEngine;

public interface IUIAnimationController
{
    public void Init();
    public void Destroy();
    public void SetSliderValue(float value);
}
