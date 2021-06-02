using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [SerializeField] private float m_horizontalMoveSpeed = 5f;
    private CharacterController m_characterController;

    private float m_horizontal;
    private Vector3 m_moveDirection = Vector3.zero;
    private Vector3 m_velocity = Vector3.zero;

    [SerializeField] private bool m_useGravity = true;
    private bool m_isGrounded;
    private float m_GravityScale = 1f;

    [SerializeField] private float m_jumpHeight = 6f;
    private bool m_hasDoubledJump;

    private void Start()
    {
        m_characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        m_horizontal = Input.GetAxis("Horizontal");
        m_moveDirection.x = m_horizontal;
        m_velocity.x = m_moveDirection.x * m_horizontalMoveSpeed;

        Debug.Assert(m_characterController != null, nameof(m_characterController) + " != null");
        m_isGrounded = m_characterController.isGrounded;

        if (m_isGrounded)
        {
            m_hasDoubledJump = false;
            if (Input.GetButtonDown("Jump"))
            {
                m_velocity.y = m_jumpHeight;
            }
        }
        else
        {
            if (!m_hasDoubledJump && Input.GetButtonDown("Jump"))
            {
                m_hasDoubledJump = true;
                m_velocity.y += m_jumpHeight;
            }

            if (m_useGravity)
            {
                m_velocity += Physics.gravity * m_GravityScale * Time.deltaTime;
            }
        }

        m_characterController.Move(m_velocity * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (m_velocity.y < 0)
        {
            m_GravityScale = 2.5f;
        }
        else if (m_velocity.y > 0 && !Input.GetButtonDown("Jump"))
        {
            m_GravityScale = 2f;
        }
        else
        {
            m_GravityScale = 1f;
        }
    }
}
