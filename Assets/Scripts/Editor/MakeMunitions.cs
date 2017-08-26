using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
    public class MakeMunitions
    {
        [MenuItem("Assets/Create/Munition Object")]
        public static void CreatMunitions()
        {
            Munition asset = ScriptableObject.CreateInstance<Munition>();

            AssetDatabase.CreateAsset(asset, "Assets/NewScripableObject.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;

        }    
    }
}