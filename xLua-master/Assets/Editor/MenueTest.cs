using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MenueTest
{
    [MenuItem("GameObject/GetDatabaseAsssets", false, -1)]
    public static void GetDatabaseAsssets()
    {
        string[] assets = AssetDatabase.FindAssets("b:TextAsset", new string[] { "Assets/XLua/LuaScripts" });
        if (assets.Length > 0)
        {
            for (int i = 0; i < assets.Length; i++)
            {
                Debug.LogError(assets[i]);
            }

        }
        else
        {
            Debug.LogWarning("查找失败");
        }

    }
}
