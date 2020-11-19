using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ABManager
{
    private static ABManager instance;
    public static ABManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ABManager();
            }
            return instance;
        }
    }
    private AssetBundleManifest manifest = null;
    string abPath;
    private ABManager()
    {
        abPath = Application.dataPath.Substring(0, Application.dataPath.Length - 6) + "AB";
        AssetBundle main = AssetBundle.LoadFromFile(abPath + "/AB");
        manifest = main.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
    }
    Dictionary<string, AssetBundle> abDic = new Dictionary<string, AssetBundle>();
    /// <summary>
    /// 加载AB资源的
    /// </summary>
    /// <param name="abName"></param>
    public void LoadABFile(string abName)
    {
        if (abDic.ContainsKey(abName))
        {
            return;
        }
        string[] des = manifest.GetAllDependencies(abName);
        for (int i = 0; i < des.Length; i++)
        {
            if (!abDic.ContainsKey(des[i]))
            {
                abDic.Add(des[i], AssetBundle.LoadFromFile(abPath + "/" + des[i]));
            }
        }
        abDic.Add(abName, AssetBundle.LoadFromFile(abPath + "/" + abName));
    }
    /// <summary>
    /// 从AB包中去Unity资源
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public Object LoadAsset(string abName, string name)
    {
        LoadABFile(abName);
        if (abDic.ContainsKey(abName))
        {
            return abDic[abName].LoadAsset(name);
        }
        else
        {
            Debug.LogErrorFormat("ab 资源出错，ab:{0},name:{1}", abName, name);
            return null;
        }
    }
}
