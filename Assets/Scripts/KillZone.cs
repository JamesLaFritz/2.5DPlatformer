using UnityEngine;

public class KillZone : MonoBehaviour
{
    [SerializeField, Tag] private string m_PlayerTag = "Player";

    [SerializeField] private StatVariable m_playerLives;

    private void Awake()
    {
        if (m_playerLives == null)
        {
            Debug.LogException(new System.NullReferenceException(
                                   $"{name} requires Lives Stat Please add one in the Inspector"), this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Assert(other != null, nameof(other) + " != null");
        if (!other.CompareTag(m_PlayerTag)) return;

        Debug.Assert(m_playerLives != null, nameof(m_playerLives) + " != null");
        m_playerLives.Add(-1);
    }
}
