using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    #region Fields

    // ReSharper disable MissingLinebreak

    private CharacterController m_characterController;

    [Header("Movement")]
    [SerializeField] private float m_moveSpeed = 5f;

    private Vector3 m_moveDirection = Vector3.zero;
    private Vector3 m_velocity = Vector3.zero;

    [Header("Gravity")]
    // ReSharper disable once RedundantLinebreak
    private bool m_isGrounded;

    [SerializeField] private float m_GravityScale = 1f;
    private float m_currentGravityScale;

    [Header("Jumping")]
    [SerializeField] private float m_jumpHeight = 6f;

    [SerializeField] private float m_fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;
    private bool m_jump;
    private bool m_hasDoubledJump;

    [Header("Wall")]
    [SerializeField, Tag] private string m_wallTag = "Wall";

    [SerializeField] private float m_wallFallMultiplier = 0.5f;
    private bool m_isContactingWall;
    private bool m_isWallJumping;

    [Header("Pushing")]
    [Range(0.1f, 10)]
    [SerializeField] private float m_pushPower = 2;

    // ReSharper restore MissingLinebreak

    #endregion

    private void Start()
    {
        m_characterController = GetComponent<CharacterController>();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        OnControllerColliderPushRigidbody(hit);
        OnControllerColliderHitWall(hit);
    }

    private void OnControllerColliderHitWall(ControllerColliderHit hit)
    {
        // ReSharper disable PossibleNullReferenceException
        if (!hit.collider.CompareTag(m_wallTag) || m_isGrounded ||
            m_characterController.collisionFlags != CollisionFlags.Sides) return;

        m_isContactingWall = true;
        Debug.DrawRay(hit.point, hit.normal, Color.blue);

        // ReSharper restore PossibleNullReferenceException

        if (!m_jump) return;

        m_isWallJumping = true;
        m_velocity.y = m_jumpHeight;
        m_moveDirection.x = hit.normal.x;
        m_hasDoubledJump = false;
    }

    private void OnControllerColliderPushRigidbody(ControllerColliderHit hit)
    {
        Rigidbody hitRigidbody = hit.rigidbody;
        // confirm it has a rigidbody and that the rigidbody can be pushed (the body is not kinematic)
        if (hitRigidbody == null || hitRigidbody.isKinematic) return;

        // make sure that the box is not below the player
        if (hit.moveDirection.y < -0.3f) return;

        //calculate move direction
        Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0, 0);
        //push (using rigid body velocity
        float pushPower = m_pushPower / (hitRigidbody.mass > 0 ? hitRigidbody.mass : 0.1f);
        hitRigidbody.velocity = pushDirection * pushPower;
    }

    private void Update()
    {
        if (!m_isWallJumping)
        {
            m_moveDirection.x = Input.GetAxis("Horizontal");
        }

        m_velocity.x = m_moveDirection.x * m_moveSpeed;

        // ReSharper disable once PossibleNullReferenceException
        if (m_characterController.collisionFlags == CollisionFlags.None)
            m_isContactingWall = false;

        // make the player face the correct direction
        if (m_moveDirection != Vector3.zero)
        {
            transform.forward = m_moveDirection;
        }

        m_jump = Input.GetButtonDown("Jump");

        // ReSharper disable once PossibleNullReferenceException
        m_isGrounded = m_characterController.isGrounded;
        //Debug.Log($"{name} {(m_isGrounded ? "GROUNDED" : "NOT GROUNDED")}");

        if (m_isGrounded)
        {
            OnGrounded();
        }
        else
        {
            InAir();
            ApplyGravity();
        }

        // ReSharper disable once PossibleNullReferenceException
        m_characterController.Move(m_velocity * Time.deltaTime);
    }

    private void OnGrounded()
    {
        m_hasDoubledJump = false;
        m_isContactingWall = false;
        m_isWallJumping = false;

        if (m_jump)
        {
            m_velocity.y = m_jumpHeight;
        }
    }

    private void InAir()
    {
        if (m_hasDoubledJump || !m_jump) return;

        m_isWallJumping = false;
        m_hasDoubledJump = true;
        m_velocity.y += m_jumpHeight;
    }

    #region Gravity

    private void ApplyGravity()
    {
        if (m_isContactingWall && m_velocity.y < 0)
        {
            m_currentGravityScale = m_GravityScale * m_wallFallMultiplier;
        }
        else if (!PlatformJumpGravity())
        {
            m_currentGravityScale = m_GravityScale;
        }

        m_velocity += Physics.gravity * m_currentGravityScale * Time.deltaTime;
    }

    private bool PlatformJumpGravity()
    {
        //https://www.reddit.com/r/Unity3D/comments/60hchp/better_jumping_with_four_lines_of_code_youtube/

        // If the player is on the downward portion of its jump / falling
        if (m_velocity.y < 0)
        {
            m_currentGravityScale = m_GravityScale * m_fallMultiplier;
            return true;
        }

        //control jump height by length of time jump button held
        // apply more gravity if the player is not holding the jump button
        // and is moving upward.
        else if (m_velocity.y > 0 && !Input.GetButton("Jump"))
        {
            m_currentGravityScale = m_GravityScale * lowJumpMultiplier;
            return true;
        }

        // else use regular gravity
        return false;
    }

    #endregion
}
