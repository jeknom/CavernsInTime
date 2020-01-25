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
    [SerializeField] Text timerText;
    [SerializeField] Image hourglassImage;
    [SerializeField] List<Sprite> sandTrickleImages;
    [SerializeField] Button hourglassButton;

    [HideInInspector] public bool isYoung = false;
    [HideInInspector] public bool isRotating = false;
    [HideInInspector] public float trickleTime = 30f;
    [HideInInspector] public UnityEvent OnRotate;

    Quaternion originalRotation;
    WaitForSeconds reusableWaitForSeconds = new WaitForSeconds(1f);
    Option<IEnumerator> currentTrickleRoutine = new Option<IEnumerator>();
    float currentTrickleTime;
    readonly float rotateDuration = 1f;
    bool isTrickling = false;

    public void Init()
    {
        this.originalRotation = this.transform.rotation;
        this.currentTrickleTime = this.trickleTime;
        this.hourglassButton.onClick.AddListener(() => this.Rotate());
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

    void OnMouseDown()
    {
        Debug.Log("Hello!");
        this.Rotate();
    }

    public void StartTrickle()
    {
        if (!this.isTrickling)
        {
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
        this.currentTrickleTime = this.trickleTime;
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
                    this.rotateDuration,
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
        this.currentTrickleTime = this.trickleTime - this.currentTrickleTime;
        this.isRotating = false;
        this.isYoung = !this.isYoung;
        this.transform.rotation = this.originalRotation;

        if (!this.isTrickling)
        {
            this.StartTrickle();
        }
    }

    void SetCurrentHourglassImage()
    {
        // Calculating the image index from the percentage of time passed.
        var percentile = Math.Round(this.currentTrickleTime / this.trickleTime, 1);
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
                this.currentTrickleTime -= 2;
            }
            else
            {
                this.currentTrickleTime--;
            }

            this.SetCurrentHourglassImage();
            this.timerText.text = this.currentTrickleTime.ToString();

            yield return this.reusableWaitForSeconds;
        }
        while (
            this.currentTrickleTime > 0 &&
            this.currentTrickleTime < this.trickleTime);

        this.StopTrickle();
    }
}