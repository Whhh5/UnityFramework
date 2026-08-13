using System.Collections.Generic;
using System.Reflection;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using UnityEditor;
using UnityEngine;
using Color = UnityEngine.Color;

public class GridEditorInfo
{
    public Rect globalRect;
    public int monsterIndex;
}
public struct WaveEditorInfo
{
    public List<GameBattleWaveMonsterInfo> monsterInfos;
    public Dictionary<int, GridEditorInfo> gridInfos;
}
public class DebugWaveWindow : EditorWindow
{
    // map
    private int _MapId;
    private int _MaxRow;
    private int _MaxCol;
    private Vector2 _GridSize;
    private readonly float _Scale = 0.5f;

    // wave
    private readonly Dictionary<int, WaveEditorInfo> _GridIndex2Rect = new();
    private readonly Dictionary<int, Rect> _MonsterMenuDic = new();
    private Rect _CloseMonsterRect;
    private int _CurWaveId;

    // copy
    private GameBattleWaveMonsterInfo _CopyMonsterInfo = new();
    private WaveEditorInfo _CopyWaveInfo = new();

    #region window
    private void OnEnable()
    {
        EditorApplication.update += OnEditorUpdate;
    }
    private void OnDisable()
    {
        EditorUtil.ClearMouseController(EnMouseControllerType.DebugWaveWindow);
        EditorApplication.update -= OnEditorUpdate;
        _CurWaveId = -1;
        _MapId = -1;
        _LastSelectMonsterIndex = -1;
    }
    private void OnEditorUpdate()
    {
        this.Repaint();
    }
    private void OnGUI()
    {
        CopyMapMonster();

        var rect = position;
        var menuWidth = 100f;
        var mapMenuRect = new Rect()
        {
            width = menuWidth,
            height = rect.height,
        };
        var menuRect = new Rect()
        {
            width = menuWidth,
            height = rect.height,
            x = mapMenuRect.xMax + 5,
        };
        GUI.BeginClip(mapMenuRect);
        DrawMapMenu(mapMenuRect);
        GUI.EndClip();

        GUI.BeginClip(menuRect);
        DrawWaveMenuList(menuRect);
        GUI.EndClip();

        var monsterInfoWidth = 300f;
        var gridRect = new Rect()
        {
            x = menuRect.xMax,
            y = menuRect.yMin,
            width = rect.width - menuRect.xMax - monsterInfoWidth,
            height = rect.height,
        };

        if (_LastSelectMonsterIndex >= 0)
        {
            var monsterInfoRect = new Rect()
            {
                width = monsterInfoWidth,
                height = rect.height,
                position = new Vector2(gridRect.xMax, gridRect.yMin),
            };
            GUI.BeginClip(monsterInfoRect);
            DrawMonsterInfo(monsterInfoRect);
            GUI.EndClip();
        }

        GUI.BeginClip(gridRect);
        DrawGrid(gridRect);
        GUI.EndClip();

        CheckMouseController();
        CheckMouseMenu();

        #region menu scroll
        if (EditorUtil.HasMouseInRect(_WaveMenuRect))
        {
            if (Event.current.isScrollWheel)
            {
                _WaveMenuPosition = Event.current.delta.y * _WaveMenuScrollDelta / 10 + _WaveMenuPosition;
            }
        }
        if (EditorUtil.HasMouseInRect(_MapMenuRect))
        {
            if (Event.current.isScrollWheel)
            {
                _MapMenuScrollDelta = Event.current.delta.y * _MapMenuScrollDelta / 10 + _MapMenuScrollDelta;
            }
        }
        #endregion
    }
    #endregion

    #region datas
    private GridEditorInfo GetCurWaveGridInfo(int gridIndex)
    {
        if (!_GridIndex2Rect.TryGetValue(_CurWaveId, out var waveInfo))
            return null;
        if (!waveInfo.gridInfos.TryGetValue(gridIndex, out var result))
        {
            result = new()
            {
                monsterIndex = -1,
            };
            waveInfo.gridInfos.Add(gridIndex, result);
        }
        return result;
    }
    public List<GameBattleWaveMonsterInfo> GetMonsterInfos()
    {
        if (!_GridIndex2Rect.TryGetValue(_CurWaveId, out var waveInfo))
            return null;
        return waveInfo.monsterInfos;
    }
    public GameBattleWaveMonsterInfo GetMonsterInfos(int index)
    {
        if (!_GridIndex2Rect.TryGetValue(_CurWaveId, out var waveInfo))
            return null;
        return waveInfo.monsterInfos[index];
    }
    public bool CurWaveGridIsValid(int index)
    {
        if (!_GridIndex2Rect.TryGetValue(_CurWaveId, out var waveInfo))
            return false;
        return waveInfo.gridInfos.ContainsKey(index);
    }
    #endregion

