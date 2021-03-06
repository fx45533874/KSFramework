﻿using UnityEngine;
using System.IO;
using AppSettings;
using KEngine;
using KEngine.Editor;
using KSFramework.Editor;
using UnityEditor;

public class KQuickWindowEditor : EditorWindow
{
    Vector3 scrollPos = Vector2.zero;
    [SerializeField] private string reloadUIAb;
    [SerializeField] private string reloadUIScript;
    private GUIStyle textFieldStyle;
    private bool init = false;

    [MenuItem("KEngine/Open Quick Window %&Q")]
    static void DoIt()
    {
        //note 在Editor下Scree.Width,Screen.Height获取的并不是真实的显示器宽度和高度
        KQuickWindowEditor window = EditorWindow.GetWindow<KQuickWindowEditor>();
        window.titleContent = new GUIContent("快捷工具窗");
        window.position = new Rect(400, 100, 640, 480);
        window.Show();
    }

    void InitStyle()
    {
        if (init)
            return;
        textFieldStyle = new GUIStyle(GUI.skin.textField);
        textFieldStyle.alignment = TextAnchor.MiddleLeft;

        init = true;
    }

    public void OnGUI()
    {
        InitStyle();
        scrollPos = GUI.BeginScrollView(new Rect(10, 10, position.width, position.height), scrollPos, new Rect(0, 0, 360, 1000));
        DrawKEngineInit();
        GUILayout.Space(20);

        DrawHotReLoadUI();
        GUILayout.Space(20);

        DrawKEngineUI();
        GUILayout.Space(20);

        DrawAssetBundleUI();
        GUILayout.Space(20);

        DrawBuildUI();
        GUILayout.Space(20);

        DrawDebugUI();
        GUILayout.Space(20);
        GUI.EndScrollView();
    }


