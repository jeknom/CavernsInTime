using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Level Data", menuName = "Scriptable Objects/Level Data")]
public class LevelData : ScriptableObject
{
    public float levelRotates;
    public float levelTime;
    public float currentRotates = 0;
    public float currentTime = 0;

    public void Reset()
    {
        this.currentRotates = this.levelRotates;
        this.currentTime = this.levelTime;
    }
}

[CustomEditor(typeof(LevelData))]
public class LevelDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var levelData = target as LevelData;

        EditorGUILayout.LabelField("Max values");
        EditorGUILayout.Separator();
        levelData.levelRotates = EditorGUILayout.FloatField("Rotates", levelData.currentRotates);
        levelData.levelTime = EditorGUILayout.FloatField("Time", levelData.levelTime);
        UnityEditor.EditorGUILayout.HelpBox("These are the total amounts for the level.", MessageType.Info);

        EditorGUILayout.LabelField("Current values");
        EditorGUILayout.Separator();
        EditorGUI.BeginDisabledGroup(true);
        levelData.currentRotates = EditorGUILayout.FloatField("Current Rotates", levelData.currentRotates);
        levelData.currentTime = EditorGUILayout.FloatField("Current Time", levelData.currentTime);
        UnityEditor.EditorGUILayout.HelpBox("These values will be modified in code.", MessageType.Info);
        EditorGUI.EndDisabledGroup();
    }
}
