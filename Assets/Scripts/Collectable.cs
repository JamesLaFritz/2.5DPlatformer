using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField, Tag] private string m_playerTag = "Player";

    [SerializeField] private IntReference m_count;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"{other.name} with tag {other.tag} has collided with {name}");
        if (other.CompareTag(m_playerTag))
        {
            Debug.Assert(m_count != null, nameof(m_count) + " != null");
            m_count.Value++;

            Destroy(gameObject);
        }
    }
}
