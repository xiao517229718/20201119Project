using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
public enum tets
{
    oine = 1,
    two = 2,

}
public class CreatAssetBundle
{
    public static void RemoveAllAssetBundleName()
    {
        string[] allBundleName = AssetDatabase.GetAllAssetBundleNames();
        for (int i = 0; i < allBundleName.Length; i++)
        {
            AssetDatabase.RemoveAssetBundleName(allBundleName[i], true);
        }
    }
    [MenuItem("AssetBundleTools/BuildWindowsAssetBundles")]
    public static void buildWindowsAssetBundle()
    {
        GUIUtility.systemCopyBuffer = "";
        DeleteAllAssetBundles(RuntimePlatform.WindowsPlayer);
        string outPath = string.Empty;
        outPath = PathTools.getABOutPath(RuntimePlatform.WindowsPlayer);
        if (!Directory.Exists(outPath))
        {
            Directory.CreateDirectory(outPath);
        }
        Debug.LogError(outPath);
        // AssetBundle assetBundle = BuildPipeline.BuildAssetBundle()
        AssetBundleBuild[] assetBundleBuilds = new AssetBundleBuild[2];
        // assetBundleBuilds[1].addressableNames=""
        List<AssetBundleBuild> buildMaps = new List<AssetBundleBuild>();
        AssetBundleBuild assetBundleBuild = new AssetBundleBuild();
        assetBundleBuild.assetBundleName = "Test.unity3d";
        assetBundleBuild.assetNames = new string[] { "Assets/AB_Resources/perfabs/Image.prefab", "Assets/AB_Resources/perfabs/ceshi.prefab" };
        buildMaps.Add(assetBundleBuild);
        BuildPipeline.BuildAssetBundles(outPath, buildMaps.ToArray(), BuildAssetBundleOptions.None, BuildTarget.Android);
        //BuildPipeline.BuildAssetBundles()
        AssetDatabase.Refresh();
    }
    [MenuItem("GameObject/TestAB", false, -1)]
    public static void InstateObj()
    {
        AssetBundle assetBundle = AssetBundle.LoadFromFile(Application.dataPath + "/StreamingAssets/StreamingAssets");
        AssetBundleManifest assetBundleManifest = assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        Debug.LogError(assetBundle);
        Debug.LogWarning(assetBundleManifest);

        string[] bundles = assetBundleManifest.GetAllAssetBundles();
        for (int i = 0; i < bundles.Length; i++)
        {
            Debug.Log(bundles[i]);
            //  AssetBundle elem = elmebundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        }

    }

    private static void DeleteAllAssetBundles(RuntimePlatform platform)
    {
        string outPath = string.Empty;
        outPath = PathTools.getABOutPath(platform);
        Debug.Log("删除资源:" + outPath);
        if (!string.IsNullOrEmpty(outPath))
        {
            if (Directory.Exists(outPath))
            {
                Directory.Delete(outPath, true);
                File.Delete(outPath + ".meta");
            }
        }
        AssetDatabase.Refresh();
    }

    public class BuildAssetBundlesBuildMapExample : MonoBehaviour
    {
        [MenuItem("Example/Build Asset Bundles Using BuildMap")]
        static void BuildMapABs()
        {
            // Create the array of bundle build details.
            AssetBundleBuild[] buildMap = new AssetBundleBuild[2];

            buildMap[0].assetBundleName = "enemybundle";

            string[] enemyAssets = new string[2];
            enemyAssets[0] = "Assets/Textures/char_enemy_alienShip.jpg";
            enemyAssets[1] = "Assets/Textures/char_enemy_alienShip-damaged.jpg";

            buildMap[0].assetNames = enemyAssets;


            buildMap[1].assetBundleName = "herobundle";

            string[] heroAssets = new string[1];
            heroAssets[0] = "char_hero_beanMan";
            buildMap[1].assetNames = heroAssets;

            BuildPipeline.BuildAssetBundles("Assets/ABs", buildMap, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
        }
    }

    /// <summary>
    /// 自动设置AssetBundle包名
    /// </summary>
    [MenuItem("AssetBundleTools/Set AssetBundle Label")]
    public static void setAssetBundleLabel()
    {
        //打包资源根目录
        string abRootPath = string.Empty;
        //根目录下的目录信息
        DirectoryInfo[] abDirector;
        RemoveAllAssetBundleName();
        //获取打包资源根目录
        abRootPath = PathTools.getABResourcesPath();
        Debug.LogWarning(abRootPath + "-----");
        DirectoryInfo abRootDirector = new DirectoryInfo(abRootPath);
        abDirector = abRootDirector.GetDirectories();
        Debug.LogWarning(abDirector.Length);
        //遍历每一个场景目录
        foreach (DirectoryInfo scenesDir in abDirector)
        {
            string assetName = scenesDir.Name;
            findFile(scenesDir, assetName);
        }
        AssetDatabase.Refresh();
        Debug.LogWarning("设置成功");
    }
    private static void findFile(DirectoryInfo scenesDir, string assetName)
    {
        //所有文件信息
        FileInfo[] fileArr = scenesDir.GetFiles();
        foreach (FileInfo fileInfo in fileArr)
        {
            setFileABLabel(fileInfo, assetName);
        }
        //所有文件夹信息
        DirectoryInfo[] dirArr = scenesDir.GetDirectories();
        foreach (DirectoryInfo dir in dirArr)
        {
            findFile(dir, assetName);
        }
    }
    private static void setFileABLabel(FileInfo fileInfo, string assetName)
    {
        //忽视unity自身生成的meta文件
        if (fileInfo.Extension == ".meta")
            return;
        int index = fileInfo.FullName.IndexOf("Assets");
        //截取Assets之后的路径
        //AssetImporter.GetAtPath必须是unity工程的相对路径
        //所以要Assets开头
        string filePath = fileInfo.FullName.Substring(index);
        //通过AssetImporter指定要标记的文件
        AssetImporter importer = AssetImporter.GetAtPath(filePath);
        //区分场景文件和资源文件后缀名
        if (fileInfo.Extension == ".unity")
            importer.assetBundleVariant = "u3d";
        else
            importer.assetBundleVariant = "ab";
        //包名称
        string bundleName = string.Empty;
        //需要拿到场景目录下面一级目录名称
        //包名=场景目录名+下一级目录名
        int indexScenes = fileInfo.FullName.IndexOf(assetName) + assetName.Length + 1;
        string bundlePath = fileInfo.FullName.Substring(indexScenes);
        //替换win路径里的反斜杠
        bundlePath = bundlePath.Replace(@"\", "/");
        Debug.Log(bundlePath);
        if (bundlePath.Contains("/"))
        {
            string[] strArr = bundlePath.Split('/');
            bundleName = assetName + "/" + strArr[0];
        }
        else
        {
            bundleName = assetName + "/" + assetName;
        }
        importer.assetBundleName = bundleName;
    }
}
public class PathTools
{
    //AssetBundle资源路径
    public const string AB_Resources = "/AB_Resources";

    public static string getABResourcesPath()
    {
        return Application.dataPath + AB_Resources;
    }

    /// <summary>
    /// AssetBundle输出路径
    /// </summary>
    /// <returns></returns>
    public static string getABOutPath(RuntimePlatform platform)
    {
        return getPlatformPath(platform);
    }

    private static string getPlatformPath(RuntimePlatform platform)
    {
        string platformPath = string.Empty;
        switch (platform)
        {
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                platformPath = Application.streamingAssetsPath;
                break;
            case RuntimePlatform.IPhonePlayer:
            case RuntimePlatform.Android:
                platformPath = Application.persistentDataPath;
                break;
            default:
                break;
        }
        return platformPath;
    }
}