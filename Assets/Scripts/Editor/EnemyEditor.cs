using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(Enemy))]
public class EnemyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Enemy enemy = target as Enemy;
        if (enemy.UseInspectorParams)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Save specs"))
            {
                enemy.SaveParamsToJson();
                AssetDatabase.Refresh();
            }
            if (GUILayout.Button("Load specs"))
            {
                enemy.LoadParamsFromJson();
            }
            GUILayout.EndHorizontal();
        }
    }
}
#endif