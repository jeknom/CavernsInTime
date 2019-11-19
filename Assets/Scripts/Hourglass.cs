using UnityEngine;
using DG.Tweening;

public class Hourglass : MonoBehaviour
{
    [SerializeField] float rotateDuration = 1f;
    bool isRotating = false;

    void Turn()
    {
        if (!this.isRotating)
        {
            this.isRotating = true;
            var endValue = new Vector3(0f, 0f, 180f);
            var tween = DOTween.Play(this.transform);
            this.transform
                .DORotate(
                    endValue,
                    rotateDuration,
                    RotateMode.FastBeyond360)
                .OnStart(() => this.isRotating = true)
                .SetRelative()
                .OnComplete(() => this.isRotating = false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            this.Turn();
        }
    }
}
