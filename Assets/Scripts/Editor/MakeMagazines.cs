
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
    public class MakeMagazines : MonoBehaviour {
        [MenuItem("Assets/Create/Magazine Object")]
        public static void CreatMagazines()
        {
            MagazineData asset = ScriptableObject.CreateInstance<MagazineData>();

            AssetDatabase.CreateAsset(asset, "Assets/NewScripableObject.asset");
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;

        }
    }

}