    public void DrawKEngineInit()
    {
        GUILayout.BeginHorizontal("HelpBox");
        EditorGUILayout.LabelField("== KEngine ==");
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("初始化资源链接", GUILayout.ExpandWidth(true), GUILayout.MaxHeight(30)))
        {
            ResourcesSymbolLinkHelper.SymbolLinkResource();
        }
        if (GUILayout.Button("删除资源链接", GUILayout.ExpandWidth(true), GUILayout.MaxHeight(30)))
        {
            ResourcesSymbolLinkHelper.RemoveSymbolLinkResource();
        }
        GUILayout.EndHorizontal();
    }

    public void DrawHotReLoadUI()
    {
        GUILayout.BeginHorizontal("HelpBox");
        EditorGUILayout.LabelField("== 热重载 ==");
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("重载配置表", GUILayout.ExpandWidth(true), GUILayout.MaxHeight(30)))
        {
            SettingsManager.AllSettingsReload();
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("快速编译配置表", GUILayout.ExpandWidth(true), GUILayout.MaxHeight(30)))
        {
            SettingModuleEditor.QuickCompileSettings();
        }

        if (GUILayout.Button("编译配置表并生成代码", GUILayout.ExpandWidth(true), GUILayout.MaxHeight(30)))
        {
            SettingModuleEditor.CompileSettings();
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("重载所有UI的Lua代码", GUILayout.ExpandWidth(true), GUILayout.MaxHeight(30)))
        {
            KSFrameworkEditor.ReloadAllUIScript();
        }

        if (GUILayout.Button("重新打开所有UI", GUILayout.ExpandWidth(true), GUILayout.MaxHeight(30)))
        {
            KSFrameworkEditor.ReloadAllUI();
        }

        GUILayout.EndHorizontal();

        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("UI名字:", GUILayout.Width(40));
        reloadUIScript = EditorGUILayout.TextField(reloadUIScript, GUILayout.MinWidth(120),GUILayout.MaxHeight(30));
        if (GUILayout.Button("重载Lua脚本", GUILayout.MinWidth(100), GUILayout.MaxHeight(30)))
        {
            KSFrameworkEditor.ReloadUIScript(reloadUIScript);
        }

        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("UI名字:", GUILayout.Width(40));
        reloadUIAb = EditorGUILayout.TextField(reloadUIAb, GUILayout.MinWidth(120),GUILayout.MaxHeight(30));
        if (GUILayout.Button("重载AB并打开", GUILayout.MinWidth(100), GUILayout.MaxHeight(30)))
        {
            KSFrameworkEditor.ReloadUIAB(reloadUIAb);
        }

        GUILayout.EndHorizontal();
        //GUILayout.Space(10);
    }

    public void DrawKEngineUI()
    {
        GUILayout.BeginHorizontal("HelpBox");
        EditorGUILayout.LabelField("== UI相关辅助 ==");
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("创建UI", GUILayout.ExpandWidth(true), GUILayout.MaxHeight(30)))
        {
            KUGUIBuilder.CreateNewUI();
        }

        if (GUILayout.Button("为当前UI创建Lua脚本", GUILayout.ExpandWidth(true), GUILayout.MaxHeight(30)))
        {
            KSFrameworkEditor.AutoMakeUILuaScripts();
        }

        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();
        if (GUILayout.Button("导出当前UI", GUILayout.ExpandWidth(true), GUILayout.MaxHeight(30)))
        {
            KUGUIBuilder.ExportCurrentUI();
        }

        if (GUILayout.Button("导出全部UI", GUILayout.ExpandWidth(true), GUILayout.MaxHeight(30)))
        {
            KUGUIBuilder.ExportAllUI();
        }

        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("导出公共图集", GUILayout.ExpandWidth(true), GUILayout.MaxHeight(30)))
        {
            KUGUIBuilder.BuildCommonAtlas();
        }
        
        GUILayout.EndHorizontal();
    }

    public void DrawAssetBundleUI()
    {
        GUILayout.BeginHorizontal("HelpBox");
        EditorGUILayout.LabelField("== Assetbundle相关 ==");
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("打包AB", GUILayout.ExpandWidth(true), GUILayout.MaxHeight(30)))
        {
            BuildTools.BuildAllAssetBundles();
        }

        if (GUILayout.Button("删除全部AB并重新打包", GUILayout.ExpandWidth(true), GUILayout.MaxHeight(30)))
        {
            BuildTools.ReBuildAllAssetBundles();
        }

        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();
        if (GUILayout.Button("设置AB Name", GUILayout.ExpandWidth(true), GUILayout.MaxHeight(30)))
        {
            BuildTools.MakeAssetBundleNames();
        }
        if (GUILayout.Button("清空全部AB Name", GUILayout.ExpandWidth(true), GUILayout.MaxHeight(30)))
        {
            BuildTools.ClearAssetBundleNames();
        }
        if (GUILayout.Button("打开AB目录", GUILayout.ExpandWidth(true), GUILayout.MaxHeight(20)))
        {
            var path = AppConfig.ProductRelPath + "/Bundles/" + KResourceModule.GetBuildPlatformName();
            OpenFolder(path);
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("清理冗余资源", GUILayout.ExpandWidth(true), GUILayout.MaxHeight(30)))
        {
            BuildTools.CleanAssetBundlesRedundancies();
        }

        GUILayout.EndHorizontal();
    }

    void DrawBuildUI()
    {
        GUILayout.BeginHorizontal("HelpBox");
        EditorGUILayout.LabelField("== 生成安装包 ==");
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("打PC版", GUILayout.ExpandWidth(true), GUILayout.MaxHeight(20)))
        {
            KAutoBuilder.PerformWinReleaseBuild();
        }

        if (GUILayout.Button("打PC版-Dev", GUILayout.ExpandWidth(true), GUILayout.MaxHeight(20)))
        {
            KAutoBuilder.PerformWinBuild();
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("打Android版", GUILayout.ExpandWidth(true), GUILayout.MaxHeight(20)))
        {
            KAutoBuilder.PerformAndroidBuild();
        }

        if (GUILayout.Button("打IOS版", GUILayout.ExpandWidth(true), GUILayout.MaxHeight(20)))
        {
            KAutoBuilder.PerformiOSBuild();
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("打开安装包目录", GUILayout.ExpandWidth(true), GUILayout.MaxHeight(20)))
        {
            var path = AppConfig.ProductRelPath + "/Apps/" + KResourceModule.GetBuildPlatformName();
            OpenFolder(path);
        }

        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();


        GUILayout.EndHorizontal();
    }

    void DrawDebugUI()
    {
        GUILayout.BeginHorizontal("HelpBox");
        EditorGUILayout.LabelField("== Debug Module ==");
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Dump", GUILayout.ExpandWidth(true), GUILayout.MaxHeight(20)))
        {
            KProfiler.Dump();
        }
        if (GUILayout.Button("打开AB加载耗时记录", GUILayout.ExpandWidth(true), GUILayout.MaxHeight(20)))
        {
            OpenFolder(Application.persistentDataPath);
        }


        GUILayout.EndHorizontal();
    }

    void OpenFolder(string path)
    {
        var fullPath = Path.GetFullPath(path);
        if (Directory.Exists(fullPath) == false)
        {
            Log.Info("{0} 目录不存在，尝试定位到父目录。", fullPath);

            DirectoryInfo directoryInfo = new DirectoryInfo(fullPath);
            if (directoryInfo.Parent != null) fullPath = directoryInfo.Parent.FullName;
        }

        Log.Info("open: {0}", fullPath);
        System.Diagnostics.Process.Start("explorer.exe", fullPath);
    }
}