    #region wave menu
    private float _WaveMenuPosition = 0;
    private Rect _WaveMenuRect;
    private float _WaveMenuScrollDelta;
    private void DrawWaveMenuList(Rect rect)
    {
        var titleRect = new Rect()
        {
            size = new Vector2(rect.width - 5, 30),
            x = 0,
            y = 5,
        };
        GUI.Label(titleRect, "wave");
        EditorGuiUtil.DrawBox(titleRect, "");

        var listRect = new Rect()
        {
            x = 0,
            y = titleRect.yMax + 5,
            height = rect.height - titleRect.yMax - 5 * 2,
            width = titleRect.width,
        };
        EditorGuiUtil.DrawBox(listRect, "");
        GUI.BeginClip(listRect);
        if (_MapId > 0)
        {
            var mapCfg = ExcelEditorUtil.GetCfg<MapCfg>(_MapId);
            var count = mapCfg.nMaxWaveId - mapCfg.nMinWaveId + 1;
            var btnWidth = rect.width - 30;
            var btnHeight = 50;
            var topOffset = btnHeight / 2 + 10f;

            var maxHeight = count * btnHeight + topOffset;
            _WaveMenuScrollDelta = count > 0 ? maxHeight / count : 0;
            var scrollBarRect = new Rect(listRect)
            {
                width = (listRect.width - btnWidth) / 2,
                position = Vector2.zero,
            };
            _WaveMenuPosition = GUI.VerticalScrollbar(scrollBarRect, _WaveMenuPosition, 200, 0, maxHeight);
            _WaveMenuRect = rect;
            for (int i = 0; i <= count; i++)
            {
                var waveId = mapCfg.nMinWaveId + i;
                var cfg = ExcelEditorUtil.GetCfg<BattleWaveCfg>(waveId);
                if (cfg == null)
                {
                    cfg = ExcelEditorUtil.CreateTypeInstance<BattleWaveCfg>();
                    ExcelEditorUtil.SetCfgValue(cfg, nameof(BattleWaveCfg.nWaveId), waveId);
                    ExcelEditorUtil.AddCfg(cfg);
                }
                var btnRect = new Rect()
                {
                    width = btnWidth,
                    height = btnHeight,
                    center = new Vector2()
                    {
                        x = listRect.width / 2,
                        y = i * btnHeight + topOffset - _WaveMenuPosition,
                    }
                };
                var col = GUI.backgroundColor;
                if (_CurWaveId == cfg.nWaveId)
                    GUI.backgroundColor = Color.cyan;
                if (GUI.Button(btnRect, $"{cfg.nWaveId}"))
                {
                    if (cfg.nWaveId == _CurWaveId)
                    {
                        SetWaveId(-1);
                    }
                    else
                    {
                        SetWaveId(cfg.nWaveId);
                    }

                }
                GUI.backgroundColor = col;
            }
        }
        GUI.EndClip();
    }
    #endregion

    #region map menu
    private float _MapMenuPosition = 0;
    private Rect _MapMenuRect;
    private float _MapMenuScrollDelta;
    private void DrawMapMenu(Rect rect)
    {
        var count = ExcelEditorUtil.GetCfgCount<MapCfg>();
        var btnWidth = rect.width - 30;
        var btnHeight = 50;
        var topOffset = btnHeight / 2 + 10f;

        var titleRect = new Rect()
        {
            size = new Vector2(rect.width - 5, 30),
            x = 5,
            y = 5,
        };
        GUI.Label(titleRect, "map");
        EditorGuiUtil.DrawBox(titleRect, "");

        var listRect = new Rect(rect)
        {
            y = titleRect.yMax + 5,
            height = rect.height - titleRect.yMax - 5 * 2,
        };
        EditorGuiUtil.DrawBox(listRect, "");
        GUI.BeginClip(listRect);
        {
            var maxHeight = count * btnHeight + topOffset;
            _MapMenuScrollDelta = count > 0 ? maxHeight / count : 0;
            var scrollBarRect = new Rect(listRect)
            {
                width = (listRect.width - btnWidth) / 2,
                position = Vector2.zero,
            };
            _MapMenuPosition = GUI.VerticalScrollbar(scrollBarRect, _MapMenuPosition, 200, 0, maxHeight);
            _MapMenuRect = rect;
            {
                for (int i = 0; i < count; i++)
                {
                    var cfg = ExcelEditorUtil.GetCfgByIndex<MapCfg>(i);
                    var btnRect = new Rect()
                    {
                        width = btnWidth,
                        height = btnHeight,
                        center = new Vector2()
                        {
                            x = listRect.width / 2,
                            y = i * btnHeight + topOffset - _MapMenuPosition,
                        }
                    };
                    var col = GUI.backgroundColor;
                    if (_MapId == cfg.nMapId)
                        GUI.backgroundColor = Color.cyan;
                    if (GUI.Button(btnRect, $"{cfg.nMapId}"))
                    {
                        SetWaveId(-1);
                        if (_MapId == cfg.nMapId)
                        {
                            SettingMapId(-1);
                        }
                        else
                        {
                            SettingMapId(cfg.nMapId);
                        }
                    }
                    GUI.backgroundColor = col;
                }
            }
        }
        GUI.EndClip();
    }
    #endregion

