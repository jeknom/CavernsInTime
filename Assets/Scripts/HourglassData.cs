using UnityEngine;

[CreateAssetMenu(
    fileName = "Hourglass Data",
    menuName = "Scriptable Objects/Hourglass Data")]
public class HourglassData : ScriptableObject
{
    public float trickleTime = 10f;
    public float rotateDuration = 1;
}