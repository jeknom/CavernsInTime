using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Game
{
    public class Player : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] FloatingJoystick joystick;
        [SerializeField] HourglassData hourglassData;
        [SerializeField] SpriteRenderer playerSpriteRenderer;
        [SerializeField] Sprite youngSprite;
        [SerializeField] Sprite oldSprite;

        [Header("Options")]
        [SerializeField] float movementSpeedModifier = 0.1f;

        Quaternion originalRotation;
        Rigidbody2D rigidbody2d;

        void Start()
        {
            this.originalRotation = this.transform.rotation;
            this.rigidbody2d = this.GetComponent<Rigidbody2D>();
            this.hourglassData.OnRotate.AddListener(() => this.OnRotate());
        }

        void OnDestroy()
        {
            this.hourglassData.OnRotate.RemoveListener(() => this.OnRotate());
        }

        void Update()
        {
            if (this.joystick.Direction.magnitude != 0f &&
                !this.hourglassData.isRotating)
            {
                this.rigidbody2d.MovePosition(
                    this.rigidbody2d.position +
                    this.joystick.Direction *
                    this.movementSpeedModifier);
            }
        }

        void OnRotate()
        {
            this.transform
                .DORotate(
                    new Vector3(0f, 0f, -360f),
                    this.hourglassData.rotateDuration,
                    RotateMode.FastBeyond360)
                .OnComplete(() => this.OnRotateComplete());
        }

        void OnRotateComplete()
        {
            this.playerSpriteRenderer.sprite = this.hourglassData.isRotated
                ? this.youngSprite
                : this.oldSprite;

            this.transform.rotation = this.originalRotation;
        }
    }
}
