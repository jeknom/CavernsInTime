using UnityEngine;
using DG.Tweening;

public class Hourglass : MonoBehaviour
{
    [SerializeField] LevelData data;
    [SerializeField] float rotateDuration;
    bool isRotating = false;

    public void Rotate()
    {
        if (!this.isRotating)
        {
            this.isRotating = true;
            var endValue = new Vector3(0f, 0f, 180f);
            this.transform
                .DORotate(
                    endValue,
                    this.rotateDuration,
                    RotateMode.FastBeyond360)
                .OnStart(() => this.OnRotateStart())
                .SetRelative()
                .OnComplete(() => this.OnRotateComplete());
        }
    }

    void OnRotateStart()
    {
        this.isRotating = true;
    }

    void OnRotateComplete()
    {
        this.isRotating = false;
    }

    public void StartTimer()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            this.Rotate();
        }
    }
}