    #region wave monster
    private void SetWaveId(int waveId)
    {
        _CurWaveId = waveId;
        _LastSelectMonsterIndex = -1;
        if (waveId > 0)
        {
            if (!_GridIndex2Rect.ContainsKey(waveId))
            {
                var waveCfg = ExcelEditorUtil.GetCfg<BattleWaveCfg>(_CurWaveId);
                var index = 0;
                var userData = new CommonSerializeToIntArrayUserData()
                {
                    arrParams = waveCfg.arrMonsterInfos,
                    count = waveCfg.arrMonsterInfos?.Length ?? 0,
                    startIndex = 0,
                };
                var waveGridInfo = new WaveEditorInfo()
                {
                    gridInfos = new(),
                };
                ISerializeToIntArray.ParseToValue(userData.arrParams, userData.startIndex, userData.count, ref index, out waveGridInfo.monsterInfos, userData);
                _GridIndex2Rect.Add(waveId, waveGridInfo);
            }
        }
    }
    #endregion

    #region map grid
    private void SettingMapId(int mapId)
    {
        _MapId = mapId;
        if (mapId <= 0)
        {
            _GridSize = Vector2.zero;
            _MaxRow = 0;
            _MaxCol = 0;
        }
        else
        {
            var mapCfg = ExcelEditorUtil.GetCfg<MapCfg>(mapId);
            _GridSize = new Vector2(mapCfg.nGridSize[0], mapCfg.nGridSize[1]) * 100 * _Scale;
            _MaxRow = mapCfg.nGridRowCol[0];
            _MaxCol = mapCfg.nGridRowCol[1];
        }
    }

