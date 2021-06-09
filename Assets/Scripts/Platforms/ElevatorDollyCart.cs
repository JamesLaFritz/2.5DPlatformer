using System.Collections;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class ElevatorDollyCart : CinemachineDollyCart
{
    [SerializeField] private float m_desiredSpeed;

    [SerializeField] private int m_numberOfFloors = 2;

    public void Call(int floor)
    {
        StartCoroutine(MoveToFloor(floor));
    }

    private IEnumerator MoveToFloor(int floor)
    {
        Debug.Assert(m_Path != null, nameof(m_Path) + " != null");

        float desiredPosition = m_Path.FromPathNativeUnits(floor - 1, m_PositionUnits);

        if (!(m_Path.MaxUnit(CinemachinePathBase.PositionUnits.PathUnits) > 1) && m_numberOfFloors > 2)
        {
            desiredPosition = GetSlopePoint(
                m_Path.MinUnit(m_PositionUnits),
                m_Path.MaxUnit(m_PositionUnits),
                1, m_numberOfFloors, floor);
        }

        m_Path.StandardizePathDistance(m_Path.PathLength);

        //Debug.Log($"{floor}: {desiredPosition} : {m_Path.FromPathNativeUnits(floor, m_PositionUnits)}");

        if (m_Position > desiredPosition)
        {
            m_Speed = m_desiredSpeed * -1;

            while (m_Position > desiredPosition)
            {
                yield return null;
            }
        }
        else
        {
            m_Speed = m_desiredSpeed;
            while (m_Position < desiredPosition)
            {
                yield return null;
            }
        }


        m_Position = desiredPosition;
        m_Speed = 0;
    }

    /// <summary>
    /// Slope = rise/run = (y2 - y1) / (x2 - x1)
    /// </summary>
    private static float GetSlope(float yMin, float yMax, float xMin, float xMax)
    {
        float slope = (yMax - yMin) / (xMax - xMin);
        //Debug.Log($"Slope:({yMax} - {yMin}) /  ({xMax} - {xMin}) = {slope}");
        return (yMax - yMin) / (xMax - xMin);
    }

    /// <summary>
    /// y - y1 = m(x - x1)
    /// SlopPoint - yMin = Slope(x - xMin)
    /// Slope Point = Slope(x - xMin) + yMin
    /// </summary>
    private static float GetSlopePoint(float yMin, float yMax, float xMin, float xMax, float x)
    {
        float slope = GetSlope(yMin, yMax, xMin, xMax);
        float slopePoint = slope * (x - xMin) + yMin;
        //Debug.Log($"Slope Point: {slope} * ({x} - {xMin}) + {yMin} = {slopePoint}");
        return slopePoint;
    }
}
