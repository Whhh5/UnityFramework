using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class DebugWindow : EditorWindow
{
    private Dictionary<int, GameObject> _GameObjects = new();

    private List<int> _TempEntityID = new();
    private Vector3 pos;
    private float radius;

    private Vector2 inDir;
    private Vector2 normalDir;

    private int _EntityId;
    private EnEntityCmd _CmdId;


    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        {
            _EntityId = EditorGUILayout.IntField(_EntityId, GUILayout.Width(100));
            _CmdId = (EnEntityCmd)EditorGUILayout.EnumPopup(_CmdId, GUILayout.Width(100));
            if (GUILayout.Button("exe", GUILayout.Width(100), GUILayout.Height(30)))
            {
                SceneEntityMgr.Instance.AddCmd(_EntityId, _CmdId);
            }
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("HashCode", GUILayout.Width(100), GUILayout.Height(30)))
        {
            if (Selection.assetGUIDs.Length > 0)
            {
                var logStr = new StringBuilder();
                for (int i = 0; i < Selection.assetGUIDs.Length; i++)
                {
                    var guid = Selection.assetGUIDs[i];
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    var hasCode = path.GetHashCode();
                    logStr.AppendLine($"{path}:{hasCode}");
                }
                Debug.Log(logStr);
            }
        }
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("ab info", GUILayout.Width(100), GUILayout.Height(30)))
            {
                var loader = ABBLoadMgr.Instance.GetLoader();
                loader.Log();
            }
            if (GUILayout.Button("clear windows", GUILayout.Width(100), GUILayout.Height(30)))
            {
                UIMgr.Instance.HideAllWindow();
            }
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("upload persistent", GUILayout.Width(100), GUILayout.Height(30)))
            {
                PersistentMgr.Instance.UploadPeresistentData();
            }
        }

        inDir = EditorGUILayout.Vector3Field("inDir", inDir);
        normalDir = EditorGUILayout.Vector3Field("normalDir", normalDir);
        if (GUILayout.Button("Reflect", GUILayout.Width(100), GUILayout.Height(30)))
        {
            var dir = Vector2.Reflect(inDir.normalized, normalDir.normalized);
            Debug.Log(dir);
        }
    }

}