using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TimeMgr : Singleton<TimeMgr>
{
    private int _IsPause;
    private readonly EnDotweenId _TimeSclaeId = EnDotweenId.GameMapMgr;
    private readonly Dictionary<int, float> _GameSpeedList = new(10);
    private float _CurSpeed = -1;
    private float _FinalSpeed = -1;
    public int SetGameSpeed(float speed, bool immediately)
    {
        var setId = ABBUtil.GetTempKey();
        _GameSpeedList.Add(setId, speed);
        if (!IsPause())
        {
            if (speed > _FinalSpeed)
            {
                SetSpeed(speed, immediately);
            }
        }
        return setId;
    }
    public void CancelGameSpeed(int setId, bool immediately)
    {
        if (!_GameSpeedList.TryGetValue(setId, out var speed))
            return;
        _GameSpeedList.Remove(setId);

        if (!Mathf.Approximately(speed, _FinalSpeed))
            return;
        if (IsPause())
            return;
        UpdateCurrentSpeed(immediately);
    }
    private void UpdateCurrentSpeed(bool immediately)
    {
        if (_GameSpeedList.Count > 0)
        {
            var maxValue = -1f;
            foreach (var item in _GameSpeedList)
            {
                if (item.Value <= maxValue)
                    continue;
                maxValue = item.Value;
            }
            SetSpeed(maxValue, immediately);
        }
        else
        {
            SetSpeed(1, immediately);
        }
    }

    private void SetSpeed(float speed, bool immediately)
    {
        _FinalSpeed = speed;
        if (immediately)
        {
            DOTween.Kill(_TimeSclaeId);
            Time.timeScale = speed;
            _CurSpeed = speed;
        }
        else
        {
            DOTween.Kill(_TimeSclaeId);
            var from = Time.timeScale;
            var to = speed;
            DOTween.To(() => 0f, slider =>
            {
                Time.timeScale = Mathf.Lerp(from, to, slider);
                _CurSpeed = Time.timeScale;

            }, 1f, Mathf.Min(1, Mathf.Abs(to - from) / 2))
                .SetId(_TimeSclaeId)
                .SetUpdate(true);
        }
    }
    public void SetPause(bool isPause, bool immediately)
    {
        _IsPause += isPause ? 1 : -1;
        if (isPause)
        {
            if (_IsPause == 1)
            {
                SetSpeed(0, immediately);
            }
        }
        else
        {
            if (_IsPause == 0)
            {
                UpdateCurrentSpeed(immediately);
            }
        }
    }
    public bool IsPause()
    {
        return _IsPause > 0;
    }
    public float GetCurGameSpeed()
    {
        return _CurSpeed;
    }
}
