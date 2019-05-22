using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DefinesEditorWindow : EditorWindow
{
	private readonly List<string> _commonDefines = new List<string>
	{
		"DEBUG_INAPP", "RECEIPT_VALIDATION", "DEBUG_ADS", "ADS_APPODEAL", "ADS_APPODEAL_SMART_BANNERS",
		"TEST_PUSH"
	};
	
	private string _newDefineIos;
	private string _newDefineAndroid;
	private string _newDefineStandalone;
	
	[MenuItem ("DevGame/Editor/Defines editor")]
	private static void OpenDefinesEditorWindow()
	{
		GetWindow(typeof(DefinesEditorWindow));
	}
	
	protected virtual void OnGUI()
	{
		EditorGUILayout.BeginHorizontal();
		DrawBuildTargetGroup(BuildTargetGroup.iOS, ref _newDefineIos);
		DrawBuildTargetGroup(BuildTargetGroup.Android, ref _newDefineAndroid);
		DrawBuildTargetGroup(BuildTargetGroup.Standalone, ref _newDefineStandalone);
		EditorGUILayout.EndHorizontal();
	}

	private void DrawBuildTargetGroup(BuildTargetGroup buildTargetGroup, ref string newDefine)
	{
		EditorGUILayout.BeginVertical();
		
		EditorGUILayout.LabelField(string.Format("Platform: {0}", buildTargetGroup.ToString()), EditorStyles.boldLabel);
		EditorGUILayout.LabelField("Define symbols:", EditorStyles.boldLabel);
		EditorGUILayout.Space();
		
		EditorGUILayout.BeginHorizontal();
		newDefine = EditorGUILayout.TextField(newDefine);
		EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(newDefine));
		if (GUILayout.Button("Add"))
		{
			DefinesEditor.AddDefine(buildTargetGroup, newDefine);
			newDefine = string.Empty;
		}
		EditorGUI.EndDisabledGroup();
		EditorGUILayout.EndHorizontal();
		
		var defines = DefinesEditor.GetDefines(buildTargetGroup);
		foreach (var define in defines)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(define);
			if (GUILayout.Button("Remove"))
			{
				DefinesEditor.RemoveDefine(buildTargetGroup, define);
			}
			EditorGUILayout.EndHorizontal();
		}
		DrawCommonDefines(buildTargetGroup, defines);
		
		EditorGUILayout.EndVertical();
	}

	private void DrawCommonDefines(BuildTargetGroup buildTargetGroup, List<string> defines)
	{
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Common defines:", EditorStyles.boldLabel);
		
		foreach (var define in _commonDefines)
		{
			if (defines.Contains(define)) continue;
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(define);
			if (GUILayout.Button("Add"))
			{
				DefinesEditor.AddDefine(buildTargetGroup, define);
			}
			EditorGUILayout.EndHorizontal();
		}
	}
}
