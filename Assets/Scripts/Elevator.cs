using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private Transform[] m_floors;
    private bool m_hasFloors;
    private bool[] m_floorsValid;

    [SerializeField] private int m_currentFloor;

    [SerializeField] private float m_speed = 4;

    private Vector3 m_targetPosition, m_position;

    private int m_direction = 1;

    private bool m_shouldMove;

    private void Awake()
    {
        if (m_floors == null || m_floors.Length < 1)
        {
            string message = $"{name} needs floors assigned in the inspector!\n";
            Debug.LogException(new System.NullReferenceException(message), this);
            return;
        }

        m_hasFloors = true;
        m_floorsValid = new bool[m_floors.Length];

        for (int i = 0; i < m_floors.Length; i++)
        {
            m_floorsValid[i] = m_floors[i] != null;
        }
    }

    public void CallElevator(int floor, int moveDirection)
    {
        if (!m_hasFloors) return;

        Debug.Assert(m_floors != null, nameof(m_floors) + " != null");
        m_currentFloor = Mathf.Clamp(floor - 1, 0, m_floors.Length - 1);
        m_direction = moveDirection;
        SetTargetPosition();
        m_shouldMove = true;
    }

    public void CallElevator(int floor)
    {
        CallElevator(floor, 1);
    }


    private void SetTargetPosition()
    {
        if (!m_hasFloors) return;

        Debug.Assert(m_floorsValid != null, nameof(m_floorsValid) + " != null");
        if (!m_floorsValid[m_currentFloor])
        {
            NextPosition();
            return;
        }

        Debug.Assert(m_floors != null, nameof(m_floors) + " != null");
        Debug.Assert(m_floors[m_currentFloor] != null, nameof(m_floors) + " != null");
        m_targetPosition = m_floors[m_currentFloor].position;
    }

    private void NextPosition()
    {
        m_currentFloor += m_direction;

        Debug.Assert(m_floors != null, nameof(m_floors) + " != null");
        if (m_currentFloor < 0 || m_currentFloor >= m_floors.Length)
        {
            m_direction *= -1;
            m_currentFloor += m_direction;
        }

        SetTargetPosition();
    }

    private void FixedUpdate()
    {
        if (!m_shouldMove) return;

        m_position = Vector3.MoveTowards(transform.position, m_targetPosition,
                                         m_speed * Time.deltaTime);
        transform.position = m_position;

        if (m_position != m_targetPosition) return;

        NextPosition();
    }
}