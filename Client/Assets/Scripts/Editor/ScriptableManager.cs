using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;


public class ScriptableManager : MonoBehaviour
{
	[MenuItem("RememberPos/Configs/PersonConf")]
	static void CreatePerson()
	{
		CreateAsset<PersonConf>();
	}

	[MenuItem("RememberPos/Configs/PersonLib")]
	static void CreatePersonLib()
	{
		CreateAsset<PersonLibrary>();
	}

	[MenuItem("RememberPos/Configs/ViewConf")]
	static void CreateView()
	{
		CreateAsset<ViewConf>();
	}

	[MenuItem("RememberPos/Configs/ViewLib")]
	static void CreateViewLib()
	{
		CreateAsset<ViewLibrary>();
	}

	[MenuItem("RememberPos/Configs/Block")]
	static void CreateBlock()
	{
		CreateAsset<BlockConf>();
	}

	[MenuItem("RememberPos/Configs/BlockLib")]
	static void CreateBlockLib()
	{
		CreateAsset<BlockLibrary>();
	}

	[MenuItem("RememberPos/Configs/BlockBoard")]
	static void CreateBlockBoard()
	{
		CreateAsset<BlockBoardConf>();
	}

	[MenuItem("RememberPos/Configs/BlockBoardLib")]
	static void CreateBlockBoardLib()
	{
		CreateAsset<BlockBoardLibrary>();
	}

	[MenuItem("RememberPos/Configs/Board")]
	static void CreateBoard()
	{
		CreateAsset<BoardConf>();
	}

	[MenuItem("RememberPos/Configs/BoardLib")]
	static void CreateBoardLib()
	{
		CreateAsset<BoardLibrary>();
	}

	/// <param name="isSpecify">是否的指定新路径 <c>true</c> 是 </param>
	public static void CreateAsset<T>(string newDir = "Assets/Configs", bool isSpecify = false) where T : ScriptableObject
	{
		if(isSpecify)
		{
			Create<T>(newDir); 
			return; 
		}
		Create<T>(GetPath()); 
	}

	static void Create<T>(string dir) where T : ScriptableObject
	{
		if (!Directory.Exists(dir))
		{
			Directory.CreateDirectory(dir); 
		}
		string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(dir + "/New" + typeof(T).ToString() + ".asset");
		T asset = ScriptableObject.CreateInstance<T>(); 
		AssetDatabase.CreateAsset(asset, assetPathAndName);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
		EditorUtility.FocusProjectWindow();
		Selection.activeObject = asset;
	}

	static string GetPath()
	{
		string dir = AssetDatabase.GetAssetPath(Selection.activeObject);
		if (dir == "")
		{
			dir = "Assets/Configs";
		}
		else if (string.Compare(Path.GetExtension(dir), "") != 0)
		{
			// if the extension exists, then go to the parent directory
			dir = Path.GetDirectoryName(dir);
		}
		return dir; 
	}
}
