using UnityEngine;
using DG.Tweening;

namespace Game
{
    public class Player : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] FloatingJoystick joystick;
        [SerializeField] Hourglass hourglass;
        [SerializeField] SpriteRenderer playerSpriteRenderer;
        [SerializeField] Sprite youngSprite;
        [SerializeField] Sprite oldSprite;
        [SerializeField] float rotateDuration;

        [Header("Options")]
        [SerializeField] float oldSpeed = 0.1f;
        [SerializeField] float youngSpeed = 0.3f;

        Quaternion originalRotation;
        Rigidbody2D rigidbody2d;
        float movementSpeedModifier = 0.1f;

        void Start()
        {
            this.originalRotation = this.transform.rotation;
            this.rigidbody2d = this.GetComponent<Rigidbody2D>();
            this.movementSpeedModifier = this.oldSpeed;
            this.hourglass.OnRotate.AddListener(() => this.OnRotate());
        }

        void OnDestroy()
        {
            this.hourglass.OnRotate.RemoveListener(() => this.OnRotate());
        }

        void Update()
        {
            if (this.joystick.Direction.magnitude != 0f &&
                !this.hourglass.isRotating)
            {
                this.rigidbody2d.MovePosition(
                    this.rigidbody2d.position +
                    this.joystick.Direction *
                    this.movementSpeedModifier);
            }
        }

        void OnRotate()
        {
            var tween = this.transform
                .DORotate(
                    new Vector3(0f, 0f, -360f),
                    this.rotateDuration,
                    RotateMode.FastBeyond360)
                .OnComplete(() => this.OnRotateComplete());

            DOTween.Play(tween);
        }

        void OnRotateComplete()
        {
            if (this.hourglass.isYoung)
            {
                this.playerSpriteRenderer.sprite = this.youngSprite;
                this.movementSpeedModifier = this.youngSpeed;
            }
            else
            {
                this.playerSpriteRenderer.sprite = this.oldSprite;
                this.movementSpeedModifier = this.oldSpeed;
            }

            this.transform.rotation = this.originalRotation;
        }
    }
}
