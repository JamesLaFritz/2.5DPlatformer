using UnityEngine;

/// <summary>
/// When the Player enters the trigger make  us the player's parent.
/// </summary>
public class MovePlayerWithPlatform : MonoBehaviour
{
    [SerializeField, Tag] private string m_playerTag = "Player";

    private Transform m_otherTransform;
    private bool m_hasOtherTransform;

    private Vector3 m_lastPosition;

    private void FixedUpdate()
    {
        if (!m_hasOtherTransform) return;

        Debug.Assert(m_otherTransform != null, nameof(m_otherTransform) + " != null");
        m_otherTransform.position += transform.position - m_lastPosition;
        m_lastPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Assert(other != null, nameof(other) + " != null");
        if (!other.CompareTag(m_playerTag)) return;

        m_lastPosition = transform.position;
        m_otherTransform = other.transform;
        m_hasOtherTransform = true;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Assert(other != null, nameof(other) + " != null");
        if (!other.CompareTag(m_playerTag)) return;

        m_hasOtherTransform = false;
        m_otherTransform = null;
    }
}