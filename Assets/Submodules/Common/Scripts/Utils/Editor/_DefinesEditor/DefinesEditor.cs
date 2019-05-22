using System.Collections.Generic;
using System.Linq;
using UnityEditor;

public static class DefinesEditor
{
    public static List<string> GetDefines(BuildTargetGroup buildTargetGroup)
    {
        return PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup).Split(';').ToList();
    }
    
    public static void SetDefines(BuildTargetGroup buildTargetGroup, List<string> defines)
    {
        var str = "";
        if (!defines.IsNullOrEmpty())
        {
            defines.ForEach(a => str += string.Format("{0};", a));
        }
        
        PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, str);
    }
    
    public static void AddDefine(BuildTargetGroup buildTargetGroup, string define)
    {
        var defines = GetDefines(buildTargetGroup);
        if (!defines.Contains(define))
        {
            defines.Add(define);    
        }
        SetDefines(buildTargetGroup, defines);
    }
    
    public static void RemoveDefine(BuildTargetGroup buildTargetGroup, string define)
    {
        var defines = GetDefines(buildTargetGroup);
        if (defines.Contains(define))
        {
            defines.Remove(define);    
        }
        SetDefines(buildTargetGroup, defines);
    }

    public static bool ContainsDefine(BuildTargetGroup buildTargetGroup, string define)
    {
        var defines = GetDefines(buildTargetGroup);
        return defines.Contains(define);
    }
}
