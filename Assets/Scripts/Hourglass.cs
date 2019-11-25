using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Hourglass : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] LevelData data;
    [SerializeField] HourglassData hourglassData;
    [SerializeField] Text timerText;
    [SerializeField] Image hourglassImage;
    [SerializeField] List<Sprite> sandTrickleImages;

    Quaternion originalRotation;
    WaitForSeconds reusableWaitForSeconds = new WaitForSeconds(1f);
    IEnumerator currentTrickleRoutine;

    public void StartTrickle()
    {
        if (!this.hourglassData.isTrickling)
        {
            this.currentTrickleRoutine = TrickleRoutine();
            this.StartCoroutine(this.currentTrickleRoutine);
            this.hourglassData.isTrickling = true;
        }
    }

    public void StopTrickle()
    {
        this.transform.DOKill();

        if (this.hourglassData.isTrickling)
        {
            this.StopCoroutine(this.currentTrickleRoutine);
            this.ResetTrickle();
            this.hourglassData.isTrickling = false;
        }
    }

    public void PauseTrickle()
    {
        if (this.hourglassData.isTrickling)
        {
            this.StopCoroutine(this.currentTrickleRoutine);
            this.hourglassData.isTrickling = false;
        }
    }

    public void ResetTrickle()
    {
        this.hourglassData.currentTrickle = this.hourglassData.trickleTime;
    }

    public void Rotate()
    {
        if (!this.hourglassData.isRotating)
        {
            this.hourglassData.isRotating = true;
            var endValue = new Vector3(0f, 0f, 180f);
            this.transform
                .DORotate(
                    endValue,
                    this.hourglassData.rotateDuration,
                    RotateMode.FastBeyond360)
                .OnStart(() => this.OnRotateStart())
                .SetRelative()
                .OnComplete(() => this.OnRotateComplete());

            this.transform.DOPlay();
        }
    }

    void OnRotateStart()
    {
        this.hourglassData.isRotating = true;
        this.hourglassData.OnRotate.Invoke();

        this.PauseTrickle();
    }

    void OnRotateComplete()
    {
        this.hourglassData.currentTrickle =
            this.hourglassData.trickleTime - this.hourglassData.currentTrickle;
        this.hourglassData.isRotating = false;
        this.hourglassData.isRotated = !this.hourglassData.isRotated;
        this.transform.rotation = this.originalRotation;

        this.StartTrickle();
    }

    void Start()
    {
        this.originalRotation = this.transform.rotation;    
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            this.Rotate();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (!this.hourglassData.isTrickling)
            {
                Debug.Log("Starting");
                this.StartTrickle();
            }
            else
            {
                Debug.Log("Stopping");
                this.StopTrickle();
            }
        }
    }

    void SetCurrentHourglassImage()
    {
        // Calculating the image index from the percentage of time passed.
        var percentile = Math.Round(
                this.hourglassData.currentTrickle /
                this.hourglassData.trickleTime, 1);
        var index = Math.Floor(percentile * 10);
        if (index <= 0)
        {
            this.hourglassImage.sprite = this.sandTrickleImages[0];
        }
        else if (index >= 10)
        {
            this.hourglassImage.sprite = this.sandTrickleImages[9];
        }
        else
        {
            this.hourglassImage.sprite = this.sandTrickleImages[(int)index];
        }
    }

    IEnumerator TrickleRoutine()
    {
        do
        {
            this.hourglassData.currentTrickle--;
            this.SetCurrentHourglassImage();
            this.timerText.text = this.hourglassData.currentTrickle.ToString();

            yield return this.reusableWaitForSeconds;
        }
        while (
            this.hourglassData.currentTrickle > 0 &&
            this.hourglassData.currentTrickle < this.hourglassData.trickleTime);

        this.StopTrickle();
    }
}