    private bool _IsShowGridInfos = false;
    private void DrawGrid(Rect rect)
    {
        // 导航栏
        var toolbarRect = new Rect()
        {
            x = 5,
            y = 0,
            width = rect.width,
            height = 50,
        };
        EditorGuiUtil.DrawBox(toolbarRect, "");
        GUI.BeginClip(toolbarRect);
        {
            DrawGridTopMenu(toolbarRect);
        }
        GUI.EndClip();

        var monsterMenuRect = new Rect()
        {
            x = 5,
            y = toolbarRect.yMax + 5,
            width = 100,
            height = rect.height - toolbarRect.height - 10,
        };
        EditorGuiUtil.DrawBox(monsterMenuRect, "");
        GUI.BeginClip(monsterMenuRect);

        var delRect = new Rect()
        {
            size = new Vector2(80, 80),
            center = new Vector2(monsterMenuRect.width / 2, 50),
        };
        var closeIcon = _CurMouseCtlMonsterIndex < 0
            ? EditorGUIUtility.IconContent("KnobCShape")
            : EditorGUIUtility.IconContent("d_redLight");
        GUI.DrawTexture(delRect, closeIcon.image);
        _CloseMonsterRect = new Rect(delRect)
        {
            position = delRect.position + monsterMenuRect.position + rect.position
        };

        _MonsterMenuDic.Clear();
        if (_MapId > 0)
        {
            var mapCfg = ExcelEditorUtil.GetCfg<MapCfg>(_MapId);
            var offset = new Vector2(10, 10);
            var maxWidth = monsterMenuRect.width - offset.x * 2;
            var iconRect = new Rect()
            {
                x = offset.x,
                y = delRect.yMax + 5,
            };
            for (int i = 0; i < mapCfg.arrMonsterIds.Length; i++)
            {
                var monsterId = mapCfg.arrMonsterIds[i];
                var monsterCfg = ExcelEditorUtil.GetCfg<MonsterCfg>(monsterId);
                var iconAssetCfg = ExcelEditorUtil.GetCfg<AssetCfg>(monsterCfg.nIconAssetID);
                if (iconAssetCfg != null)
                {
                    var icon = AssetDatabase.LoadAssetAtPath<Texture>(iconAssetCfg.strPath);
                    iconRect.width = icon.width;
                    iconRect.height = icon.height;
                    if (iconRect.width > maxWidth)
                    {
                        var scale = maxWidth / icon.width;
                        iconRect.width = maxWidth;
                        iconRect.height *= scale;
                    }
                    GUI.DrawTexture(iconRect, icon);

                    if (_CurWaveId > 0 && Event.current.type == EventType.MouseDown)
                    {
                        var globalRect = new Rect()
                        {
                            size = iconRect.size,
                            position = iconRect.position + monsterMenuRect.position + rect.position
                        };
                        _MonsterMenuDic.Add(monsterId, globalRect);
                    }
                    iconRect.y = iconRect.yMax + 5;
                }
            }
        }
        GUI.EndClip();


        var gridRect = new Rect()
        {
            x = monsterMenuRect.xMax + 5,
            y = toolbarRect.yMax + 5,
            width = rect.width - monsterMenuRect.width,
            height = monsterMenuRect.height,
        };
        EditorGuiUtil.DrawBox(gridRect, "");
        var localGridRects = new Dictionary<int, Rect>();
        GUI.BeginClip(gridRect);
        {
            var startPos = new Vector2()
            {
                x = gridRect.width / 2 - _MaxCol / 2f * _GridSize.x,
                y = gridRect.height / 2 + _MaxRow / 2f * _GridSize.y,
            };
            for (int row = 0; row < _MaxRow; row++)
            {
                var posY = startPos.y - (row + 0.5f) * _GridSize.y;
                for (int col = 0; col < _MaxCol; col++)
                {
                    var posX = startPos.x + (col + 0.5f) * _GridSize.x;
                    var btnRect = new Rect()
                    {
                        size = _GridSize - Vector2.one * 2,
                        center = new Vector2(posX, posY),
                    };
                    var index = RowCol2Index(row, col);
                    var globalRect = new Rect()
                    {
                        size = _GridSize,
                        position = btnRect.position + rect.position + gridRect.position,
                    };
                    localGridRects.Add(index, btnRect);
                    var gridInfos = GetCurWaveGridInfo(index);
                    if (gridInfos != null)
                    {
                        gridInfos.globalRect = globalRect;
                        gridInfos.monsterIndex = -1;
                    }
                    GUI.Box(btnRect, _IsShowGridInfos ? $"{index}\n{row},{col}" : "");
                }
            }
        }
        DrawWaveMonster(localGridRects);
        GUI.EndClip();
    }
    private void DrawGridTopMenu(Rect toolbarRect)
    {

        var col = GUI.backgroundColor;
        if (_IsShowGridInfos)
            GUI.backgroundColor = Color.cyan;
        var btnIndex = 0;
        var btnSize = new Vector2(100f, toolbarRect.height - 20);
        var btnRect = new Rect()
        {
            size = btnSize,
            center = new Vector2(btnSize.x * (0.5f + btnIndex++) + 5, toolbarRect.height / 2)
        };
        if (GUI.Button(btnRect, "grid info"))
        {
            _IsShowGridInfos = !_IsShowGridInfos;
        }
        GUI.backgroundColor = col;

        var saveBtnRect = new Rect(btnRect)
        {
            x = btnRect.xMax + 5
        };
        if (GUI.Button(saveBtnRect, "save"))
        {
            foreach (var (waveId, info) in _GridIndex2Rect)
            {
                var list = new List<int>();
                ISerializeToIntArray.SerializeToInt(ref list, info.monsterInfos);
                var cfg = ExcelEditorUtil.GetCfg<BattleWaveCfg>(waveId);
                ExcelEditorUtil.SetCfgValue(cfg, nameof(BattleWaveCfg.arrMonsterInfos), list.ToArray());
            }
            ExcelEditorUtil.SaveExcel<BattleWaveCfg>();
        }
        var copyBtnRect = new Rect(saveBtnRect)
        {
            x = saveBtnRect.xMax + 5
        };
        if (GUI.Button(copyBtnRect, "copy"))
        {
            _CopyWaveInfo = default;
            if (_GridIndex2Rect.TryGetValue(_CurWaveId, out var waveInfo))
            {
                _CopyWaveInfo.gridInfos = new();
                foreach (var (gridIndex, gridInfo) in waveInfo.gridInfos)
                {
                    _CopyWaveInfo.gridInfos.Add(gridIndex, new()
                    {
                        globalRect = gridInfo.globalRect,
                        monsterIndex = gridInfo.monsterIndex,
                    });
                }
                _CopyWaveInfo.monsterInfos = new();
                foreach (var monsterInfo in waveInfo.monsterInfos)
                {
                    _CopyWaveInfo.monsterInfos.Add(new()
                    {
                        attValuesId = monsterInfo.attValuesId,
                        backRow = monsterInfo.backRow,
                        col = monsterInfo.col,
                        energyCount = monsterInfo.energyCount,
                        monsterId = monsterInfo.monsterId,
                    });
                }
            }
        }
        var pasteBtnRect = new Rect(copyBtnRect)
        {
            x = copyBtnRect.xMax + 5
        };
        var enable = GUI.enabled;
        GUI.enabled &= _CopyWaveInfo.gridInfos != null;
        if (GUI.Button(pasteBtnRect, "paste"))
        {
            if (EditorUtility.DisplayDialog("paste", "", "ok", "cancel"))
            {
                if (_GridIndex2Rect.TryGetValue(_CurWaveId, out var waveInfo))
                {
                    foreach (var (gridIndex, gridInfo) in _CopyWaveInfo.gridInfos)
                    {
                        var curGridInfo = GetCurWaveGridInfo(gridIndex);
                        curGridInfo.globalRect = gridInfo.globalRect;
                        curGridInfo.monsterIndex = gridInfo.monsterIndex;
                    }
                    waveInfo.monsterInfos.Clear();
                    foreach (var monsterInfo in _CopyWaveInfo.monsterInfos)
                    {
                        waveInfo.monsterInfos.Add(new()
                        {
                            attValuesId = monsterInfo.attValuesId,
                            backRow = monsterInfo.backRow,
                            col = monsterInfo.col,
                            energyCount = monsterInfo.energyCount,
                            monsterId = monsterInfo.monsterId,
                        });
                    }
                }
                _LastSelectMonsterIndex = -1;
            }
        }
        GUI.enabled = enable;
        var clearBtnRect = new Rect(pasteBtnRect)
        {
            x = pasteBtnRect.xMax + 5
        };
        if (GUI.Button(clearBtnRect, "clear"))
        {
            if (EditorUtility.DisplayDialog("remove all monster", "", "ok", "cancel"))
            {
                if (_GridIndex2Rect.TryGetValue(_CurWaveId, out var waveInfo))
                {
                    foreach (var item in waveInfo.gridInfos)
                    {
                        item.Value.monsterIndex = -1;
                    }
                    waveInfo.monsterInfos.Clear();
                }
            }
        }
    }

