using System.Collections;
using Cinemachine;
using UnityEngine;

public class ElevatorDollyCart : CinemachineDollyCart
{
    [SerializeField] private float m_desiredSpeed;

    public void Call(bool moveDown)
    {
        if (moveDown)
        {
            m_Speed = m_desiredSpeed * -1;
        }
        else
        {
            m_Speed = m_desiredSpeed;
        }
    }

    public void Call(int floor)
    {
        StartCoroutine(MoveToFloor(floor));
    }

    private IEnumerator MoveToFloor(int floor)
    {
        yield return null;

        float desiredPosition = floor;

        // switch (m_PositionUnits)
        // {
        //     case CinemachinePathBase.PositionUnits.PathUnits:
        //         desiredPosition = floor;
        //         break;
        //     case CinemachinePathBase.PositionUnits.Distance:
        //         break;
        //     case CinemachinePathBase.PositionUnits.Normalized:
        //         break;
        //     default:
        //         throw new System.ArgumentOutOfRangeException();
        // }

        if (m_Position > desiredPosition)
        {
            m_Speed = m_desiredSpeed * -1;

            while (m_Position > desiredPosition)
            {
                Debug.Log($"{m_Position} : {desiredPosition}");
                yield return null;
            }
        }
        else
        {
            m_Speed = m_desiredSpeed;
            while (m_Position < desiredPosition)
            {
                Debug.Log($"{m_Position} : {desiredPosition}");
                yield return null;
            }
        }

        m_Speed = 0;
        m_Position = desiredPosition;
    }
}
