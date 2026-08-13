using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomUnityToolbarAttribute("ExportExcel", EnCustomUnityToolbarType.Left, EnCustomUnityToolbarOrder.ExportExcel)]
public class ExportExcelToolbar : ICustomUnityToolbar
{
    public void OnClick()
    {
                string[] arrParams = new string[]
                    {
                        Path.Combine(ABBUtil.GetUnityMiscPath(), "Excel"),
                        Path.Combine(ABBUtil.GetDataPath(), "GameRes/HotfixRes", "GameCfgJson"),
                        Path.Combine(ABBUtil.GetDataPath(), "Framework/Hotfix/Scripts", "GameCfgCS"),
                    };
        var paramsStr = "";
        for (int i = 0; i < arrParams.Length; i++)
        {
            paramsStr += arrParams[i] + " ";
        }
        var pro2 = new ProcessStartInfo()
        {
#if UNITY_EDITOR_OSX
            FileName = Path.Combine(ABBUtil.GetUnityRootPath(), GlobalConfigEditorSO.Instance.MacExcelToolsPath),
#else
            FileName = Path.Combine(ABBUtil.GetUnityRootPath(), GlobalConfigEditorSO.Instance.ExcelToolsPath),
#endif
            RedirectStandardOutput = true, // 重定向标准输出
            UseShellExecute = false, // 不使用系统外壳程序启动
            CreateNoWindow = true, // 不创建新窗口
            Arguments = paramsStr,
            ErrorDialog = true,
            //StandardErrorEncoding = System.Text.Encoding.UTF8,
            RedirectStandardError = true,
            StandardOutputEncoding = System.Text.Encoding.UTF8,
            //RedirectStandardInput = true,
        };
        pro2.UseShellExecute = false;
        using (var process = Process.Start(pro2))
        {
            process.OutputDataReceived += (item, e) =>
            {
                if (string.IsNullOrEmpty(e.Data))
                    return;
                UnityEngine.Debug.Log($"{item},{e.Data}"); // 打印结果
            };
            process.ErrorDataReceived += (item, e) =>
            {
                if (string.IsNullOrEmpty(e.Data))
                    return;
                UnityEngine.Debug.LogError($"{item},{e.Data}"); // 打印结果
            };

            process.BeginOutputReadLine();

            process.BeginErrorReadLine();

            process.WaitForExit();
            UnityEngine.Debug.Log($"Export Excel Finish"); // 打印结果
        }
        AssetDatabase.Refresh();

//        string[] arrParams = new string[]
//{
//    Path.Combine(ABBUtil.GetUnityMiscPath(), "Excel"),
//    Path.Combine(ABBUtil.GetDataPath(), "GameRes/HotfixRes", "GameCfgJson"),
//    Path.Combine(ABBUtil.GetDataPath(), "Framework/Hotfix/Scripts", "GameCfgCS"),
//};

//        // 1. 给每个参数包裹双引号，处理路径空格问题
//        string paramsStr = string.Join(" ", arrParams.Select(p => $"\"{p}\""));

//        string toolPath;
//#if UNITY_EDITOR_OSX
//        toolPath = Path.Combine(ABBUtil.GetUnityRootPath(), GlobalConfigEditorSO.Instance.MacExcelToolsPath);
//#else
//    toolPath = Path.Combine(ABBUtil.GetUnityRootPath(), GlobalConfigEditorSO.Instance.ExcelToolsPath);
//#endif

//        ProcessStartInfo pro2;

//#if UNITY_EDITOR_OSX
//        // Mac 下通过 AppleScript 调用 Terminal.app 弹出终端执行
//        string shellCommand = $"\"{toolPath}\" {paramsStr}";
//        // 转义命令中的双引号，适配 AppleScript 语法
//        string escapedCommand = shellCommand.Replace("\"", "\\\"");

//        pro2 = new ProcessStartInfo()
//        {
//            FileName = "/usr/bin/osascript",
//            Arguments = $"-e 'tell application \"Terminal\" to do script \"{escapedCommand}\"'",
//            UseShellExecute = false,
//            CreateNoWindow = true, // osascript 本身不创建窗口
//            RedirectStandardOutput = true,
//            RedirectStandardError = true,
//            StandardOutputEncoding = System.Text.Encoding.UTF8,
//            StandardErrorEncoding = System.Text.Encoding.UTF8,
//        };
//#else
//    // Windows 下保持原有逻辑，CreateNoWindow=false 会弹出控制台
//    pro2 = new ProcessStartInfo()
//    {
//        FileName = toolPath,
//        Arguments = paramsStr,
//        RedirectStandardOutput = true,
//        UseShellExecute = false,
//        CreateNoWindow = false,
//        RedirectStandardError = true,
//        StandardOutputEncoding = System.Text.Encoding.UTF8,
//        ErrorDialog = true,
//    };
//#endif

//        using (var process = Process.Start(pro2))
//        {
//            // 仅非 Mac 模式下，重定向输出到 Unity 控制台生效
//#if !UNITY_EDITOR_OSX
//    process.OutputDataReceived += (item, e) =>
//    {
//        if (string.IsNullOrEmpty(e.Data))
//            return;
//        UnityEngine.Debug.Log($"{item},{e.Data}");
//    };
//    process.ErrorDataReceived += (item, e) =>
//    {
//        if (string.IsNullOrEmpty(e.Data))
//            return;
//        UnityEngine.Debug.LogError($"{item},{e.Data}");
//    };

//    process.BeginOutputReadLine();
//    process.BeginErrorReadLine();
//#endif

//            // 注意：WaitForExit 会阻塞主线程，工具耗时久建议改成异步回调
//            process.WaitForExit();

//            UnityEngine.Debug.Log("Export Excel Finish");
//        }

//        AssetDatabase.Refresh();

    }

}
