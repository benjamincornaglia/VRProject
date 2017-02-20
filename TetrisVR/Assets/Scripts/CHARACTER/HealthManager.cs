using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour {

    [SerializeField]
    [Range(0, 100)]
    private float m_fLife = 100f;

    float m_fInitialLife;
    GameObject m_pHealthBar;

    enum PlayMode { VR, Debug };
    [SerializeField]
    PlayMode m_ePlayMode;

    // Use this for initialization
    void Start () {
        m_fInitialLife = m_fLife;
        
        switch (m_ePlayMode)
        {
            case PlayMode.Debug:
                m_pHealthBar = GameObject.Find("HealthBar");
                break;
            case PlayMode.VR:
                m_pHealthBar = GameObject.Find("VRHealthBar");
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
        HealthInput(-Time.deltaTime);
	}

    public void HealthInput(float _fAmount)
    {
        m_fLife += _fAmount;
        if (m_pHealthBar != null)
            m_pHealthBar.GetComponent<Slider>().value = m_fLife / m_fInitialLife;

        m_fLife = Mathf.Clamp(m_fLife, 0, m_fInitialLife);
        if (m_fLife <= 0)
            Die();
    }

    void Die()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
