using UnityEngine;
using System.Collections;

public class Hud_XPBar_Manager : MonoBehaviour {

    public static Hud_XPBar_Manager Instance;

	[SerializeField] private UILabel Label_XPValue;

    private float m_currentXP;
    private float m_targetXP;
    private bool m_changingXP;

    void Awake() { Instance = this; }

    void Update()
    {
        if (m_currentXP < m_targetXP)
        {
            m_currentXP += 100 * Time.deltaTime;
            if (m_currentXP > m_targetXP) m_currentXP = m_targetXP;
            Label_XPValue.text = "" + (int)m_currentXP;
        }
    }
	//mm
	[SerializeField]
    private UILabel karmaVal;
    public void SetKarmaMissionVal(int val)
    {
        karmaVal.text = val.ToString();
    }
	//#mm
    public void InitXP(float _curxp)
    {
        m_currentXP = _curxp;
        m_targetXP = _curxp;
        Label_XPValue.text = "" + (int)m_currentXP;
    }

    public void SetNewXP(float _targetxp)
    {
        m_targetXP = _targetxp;
    }
	
	//mm
	public void DestroyXP(){
		Destroy(gameObject);
	}
	//#mm
}
