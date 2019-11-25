using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] Transform target;

    [Header("Options")]
    [SerializeField] float cameraZoom = -10;
    [SerializeField] bool useCameraFollow;
    [SerializeField] float followDelay = 0.5f;

    public void OnEnable()
    {
        if (!useCameraFollow)
        {
            this.SetTarget(this.target);
        }
    }

    public void Update()
    {
       if (useCameraFollow)
       {
            var nextPosition = new Vector3(
                this.target.position.x,
                this.target.position.y,
                this.cameraZoom);

            this.transform.position = Vector3.MoveTowards(
                this.transform.position,
                nextPosition,
                this.followDelay);
       }
    }

    public void SetTarget(Transform newTarget)
    {
        var nextPosition = new Vector3(
            newTarget.position.x,
            newTarget.position.y,
            this.cameraZoom);

        this.transform.position = nextPosition;
        this.transform.parent = newTarget;
    }
}
