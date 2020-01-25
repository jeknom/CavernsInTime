using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] LevelData data;
    [SerializeField] Hourglass hourglass;

    void Start()
    {
        this.hourglass.trickleTime = this.data.levelTime;
        this.hourglass.StartTrickle();
    }
}
