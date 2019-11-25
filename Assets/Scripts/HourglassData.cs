using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(
    fileName = "Hourglass Data",
    menuName = "Scriptable Objects/Hourglass Data")]
public class HourglassData : ScriptableObject
{
    public bool isTrickling = false;
    public bool isRotating = false;
    public bool isRotated = false;
    public float currentTrickle;
    public float trickleTime = 10f;
    public float rotateDuration = 1;
    public UnityEvent OnRotate;
}