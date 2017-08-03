using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
    public class MakeMunitions
    {
        [MenuItem("Assets/Create/Munition Object")]
        public static void CreatMunitions()
        {
            Munitions asset = ScriptableObject.CreateInstance<Munitions>();

            AssetDatabase.CreateAsset(asset, "Assets/NewScripableObject.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;

        }    
    }
}