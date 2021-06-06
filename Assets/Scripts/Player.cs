using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [SerializeField] private float m_moveSpeed = 5f;
    private CharacterController m_characterController;

    private Vector3 m_moveDirection = Vector3.zero;
    private Vector3 m_velocity = Vector3.zero;

    private bool m_isGrounded;
    [SerializeField] float m_GravityScale = 1f;
    private float m_currentGravityScale;
    [SerializeField] private float m_fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    [SerializeField] private float m_jumpHeight = 6f;
    private bool m_hasDoubledJump;

    private void Start()
    {
        m_characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Debug.Assert(m_characterController != null, nameof(m_characterController) + " != null");

        // Get if the player is grounded
        m_isGrounded = m_characterController.isGrounded;
        //Debug.Log($"{name} {(m_isGrounded ? "GROUNDED" : "NOT GROUNDED")}");

        // Get the horizontal move direction
        m_moveDirection.x = Input.GetAxis("Horizontal");
        // apply the move direction to the velocity
        m_velocity.x = m_moveDirection.x * m_moveSpeed;

        // iF the player is Grounded
        if (m_isGrounded)
        {
            // Reset the Double Jump
            m_hasDoubledJump = false;
            // If the Jump Button is pressed
            if (Input.GetButtonDown("Jump"))
            {
                // Add jump height to the velocity
                m_velocity.y = m_jumpHeight;
                //Debug.Log($"Jump {m_velocity.y}");
            }
        }
        // Else the player is in the air
        else
        {
            // if I have not doubled jump and the Jump butt is pressed
            if (!m_hasDoubledJump && Input.GetButtonDown("Jump"))
            {
                // Set the double jump to true
                m_hasDoubledJump = true;
                // add jump height to the velocity
                m_velocity.y += m_jumpHeight;
                //Debug.Log($"Double Jump {m_velocity.y}");
            }
        }

        ApplyGravity();

        // move the character by the velocity
        // ReSharper disable once PossibleNullReferenceException
        m_characterController.Move(m_velocity * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        #region Better Jumping

        //https://www.reddit.com/r/Unity3D/comments/60hchp/better_jumping_with_four_lines_of_code_youtube/

        // If the player is on the downward portion of its jump or is falling
        if (m_velocity.y < 0)
        {
            //faster falling
            m_currentGravityScale = m_GravityScale * m_fallMultiplier;
        }
        //control jump height by length of time jump button held
        // apply more gravity if the player is not holding the jump button
        // and is moving upward.
        else if (m_velocity.y > 0 && !Input.GetButton("Jump"))
        {
            m_currentGravityScale = m_GravityScale * lowJumpMultiplier;
        }
        // else use regular gravity
        else
        {
            m_currentGravityScale = m_GravityScale;
        }

        #endregion

        // Apply gravity to the velocity
        m_velocity += Physics.gravity * m_currentGravityScale * Time.deltaTime;
    }
}