    private void CheckMouseMenu()
    {
        if (Event.current.type == EventType.MouseDown)
        {
            if (!EditorUtil.MouseControllerIsNone())
                return;
            if (!Event.current.isMouse)
                return;
            if (Event.current.button != 0)
                return;
            foreach (var (monsterId, rect) in _MonsterMenuDic)
            {
                if (!EditorUtil.HasMouseInRect(rect))
                    continue;
                SettingCreateNewMonster(monsterId);
                _CopyMonsterInfo = new();
                break;
            }
        }
    }
    private void SettingCreateNewMonster(int monsterId)
    {
        _CurCtlMonsterId = monsterId;
        _MouseCtlOffset = Vector2.zero;
        _GridIndexOffset = 0;
        _CurMouseCtlMonsterIndex = -1;
        _CurIsValid = false;
        EditorUtil.AddMouseController(EnMouseControllerType.DebugWaveWindow);
    }
    private int RowCol2Index(int row, int col)
    {
        return row * _MaxCol + col;
    }
    public (int row, int col) Index2RowCol(int index)
    {
        return (index / _MaxCol, index % _MaxCol);
    }
    #endregion

    #region Draw map monster Info
    private void DrawWaveMonster(Dictionary<int, Rect> localGridRects)
    {
        if (_MapId <= 0)
            return;
        if (_CurWaveId <= 0)
            return;
        var monsterInfos = GetMonsterInfos();
        foreach (var item in _GridIndex2Rect[_CurWaveId].gridInfos)
        {
            item.Value.monsterIndex = -1;
        }
        for (int i = 0; i < monsterInfos.Count; i++)
        {
            var info = monsterInfos[i];
            var row = _MaxRow - info.backRow;
            var col = info.col;
            var index = RowCol2Index(row, col);
            var rect = localGridRects[index];

            var monsterCfg = ExcelEditorUtil.GetCfg<MonsterCfg>(info.monsterId);
            var assCfg = ExcelEditorUtil.GetCfg<AssetCfg>(monsterCfg.nIconAssetID);
            var icon = AssetDatabase.LoadAssetAtPath<Texture>(assCfg.strPath);

            var iconRect = new Rect()
            {
                position = rect.position,
                size = new Vector2(icon.width, icon.height) * _Scale,
            };

            GUI.DrawTexture(iconRect, icon, ScaleMode.ScaleToFit);
            if (_CurMouseCtlMonsterIndex == i)
            {
                EditorGUI.DrawRect(iconRect, Color.white * 0.7f);
            }
            if (_LastSelectMonsterIndex == i)
            {
                var infoTipsImg = EditorGUIUtility.IconContent("SoftlockProjectBrowser Icon");
                var tipsImgRect = new Rect(iconRect)
                {
                    size = Vector2.one * 20,
                };
                GUI.DrawTexture(tipsImgRect, infoTipsImg.image);
            }

            for (int j = 0; j < monsterCfg.arrGridLocalPos.Length; j += 2)
            {
                var monsterRow = row + monsterCfg.arrGridLocalPos[j];
                var monsterCol = col + monsterCfg.arrGridLocalPos[j + 1];
                var monsterIndex = RowCol2Index(monsterRow, monsterCol);
                var gridData = GetCurWaveGridInfo(monsterIndex);
                gridData.monsterIndex = i;
            }
        }
    }
    #endregion

