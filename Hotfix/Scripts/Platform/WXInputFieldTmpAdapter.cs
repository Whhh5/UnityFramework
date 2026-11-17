#if UNITY_WEBGL && UNITY_MINIGAME_WX
#define UNITY_WX
#endif

#if UNITY_WX
using UnityEngine;
using WeChatWASM;
using TMPro;
using UnityEngine.EventSystems;
using System.Linq;

[RequireComponent(typeof(TMP_InputField))]
public class WXInputFieldTmpAdapter : MonoBehaviour, IPointerClickHandler
{
    private TMP_InputField _field;
    private static bool _showing;

    void Awake()
    {
        _field = GetComponent<TMP_InputField>();
        _field.shouldHideMobileInput = true;
    }
    private void OnDestroy()
    {
        Close();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_showing) return;
        // 阻止 TMP 自动调原生键盘
        var type = _field.keyboardType == TouchScreenKeyboardType.NumberPad
            ? "number"
            : "text";
        WX.ShowKeyboard(new ShowKeyboardOption
        {
            defaultValue = _field.text,
            maxLength = _field.characterLimit > 0 ? _field.characterLimit : 11,
            confirmType = "done",
            keyboardType = type
        });

        WX.OnKeyboardInput(OnInput);
        WX.OnKeyboardConfirm(OnConfirm);
        WX.OnKeyboardComplete(OnComplete);
        _showing = true;
    }

    void OnInput(OnKeyboardInputListenerResult v)
    {
        // 如果是数字框，在这里过滤非数字
        var s = new string(v.value.Where(c => c >= '0' && c <= '9').ToArray());
        if (_field.characterLimit > 0 && s.Length > _field.characterLimit)
            s = s.Substring(0, _field.characterLimit);
        _field.text = s;
    }

    void OnConfirm(OnKeyboardInputListenerResult v)
    {
        //_field.text = v.value;
        Close();
    }
    void OnComplete(OnKeyboardInputListenerResult v)
    {
        //_field.text = v.value;
        Close();
    }

    void Close()
    {
        if (!_showing) return;
        WX.HideKeyboard(new HideKeyboardOption());
        WX.OffKeyboardInput(OnInput);
        WX.OffKeyboardConfirm(OnConfirm);
        WX.OffKeyboardComplete(OnComplete);
        _showing = false;
    }

    public void OnDeselect(BaseEventData _) => Close();
}
#endif