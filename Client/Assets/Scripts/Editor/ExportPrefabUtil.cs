using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class ExportPrefabUtil
{
	[MenuItem("RememberPos/ExportPrefabs %#e")]
	public static void Export()
	{
		ExportPrefab[] ep = GameObject.FindObjectsOfType<ExportPrefab>();
		foreach (var e in ep)
		{
			GameObject obj = (GameObject)GameObject.Instantiate(e.gameObject);
			Debug.Log(string.Format("<color=purple>Export Prefab {0}</color>", obj.name));
			obj.transform.parent = e.gameObject.transform.parent;
			GameObject.DestroyImmediate(obj.GetComponent<ExportPrefab>(), true);

			string finalpath = Path.Combine("Assets", e._path);
			finalpath = Path.Combine(finalpath, e.gameObject.name + ".prefab").Replace("\\", "/");

			GameObject prefab = CreateNew(obj, finalpath);
			EditorUtility.SetDirty(prefab);
			GameObject.DestroyImmediate(obj, true);
		}
		AssetDatabase.SaveAssets();
	}

	public static GameObject CreateNew(GameObject obj, string localPath)
	{
		string path = Path.GetDirectoryName(localPath);
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
		Object prefab = AssetDatabase.LoadAssetAtPath(localPath, typeof(GameObject));
		if (prefab == null)
		{
			prefab = PrefabUtility.CreateEmptyPrefab(localPath);
		}
		GameObject res = PrefabUtility.ReplacePrefab(obj, prefab, ReplacePrefabOptions.ReplaceNameBased);
		return res;
		//GameObject res = PrefabUtility.CreatePrefab(localPath, obj, ReplacePrefabOptions.Default);

		//return res;
	}

	public static string GetGameObjectPath(GameObject obj)
	{
		string path = "/" + obj.name;
		while (obj.transform.parent != null)
		{
			obj = obj.transform.parent.gameObject;
			path = "/" + obj.name + path;
		}
		return path;
	}
}