    #region mouse controller
    private int _CurMouseCtlMonsterIndex = -1;
    private int _CurCtlMonsterId = -1;
    private Vector2 _MouseCtlOffset;
    private int _GridIndexOffset = -1;
    private bool _CurIsValid;
    private int _ValidGridIndex;
    private void CheckMouseController()
    {
        if (Event.current.type == EventType.Layout)
            return;
        if (_CurWaveId <= 0)
            return;
        if (EditorUtil.MouseControllerIsNone()
            && Event.current.isMouse
            && Event.current.button == 0
            && Event.current.type == EventType.MouseDown)
        {
            CheckMouse();
        }
        if (Event.current.isMouse
            && Event.current.type == EventType.MouseUp
            && EditorUtil.MouseControllerIs(EnMouseControllerType.DebugWaveWindow))
        {
            ApplyMonster();
        }
        if (EditorUtil.MouseControllerIs(EnMouseControllerType.DebugWaveWindow))
        {
            CheckCurSelectMonster();
        }
    }
    private void ApplyMonster()
    {
        if (_CurIsValid)
        {
            var monsterCfg = ExcelEditorUtil.GetCfg<MonsterCfg>(_CurCtlMonsterId);

            if (_CurMouseCtlMonsterIndex > -1)
            {
                Clear(_CurMouseCtlMonsterIndex, _CurCtlMonsterId);
            }
            else
            {
                var monsterList = GetMonsterInfos();
                var info = new GameBattleWaveMonsterInfo()
                {
                    monsterId = _CurCtlMonsterId,
                    attValuesId = _CopyMonsterInfo.attValuesId,
                    energyCount = _CopyMonsterInfo.energyCount,
                };
                monsterList.Add(info);
                _CurMouseCtlMonsterIndex = monsterList.Count - 1;
            }
            var (row, col) = Index2RowCol(_ValidGridIndex);
            var monsterData = GetMonsterInfos(_CurMouseCtlMonsterIndex);
            monsterData.backRow = _MaxRow - row;
            monsterData.col = col;

            for (int i = 0; i < monsterCfg.arrGridLocalPos.Length; i += 2)
            {
                var checkRow = monsterCfg.arrGridLocalPos[i] + row;
                var checkCol = monsterCfg.arrGridLocalPos[i + 1] + col;
                var checkIndex = RowCol2Index(checkRow, checkCol);
                var gridData = GetCurWaveGridInfo(checkIndex);
                gridData.monsterIndex = _CurCtlMonsterId;
            }

            _LastSelectMonsterIndex = _CurMouseCtlMonsterIndex;
        }
        else
        {
            if (EditorUtil.HasMouseInRect(_CloseMonsterRect))
            {
                if (_CurMouseCtlMonsterIndex >= 0)
                {
                    Clear(_CurMouseCtlMonsterIndex, _CurCtlMonsterId);

                    var infos = GetMonsterInfos();
                    infos.RemoveAt(_CurMouseCtlMonsterIndex);
                    if (_LastSelectMonsterIndex == _CurMouseCtlMonsterIndex)
                    {
                        _LastSelectMonsterIndex = -1;
                    }
                    else if (_LastSelectMonsterIndex > _CurMouseCtlMonsterIndex)
                    {
                        _LastSelectMonsterIndex--;
                    }
                }
            }
        }
        void Clear(int monsterIndex, int monsterId)
        {
            var monsterCfg = ExcelEditorUtil.GetCfg<MonsterCfg>(monsterId);
            var info = GetMonsterInfos(monsterIndex);
            var (lastRow, lastCol) = (_MaxCol - info.backRow, info.col);
            for (int i = 0; i < monsterCfg.arrGridLocalPos.Length; i += 2)
            {
                var checkRow = monsterCfg.arrGridLocalPos[i] + lastRow;
                var checkCol = monsterCfg.arrGridLocalPos[i + 1] + lastCol;
                var checkIndex = RowCol2Index(checkRow, checkCol);
                var gridData = GetCurWaveGridInfo(checkIndex);
                gridData.monsterIndex = -1;
            }
        }
        EditorUtil.ClearMouseController(EnMouseControllerType.DebugWaveWindow);
        _CurMouseCtlMonsterIndex = -1;
    }
    private void CheckCurSelectMonster()
    {
        if (_CurCtlMonsterId > 0)
        {
            var pos = EditorUtil.GetEditorMousePos(_MouseCtlOffset);
            var monsterCfg = ExcelEditorUtil.GetCfg<MonsterCfg>(_CurCtlMonsterId);
            var assCfg = ExcelEditorUtil.GetCfg<AssetCfg>(monsterCfg.nIconAssetID);
            var icon = AssetDatabase.LoadAssetAtPath<Texture>(assCfg.strPath);
            var iconRect = new Rect()
            {
                position = pos,
                size = new Vector2(icon.width, icon.height) * _Scale,
            };
            //Rect previewRect = EditorGUILayout.GetControlRect(false, 250);
            GUI.DrawTexture(iconRect, icon, ScaleMode.ScaleToFit);
        }
        var curMousePos = EditorUtil.GetEditorMousePos(Vector2.zero);
        _CurIsValid = false;
        if (TryGetGridIndex(curMousePos, out var gridIndex))
        {
            var mainIndex = _GridIndexOffset + gridIndex;
            if (CurWaveGridIsValid(mainIndex))
            {
                var monsterCfg = ExcelEditorUtil.GetCfg<MonsterCfg>(_CurCtlMonsterId);
                var (mouseRow, mouseCol) = Index2RowCol(mainIndex);
                _CurIsValid = true;
                _ValidGridIndex = mainIndex;
                for (int i = 0; i < monsterCfg.arrGridLocalPos.Length; i += 2)
                {
                    var row = monsterCfg.arrGridLocalPos[i] + mouseRow;
                    var col = monsterCfg.arrGridLocalPos[i + 1] + mouseCol;
                    if (row < 0 || row >= _MaxRow || col < 0 || col >= _MaxCol)
                    {
                        _CurIsValid = false;
                        continue;
                    }
                    var checkIndex = RowCol2Index(row, col);
                    var gridData = GetCurWaveGridInfo(checkIndex);
                    if (gridData.monsterIndex >= 0 && gridData.monsterIndex != _CurMouseCtlMonsterIndex)
                    {
                        EditorGUI.DrawRect(gridData.globalRect, Color.red * 0.7f);
                        _CurIsValid = false;
                    }
                    else
                    {
                        EditorGUI.DrawRect(gridData.globalRect, Color.green * 0.7f);
                    }
                }
            }
        }
    }
    private void CheckMouse()
    {
        if (!TryGetMouseGridIndex(out var gridIndex))
            return;
        var gridData = GetCurWaveGridInfo(gridIndex);
        if (gridData.monsterIndex >= 0)
        {
            _LastSelectMonsterIndex = gridData.monsterIndex;
            _CurMouseCtlMonsterIndex = gridData.monsterIndex;
            var monsterData = GetMonsterInfos(gridData.monsterIndex);
            _CurCtlMonsterId = monsterData.monsterId;
            var mainIndex = RowCol2Index(_MaxRow - monsterData.backRow, monsterData.col);
            var mainPos = GetCurWaveGridInfo(mainIndex);
            var curMousePos = EditorUtil.GetEditorMousePos(Vector2.zero);
            _MouseCtlOffset = mainPos.globalRect.position - curMousePos;
            _GridIndexOffset = mainIndex - gridIndex;
            EditorUtil.AddMouseController(EnMouseControllerType.DebugWaveWindow);
        }
    }
    private bool TryGetGridIndex(Vector2 pos, out int gridIndex)
    {
        var gridInfos = _GridIndex2Rect[_CurWaveId];
        foreach (var item in gridInfos.gridInfos)
        {
            if (!item.Value.globalRect.Contains(pos))
                continue;
            gridIndex = item.Key;
            return true;
        }
        gridIndex = -1;
        return false;
    }
    private bool TryGetMouseGridIndex(out int gridIndex)
    {
        var gridInfos = _GridIndex2Rect[_CurWaveId];
        foreach (var item in gridInfos.gridInfos)
        {
            if (!EditorUtil.HasMouseInRect(item.Value.globalRect))
                continue;
            gridIndex = item.Key;
            return true;
        }
        gridIndex = -1;
        return false;
    }
    #endregion

