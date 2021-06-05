using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

public class ElevatorPanel : MonoBehaviour
{
    [SerializeField] private Renderer m_lightRender;
    private bool m_hasLightRender;
    private MaterialPropertyBlock m_materialPropertyBlock;
    private static readonly int ColorPropertyName = Shader.PropertyToID("_Color");
    [SerializeField] private Color m_activateColor = Color.green;

    [SerializeField, Tag] private string m_playerTag = "Player";

    private bool m_isCollidingWithPlayer;

    private void Awake()
    {
        m_hasLightRender = m_lightRender != null;
        m_materialPropertyBlock = new MaterialPropertyBlock();
    }

    private void OnTriggerEnter(Collider other)
    {
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

        if (Input.GetButtonDown("Fire1"))
        {
            ChangeColor();
        }
    }

    private void ChangeColor()
    {
        if (!m_hasLightRender) return;

        Debug.Assert(m_lightRender != null, nameof(m_lightRender) + " != null");
        Debug.Assert(m_materialPropertyBlock != null, nameof(m_materialPropertyBlock) + " != null");
        m_lightRender.GetPropertyBlock(m_materialPropertyBlock);
        m_materialPropertyBlock.SetColor(ColorPropertyName, m_activateColor);
        m_lightRender.SetPropertyBlock(m_materialPropertyBlock);
    }
}
