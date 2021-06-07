using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform m_targetA, m_targetB;

    [SerializeField] private float m_speed = 1.0f;

    private Vector3 m_targetAPosition, m_targetBPosition, m_targetPosition, m_position;

    private void Start()
    {
        if (m_targetA == null || m_targetB == null)
        {
            string message = $"{name} needs targets assigned in the inspector!\n";
            if (m_targetA == null)
                message += "Target A can not be null!\n";
            if (m_targetB == null)
                message += "Target B can not be null!";
            Debug.LogException(new System.NullReferenceException(message));
        }

        Debug.Assert(m_targetA != null, nameof(m_targetA) + " != null");
        m_targetAPosition = m_targetA.position;
        Debug.Assert(m_targetB != null, nameof(m_targetB) + " != null");
        m_targetBPosition = m_targetPosition = m_targetB.position;
    }

    private void FixedUpdate()
    {
        m_position = Vector3.MoveTowards(transform.position, m_targetPosition,
                                         m_speed * Time.deltaTime);

        if (m_position == m_targetBPosition)
            m_targetPosition = m_targetAPosition;
        else if (m_position == m_targetAPosition)
            m_targetPosition = m_targetBPosition;

        transform.position = m_position;
    }
}