    #region select monster info
    private int _LastSelectMonsterIndex = -1;
    private void DrawMonsterInfo(Rect rect)
    {
        var monsterInfo = GetMonsterInfos(_LastSelectMonsterIndex);

        var offset = new Vector2(10, 10);
        var closeBtnRect = new Rect()
        {
            width = 30,
            height = 30,
            x = rect.width - offset.x - 30,
            y = offset.y,
        };
        var tipsRect = new Rect()
        {
            width = 30,
            height = 30,
            x = 10,
            y = offset.y,
        };
        var infoTipsImg = EditorGUIUtility.IconContent("SoftlockProjectBrowser Icon");
        GUI.DrawTexture(tipsRect, infoTipsImg.image);

        var copyRect = new Rect(tipsRect)
        {
            x = tipsRect.xMax + 5,
            width = 50,
        };
        if (GUI.Button(copyRect, "Copy"))
        {
            _CopyMonsterInfo = new()
            {
                attValuesId = monsterInfo.attValuesId,
                energyCount = monsterInfo.energyCount,
            };
        }
        var pasteRect = new Rect(copyRect)
        {
            x = copyRect.xMax + 5,
        };
        var enable = GUI.enabled;
        GUI.enabled &= _CopyMonsterInfo != null && _CopyMonsterInfo.attValuesId > 0;
        if (_CopyMonsterInfo != null && _CopyMonsterInfo.attValuesId > 0)
        {
            if (GUI.Button(pasteRect, "Paste"))
            {
                monsterInfo.attValuesId = _CopyMonsterInfo.attValuesId;
                monsterInfo.energyCount = _CopyMonsterInfo.energyCount;
            }
        }
        GUI.enabled = enable;

        var closeIcon = EditorGUIUtility.IconContent("d_redLight");
        if (GUI.Button(closeBtnRect, closeIcon))
        {
            _LastSelectMonsterIndex = -1;
            return;
        }

        var monsterCfg = ExcelEditorUtil.GetCfg<MonsterCfg>(monsterInfo.monsterId);
        var iconAssCfg = ExcelEditorUtil.GetCfg<AssetCfg>(monsterCfg.nIconAssetID);
        var icon = AssetDatabase.LoadAssetAtPath<Texture>(iconAssCfg.strPath);
        var iconRect = new Rect()
        {
            x = offset.x,
            y = closeBtnRect.yMax + 10,
            width = icon.width,
            height = icon.height,
        };
        var maxSize = 150;
        if (iconRect.width > maxSize)
        {
            var scale = maxSize / iconRect.width;
            iconRect.width = maxSize;
            iconRect.height *= scale;
        }
        if (iconRect.height > maxSize)
        {
            var scale = maxSize / iconRect.height;
            iconRect.height = maxSize;
            iconRect.width *= scale;
        }
        GUI.DrawTexture(iconRect, icon);
        var monsterIdRect = new Rect(iconRect)
        {
            x = iconRect.xMax + 10,
            height = 20,
        };
        var monsterNameRect = new Rect(monsterIdRect)
        {
            y = monsterIdRect.yMax + 10,
        };
        GUI.Label(monsterIdRect, $"{monsterInfo.monsterId}");
        var monsterNameAssetCfg = ExcelEditorUtil.GetCfg<LanguageCfg>(monsterCfg.nNameLangId);
        GUI.Label(monsterNameRect, $"{monsterNameAssetCfg.strValueCh}");


        var energyNameRect = new Rect()
        {
            x = offset.x,
            y = iconRect.yMin + maxSize + 10,
            width = rect.width * 0.3f,
            height = 20,
        };
        var energyValueRect = new Rect(energyNameRect)
        {
            x = energyNameRect.xMax,
            width = rect.width - energyNameRect.width - offset.x * 2,
        };
        GUI.Label(energyNameRect, "击杀能量");
        var changeTxt = GUI.TextField(energyValueRect, $"{monsterInfo.energyCount}");
        if (int.TryParse(changeTxt, out var valueInt))
            monsterInfo.energyCount = valueInt;


        var gridNameRect = new Rect(energyNameRect)
        {
            y = energyNameRect.yMax + 10,
        };
        var gridValueRect1 = new Rect(energyValueRect)
        {
            y = energyNameRect.yMax + 10,
            width = energyValueRect.width / 2,
        };
        var gridValueRect2 = new Rect(gridValueRect1)
        {
            x = gridValueRect1.xMax,
        };
        GUI.Label(gridNameRect, "位置");
        GUI.Label(gridValueRect1, $"{_MaxRow - monsterInfo.backRow}");
        GUI.Label(gridValueRect2, $"{monsterInfo.col}");


        var attIdNameRect = new Rect(gridNameRect)
        {
            y = gridNameRect.yMax + 10,
        };
        var attIdValueRect = new Rect(energyValueRect)
        {
            y = gridNameRect.yMax + 10,
        };
        GUI.Label(attIdNameRect, "属性组");
        EditorGuiUtil.DrawCfgField<AttributeValueCfg>(attIdValueRect, monsterInfo.attValuesId, selectId =>
        {
            monsterInfo.attValuesId = selectId;
        });
        var attValueOffset = new Vector2(20, 10);
        var attValueCfg = ExcelEditorUtil.GetCfg<AttributeValueCfg>(monsterInfo.attValuesId);
        if (attValueCfg != null)
        {
            var attNameWidth = (rect.width - offset.x * 2 - attValueOffset.x) * 0.3f;
            var attValueWidth = rect.width - attNameWidth - offset.x * 2 - attValueOffset.x;
            var attNameRect = new Rect()
            {
                x = attValueOffset.x + attIdNameRect.xMin,
                y = attValueOffset.y + attIdNameRect.yMax,
                width = attNameWidth,
                height = 20,
            };
            var attValueRect = new Rect(attNameRect)
            {
                x = attNameRect.xMax,
                width = attValueWidth,
            };
            var attCfgType = typeof(AttributeValueCfg);
            for (var attId = EnEntityAttributeId.None + 1; attId < EnEntityAttributeId.EnumCount; attId++)
            {
                var fieldName = $"n{attId}";
                var field = attCfgType.GetField(fieldName, BindingFlags.Public | BindingFlags.Instance);
                if (field == null)
                    continue;
                var value = field.GetValue(attValueCfg);
                var attCfg = ExcelEditorUtil.GetCfg<AttributeCfg>((int)attId);
                var langCfg = ExcelEditorUtil.GetCfg<LanguageCfg>(attCfg.nNameLangId);
                var addNameStr = attCfg.nValueType == (int)EnValueType.Percentage ? "(%)" : "";
                GUI.Label(attNameRect, $"{langCfg.strValueCh}" + addNameStr);
                changeTxt = GUI.TextField(attValueRect, $"{value}");
                if (typeof(int) == field.FieldType)
                {
                    if (int.TryParse(changeTxt, out valueInt) && !value.Equals(valueInt))
                        ExcelEditorUtil.SetCfgValue(attValueCfg, fieldName, valueInt);
                }
                else if (typeof(float) == field.FieldType)
                {
                    if (float.TryParse(changeTxt, out var valueFloat) && !value.Equals(valueFloat))
                        ExcelEditorUtil.SetCfgValue(attValueCfg, fieldName, valueFloat);
                }
                attNameRect.y = attNameRect.yMax + 5;
                attValueRect.y = attValueRect.yMax + 5;
            }
        }
    }

    #endregion

    #region copy monster
    private void CopyMapMonster()
    {
        if (Event.current.type != EventType.KeyDown)
            return;
        if (Event.current.keyCode == KeyCode.E)
        {
            if (_LastSelectMonsterIndex < 0)
                return;
            var monsterInfo = GetMonsterInfos(_LastSelectMonsterIndex);
            SettingCreateNewMonster(monsterInfo.monsterId);
            _CopyMonsterInfo = new()
            {
                energyCount = monsterInfo.energyCount,
                attValuesId = monsterInfo.attValuesId,
            };
        }
    }
    #endregion
}
