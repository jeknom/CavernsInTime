using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("General")]
    [SerializeField] Transform target;

    public void OnEnable()
    {
        this.SetTarget(this.target);
    }

    public void SetTarget(Transform newTarget)
    {
        var nextPosition = new Vector3(
            newTarget.position.x,
            newTarget.position.y,
            this.transform.position.z);

        this.transform.position = nextPosition;
        this.transform.parent = newTarget;
    }
}
