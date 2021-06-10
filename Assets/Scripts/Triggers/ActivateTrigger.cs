using UnityEngine;
using UnityEngine.Events;

public class ActivateTrigger : MonoBehaviour
{
    [SerializeField, Tag] private string m_playerTag = "Player";


    private bool m_isCollidingWithPlayer;

    [SerializeField] private string m_activateInput = "Fire1";

    [SerializeField] protected UnityEvent activateEvent;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Assert(other != null, nameof(other) + " != null");
        if (!other.CompareTag(m_playerTag)) return;

        m_isCollidingWithPlayer = true;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Assert(other != null, nameof(other) + " != null");
        if (!other.CompareTag(m_playerTag)) return;

        m_isCollidingWithPlayer = false;
    }

    private void Update()
    {
        if (!m_isCollidingWithPlayer) return;

        if (Input.GetButtonDown(m_activateInput))
        {
            Activate();
        }
    }

    protected virtual void Activate()
    {
        activateEvent?.Invoke();
    }
}
