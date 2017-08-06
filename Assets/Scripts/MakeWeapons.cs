using Assets.Scripts;
using UnityEditor;
using UnityEngine;

public class MakeWeapons : MonoBehaviour {

    [MenuItem("Assets/Create/Weapon Object")]
    public static void CreatMunitions()
    {
        WeaponData asset = ScriptableObject.CreateInstance<WeaponData>();

        AssetDatabase.CreateAsset(asset, "Assets/NewScripableObject.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;

    }

}
