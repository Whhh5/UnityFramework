using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIWindowUserData : GameEntityUserData
{
    public int windowId;
    public int sortingOrder;
    public override void OnPoolDestroy()
    {
        base.OnPoolDestroy();
        windowId
            = sortingOrder
            = -1;
    }
}
public abstract class UIWindowData<TEntity, TUserData> : UIWindowData
    where TEntity : UIWindow
    where TUserData : UIWindowUserData
{
    protected TEntity _UIWindow = null;

    public override void OnPoolDestroy()
    {
        base.OnPoolDestroy();
        _UIWindow = null;
    }
    public override void OnGOCreate(GameEntity entity)
    {
        base.OnGOCreate(entity);
        _UIWindow = entity as TEntity;
    }
    protected override void OnPoolInit(UIWindowUserData userData)
    {
        base.OnPoolInit(userData);
        OnPoolInit(userData as TUserData);
    }
    protected abstract void OnPoolInit(TUserData userData);
}
public abstract class UIWindowData<T> : UIWindowData
    where T : UIWindow
{
    protected T _UIWindow = null;

    public override void OnPoolDestroy()
    {
        base.OnPoolDestroy();
        _UIWindow = null;
    }
    public override void OnGOCreate(GameEntity entity)
    {
        base.OnGOCreate(entity);
        _UIWindow = entity as T;
    }
}
public abstract class UIWindowData : UIEntityData<UIWindow, UIWindowUserData>
{
    public abstract EnUIWindowType WindowType { get; }
    protected int _WindowId;
    protected int _SortingOrder;
    public override void OnPoolDestroy()
    {
        base.OnPoolDestroy();
        _WindowId = -1;
        _SortingOrder = -1;
    }
    protected override void OnPoolInit(UIWindowUserData userData)
    {
        _WindowId = userData.windowId;
        _SortingOrder = userData.sortingOrder;
    }

    public override void OnGOCreate(GameEntity entity)
    {
        base.OnGOCreate(entity);
        _UIEntity.SetWindowId(_WindowId);

        if (!_UIEntity.TryGetComponent<Canvas>(out var canvas))
        {
            canvas = _UIEntity.gameObject.AddComponent<Canvas>();
        }
        if (!_UIEntity.TryGetComponent<GraphicRaycaster>(out var _))
        {
            _UIEntity.gameObject.AddComponent<GraphicRaycaster>();
        }
        if (WindowType == EnUIWindowType.SubWindow)
        {
            canvas.overrideSorting = false;
        }
        else
        {
            canvas.overrideSorting = true;
            canvas.sortingOrder = _SortingOrder;
        }
    }

    public async UniTask HideAsync()
    {
        if (!IsLoadSuccess)
            return;
        if (IsLoadSuccess)
            await _UIEntity.HideAsync();
    }
    public int GetSortingOrder()
    {
        return _SortingOrder;
    }
}
public abstract class UIWindow : UIEntity
{
    protected int _WindowId;
    private CancellationTokenSource _TokenSource;

    private UIAnimationControllerGroup _AnimController;
    private IUIAnimationSliderValue _AnimSliderValue;

    private CanvasGroup _CanvasGroup = null;
    public void SetWindowId(int windowId)
    {
        _WindowId = windowId;
    }

    public override void LoadCompletion()
    {
        base.LoadCompletion();
        if (!gameObject.TryGetComponent<CanvasGroup>(out _CanvasGroup))
        {
            _CanvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        if (gameObject.TryGetComponent(out _AnimController))
        {
            if (gameObject.TryGetComponent<IUIAnimationController>(out var ctl))
            {
                _AnimController.Register(ctl);
            }
        }
        if (_AnimController != null)
        {
            _AnimSliderValue = _AnimController;
        }
        else if(gameObject.TryGetComponent<IUIAnimationSliderValue>(out var sliderValue))
        {
            _AnimSliderValue = sliderValue;
        }
        if (_AnimSliderValue == null)
        {
            _AnimSliderValue = gameObject.AddComponent<UIScaleAnimation>();
        }
        _CanvasGroup.interactable = false;
        ShowAsync().Forget();
    }
    public override void OnUnload()
    {
        if (_AnimController != null)
            _AnimController.UnregisterAll();
        _TokenSource?.Cancel();
        base.OnUnload();
        _TokenSource = null;
        _AnimController = null;
        _AnimSliderValue = null;
    }

    public CancellationToken GetWindowToken()
    {
        if (_TokenSource == null)
            _TokenSource = new();
        return _TokenSource.Token;
    }

    protected void RegisterUIDissolve<T>(T go)
    {
        if (_AnimController == null)
            return;
        if (go is not MonoBehaviour mono)
            return;
        if (!mono.TryGetComponent<UIDissolveController>(out var ctl))
            ctl = mono.gameObject.AddComponent<UIDissolveController>();
        _AnimController.Register(ctl);
    }
    protected void UnregisterUIDissolve<T>(T go)
    {
        if (_AnimController == null)
            return;
        if (go is not MonoBehaviour mono)
            return;
        if (!mono.TryGetComponent<UIDissolveController>(out var ctl))
            return;
        _AnimController.Unregister(ctl);
    }
    protected void RegisterUIController<T>(T go)
    {
        if (_AnimController == null)
            return;
        if (go is not MonoBehaviour mono)
            return;
        if (!mono.TryGetComponent<IUIAnimationController>(out var ctl))
            return;
        _AnimController.Register(ctl);
    }
    protected void UnregisterUIController<T>(T go)
    {
        if (_AnimController == null)
            return;
        if (go is not MonoBehaviour mono)
            return;
        if (!mono.TryGetComponent<IUIAnimationController>(out var ctl))
            return;
        _AnimController.Unregister(ctl);
    }

    protected virtual async UniTask ShowAsync()
    {
        if (_AnimSliderValue != null)
        {
            await DOTween.To(() => 0f, slider =>
            {
                _AnimSliderValue.SetShowSliderValue(slider);

            }, 1f, 0.2f)
                .SetUpdate(true)
                .SetId(GetHashCode());
        }
        Show();
    }
    public virtual async UniTask HideAsync()
    {
        Hide();
        if (_AnimSliderValue != null)
        {
            DOTween.Kill(GetHashCode());
            await DOTween.To(() => 0f, slider =>
            {
                _AnimSliderValue.SetHideSliderValue(slider);

            }, 1f, 0.1f)
            .SetUpdate(true)
            .SetId(GetHashCode());
        }
    }

    protected virtual void Show()
    {
        _CanvasGroup.interactable = true;
    }
    protected virtual void Hide()
    {
        _CanvasGroup.interactable = false;
    }
}