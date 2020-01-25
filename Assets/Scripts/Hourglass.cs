using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Utils;
using DG.Tweening;

public class Hourglass : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] HourglassData hourglassData;
    [SerializeField] Text timerText;
    [SerializeField] Image hourglassImage;
    [SerializeField] List<Sprite> sandTrickleImages;

    Quaternion originalRotation;
    WaitForSeconds reusableWaitForSeconds = new WaitForSeconds(1f);
    Option<IEnumerator> currentTrickleRoutine = new Option<IEnumerator>();
    public UnityEvent OnRotate;
    public float currentTrickle;
    public bool isYoung = false;
    public bool isTrickling = false;
    public bool isRotating = false;

    public void StartTrickle()
    {
        if (!this.isTrickling)
        {
            this.currentTrickle = this.hourglassData.trickleTime;

            this.currentTrickleRoutine =
                new Option<IEnumerator>(TrickleRoutine());

            this.currentTrickleRoutine.MatchSome(routine =>
            {
                this.StartCoroutine(routine);
                this.isTrickling = true;
            });
        }
    }

    public void StopTrickle()
    {
        this.transform.DOKill();

        if (this.isTrickling)
        {
            this.currentTrickleRoutine.MatchSome(routine =>
            {
                this.StopCoroutine(routine);
            });

            this.ResetTrickle();
            this.isTrickling = false;
        }
    }

    public void PauseTrickle()
    {
        if (this.isTrickling)
        {
            this.currentTrickleRoutine.MatchSome(routine =>
            {
                this.StopCoroutine(routine);
            });

            this.isTrickling = false;
        }
    }

    public void ResetTrickle()
    {
        this.isTrickling = false;
        this.isYoung = false;
        this.isRotating = false;
        this.currentTrickle = this.hourglassData.trickleTime;
    }

    public void Rotate()
    {
        if (!this.isRotating)
        {
            this.isRotating = true;
            var endValue = new Vector3(0f, 0f, 180f);
            var tween = this.transform
                .DORotate(
                    endValue,
                    this.hourglassData.rotateDuration,
                    RotateMode.FastBeyond360)
                .OnStart(() => this.OnRotateStart())
                .SetRelative()
                .OnComplete(() => this.OnRotateComplete());

            DOTween.Play(tween);
        }
    }

    void OnRotateStart()
    {
        this.isRotating = true;
        this.OnRotate.Invoke();

        this.PauseTrickle();
    }

    void OnRotateComplete()
    {
        this.currentTrickle =
            this.hourglassData.trickleTime - this.currentTrickle;
        this.isRotating = false;
        this.isYoung = !this.isYoung;
        this.transform.rotation = this.originalRotation;

        if (!this.isTrickling)
        {
            this.StartTrickle();
        }
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
            if (!this.isTrickling)
            {
                this.ResetTrickle();
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
                this.currentTrickle /
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
            if (this.isYoung)
            {
                this.currentTrickle -= 2;
            }
            else
            {
                this.currentTrickle--;
            }

            this.SetCurrentHourglassImage();
            this.timerText.text = this.currentTrickle.ToString();

            yield return this.reusableWaitForSeconds;
        }
        while (
            this.currentTrickle > 0 &&
            this.currentTrickle < this.hourglassData.trickleTime);

        this.StopTrickle();
    }
}
