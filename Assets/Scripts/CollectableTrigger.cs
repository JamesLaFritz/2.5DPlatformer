using UnityEngine;
using UnityEngine.Events;

public class CollectableTrigger : MonoBehaviour
{
    [SerializeField, Tag] private string m_playerTag = "Player";

    [SerializeField] private IntReference m_collectableCount;
    [SerializeField] private int m_amountNeeded = 8;

    [SerializeField] private UnityEvent m_activateEvent;
    [SerializeField] private UnityEvent m_errorEvent;


    private bool m_isCollidingWithPlayer;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Assert(other != null, nameof(other) + " != null");
        if (other.CompareTag(m_playerTag))
        {
            m_isCollidingWithPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Assert(other != null, nameof(other) + " != null");
        if (other.CompareTag(m_playerTag))
        {
            m_isCollidingWithPlayer = false;
        }
    }

    private void Update()
    {
        if (!m_isCollidingWithPlayer) return;

        if (!Input.GetButtonDown("Fire1")) return;

        Debug.Assert(m_collectableCount != null, nameof(m_collectableCount) + " != null");
        if (m_collectableCount.Value >= m_amountNeeded)
        {
            Activate();
        }
        else
        {
            Error();
        }
    }

    private void Activate()
    {
        m_activateEvent?.Invoke();
    }

    private void Error()
    {
        m_errorEvent?.Invoke();
    }
}
