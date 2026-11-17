using UnityEngine;

public interface IUIAnimationSliderValue
{
    public void SetShowSliderValue(float value);
    public void SetHideSliderValue(float value);
}
public interface IUIAnimationController: IUIAnimationSliderValue
{

}
