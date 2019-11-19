using UnityEngine;

namespace Game
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] FloatingJoystick joystick;

        [Header("Movement variables")]
        [SerializeField] float movementSpeedModifier = 0.1f;

        Rigidbody2D rigidbody2d;

        void Start()
        {
            this.rigidbody2d = this.GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            if (this.joystick.Direction.magnitude != 0f)
            {
                this.rigidbody2d.MovePosition(
                    this.rigidbody2d.position +
                    this.joystick.Direction *
                    this.movementSpeedModifier);
            }
        }
    }
}
