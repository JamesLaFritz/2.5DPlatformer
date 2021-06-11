using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class PlayerLivesTextDisplay : MonoBehaviour
{
    private Text m_livesText;

    [SerializeField] private StatVariable m_lives;

    [SerializeField] private CodedGameEventListener m_livesChanged;

    private void OnEnable()
    {
        m_livesChanged?.OnEnable(OnLivesChanged);
    }

    private void OnDisable()
    {
        m_livesChanged?.OnDisable();
    }

    private void Awake()
    {
        m_livesText = GetComponent<Text>();

        if (m_lives == null)
        {
            Debug.LogException(new System.NullReferenceException(
                                   $"{name} requires Lives Stat Please add one in the Inspector"), this);
        }
    }

    private void Start()
    {
        OnLivesChanged();
    }

    private void OnLivesChanged()
    {
        Debug.Assert(m_livesText != null, nameof(m_livesText) + " != null");
        m_livesText.text = $"Lives: {m_lives.Value}";
    }
}
