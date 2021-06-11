using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance => _instance;

    [SerializeField] private bool m_isFirstLevel;

    [SerializeField] private IntReference m_coins;

    [SerializeField] private StatVariable m_playerLives;
    [SerializeField] private CodedGameEventListener m_playerDiedEventListener;

    private GameObject m_player;
    private bool m_hasPlayer;

    [SerializeField, Scene] private int m_gameOverScene;

    [SerializeField] private Transform m_spawnPoint;

    public Transform SpawnPoint
    {
        get => m_spawnPoint;
        set
        {
            if (value == null)
            {
                Debug.LogError(new System.NullReferenceException("Spawn Point can not be Null!!"));
                return;
            }

            m_spawnPoint = value;
        }
    }

    private void OnEnable()
    {
        m_playerDiedEventListener?.OnEnable(OnPlayerDied);
    }

    private void OnDisable()
    {
        m_playerDiedEventListener?.OnDisable();
    }

    private void Awake()
    {
        if (m_playerLives == null)
        {
            Debug.LogException(new System.NullReferenceException(
                                   $"{name} requires Lives Stat Please add one in the Inspector"), this);
        }

        _instance = this;
    }

    private void Start()
    {
        InitFirstLevel();
        SetUpPlayer();
    }

    private void InitFirstLevel()
    {
        if (!m_isFirstLevel) return;

        Debug.Assert(m_playerLives != null, nameof(m_playerLives) + " != null");
        m_playerLives.ResetStat();

        Debug.Assert(m_coins != null, nameof(m_coins) + " != null");
        m_coins.Value = 0;
    }

    private void SetUpPlayer()
    {
        m_player = GameObject.FindWithTag("Player");
        m_hasPlayer = m_player != null;

        if (!m_hasPlayer) return;

        Debug.Assert(m_player != null, nameof(m_player) + " != null");
        if (m_spawnPoint == null)
        {
            m_spawnPoint = new GameObject("Start Point").transform;
            m_spawnPoint.transform.SetPositionAndRotation(m_player.transform.position, m_player.transform.rotation);
            return;
        }

        OnPlayerDied();
    }

    private void OnPlayerDied()
    {
        if (m_playerLives.Value <= 0)
        {
            SceneManager.LoadScene(m_gameOverScene);
        }

        if (!m_hasPlayer) return;

        // move the player to the spawn position
        Debug.Assert(m_player != null, nameof(m_player) + " != null");
        Debug.Assert(m_spawnPoint != null, nameof(m_spawnPoint) + " != null");
        m_player.SetActive(false);
        m_player.transform.SetPositionAndRotation(m_spawnPoint.position, m_spawnPoint.rotation);
        m_player.SetActive(true);
    }
}
