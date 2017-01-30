using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;
using Valve.VR;
using DG.Tweening;

//[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    #region CHR variables
    //Character
    [SerializeField]
    [Range(0,10)]
    private float m_fMoveSpeed = 10f;
    [SerializeField]
    [Range(100, 200)]
    private float m_fAdditionalGravity = 100f;
    private float m_fInitialMoveSpeed;
    public bool m_bIsInCollision = true;
    enum StateMachine { StartJump, Jump, EndJump, Airborn, StartRun, Run, EndRun };
    StateMachine m_eStateMachine;
    public GameObject m_pMyController;
    GameObject m_pEnergyBar;
    GameObject m_pLeftEnergyBar;
    GameObject m_pRightEnergyBar;
    [SerializeField]
    [Range(0, 5)]
    private float m_fEnergy = 0.5f;
    float m_fActualEnergy;
    bool m_bIsDashing = false;
    #endregion

    #region Debug variables
    //Camera
    
    private float m_fSpeedH = 2.0f;
    
    private float m_fSpeedV = 2.0f;

    public GameObject Projectile;

    private float m_fYaw = 0.0f;
    private float m_fPitch = 0.0f;
    #endregion

    #region VR variables
    private Valve.VR.EVRButtonId dPadUp = Valve.VR.EVRButtonId.k_EButton_Axis0;
    public SteamVR_TrackedObject trackedObject;
    private SteamVR_Controller.Device device;
    public GameObject m_pVRController;
    #endregion

    #region Modes Variables
    enum MoveMode{ Dash_Thor, Dash_IronMan, Dash_Vision, Jump_Godzilla };
    [SerializeField]
    MoveMode m_eMoveMode;
    enum PlayMode { VR, Debug };
    [SerializeField]
    PlayMode m_ePlayMode;
#endregion

    // Use this for initialization
    void Start()
    {

        m_eStateMachine = StateMachine.Run;
        Cursor.visible = false;
        m_fInitialMoveSpeed = m_fMoveSpeed;
        m_pEnergyBar = GameObject.Find("EnergyBar");
        m_pLeftEnergyBar = GameObject.Find("LeftEnergyBar");
        m_pRightEnergyBar = GameObject.Find("RightEnergyBar");
        m_fActualEnergy = m_fEnergy;

        switch(m_ePlayMode)
        {
            case PlayMode.Debug:
                m_pMyController = this.gameObject;
                break;
            case PlayMode.VR:
                m_pMyController = m_pVRController;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        #region Recherche du steam VR
        if (trackedObject == null && m_pVRController != null)
        {
            trackedObject = GetComponent<SteamVR_TrackedObject>();
        }

        if (trackedObject)
            device = SteamVR_Controller.Input((int)trackedObject.index);
        #endregion

        switch(m_ePlayMode)
        {
            case PlayMode.Debug:
                KeyboardInput();
                Look();
                StateMachineControl();
                m_pMyController.GetComponent<Rigidbody>().AddForce(-Vector3.up * m_fAdditionalGravity, ForceMode.Acceleration);
                break;
            case PlayMode.VR:
                VRControllerInput();
                StateMachineControl();
                m_pMyController.GetComponent<Rigidbody>().AddForce(-Vector3.up * m_fAdditionalGravity, ForceMode.Acceleration);
                break;

        }

        EnergyBarBehavior();
    }

    void StateMachineControl()
    {
        switch (m_eStateMachine)
        {
            case StateMachine.StartJump:
                if (m_eMoveMode == MoveMode.Jump_Godzilla && m_bIsInCollision)
                    Dash();
                m_eStateMachine = StateMachine.Jump;
                break;
            case StateMachine.Jump:
                if (m_eMoveMode != MoveMode.Jump_Godzilla)
                    Dash();
                break;
            case StateMachine.EndJump:
                
                break;
            case StateMachine.Airborn:
                
                break;
            case StateMachine.StartRun:
                
                break;
            case StateMachine.Run:
                
                break;
        }
    }

    // (Debug) Use to shoot projectiles
	/*public void ShootVR(Transform vControllerTransform)
	{
		
		var p = Instantiate(Projectile, vControllerTransform.position + vControllerTransform.forward, transform.rotation);
		p.GetComponent<Rigidbody>().AddForce(vControllerTransform.forward * 10000.0f, ForceMode.Impulse);

	}*/

    // (Debug) Used to look around using the mouse
    void Look()
    {
        m_fYaw += m_fSpeedH * Input.GetAxis("Mouse X");
        m_fPitch -= m_fSpeedV * Input.GetAxis("Mouse Y");

        Camera.main.transform.eulerAngles = new Vector3(m_fPitch, transform.eulerAngles.y, 0.0f);
        m_pMyController.transform.eulerAngles = new Vector3(0, m_fYaw, 0.0f);
    }

    // (Debug/VR) Used to move around
    void Dash()
    {
        if (m_fActualEnergy >= 0)
        {
            switch (m_eMoveMode)
            {
                case MoveMode.Jump_Godzilla:
                    m_pMyController.GetComponent<Rigidbody>().AddForce(transform.up * m_fMoveSpeed * 2 * 30, ForceMode.VelocityChange);
                    m_pMyController.GetComponent<Rigidbody>().AddForce(transform.forward * m_fMoveSpeed * 30, ForceMode.VelocityChange);
                    break;
                case MoveMode.Dash_Vision:
                    m_pMyController.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * m_fMoveSpeed, ForceMode.VelocityChange);
                    break;
                case MoveMode.Dash_Thor:
                    m_pMyController.GetComponent<Rigidbody>().AddForce(transform.forward * m_fMoveSpeed, ForceMode.VelocityChange);
                    break;
                case MoveMode.Dash_IronMan:
                    m_pMyController.GetComponent<Rigidbody>().AddForce(-transform.forward * m_fMoveSpeed, ForceMode.VelocityChange);
                    break;
            }

            m_fActualEnergy -= Time.deltaTime;
        }
          
    }

    void VRControllerInput()
    {
        if (device == null)
        {
            return;
        }

		if (device.GetPressDown(dPadUp))
        {
			m_eStateMachine = StateMachine.StartJump;
            m_bIsInCollision = false;
            m_bIsDashing = true;
        }

		if(device.GetPressUp(dPadUp))
        {
			m_eStateMachine = StateMachine.EndJump;
            m_bIsDashing = false;
        }
    }

	void KeyboardInput()
	{
		if (Input.GetKeyDown(KeyCode.Space))
        {
            m_eStateMachine = StateMachine.StartJump;
            m_bIsInCollision = false;
            m_bIsDashing = true;
        }
						
		
		if(Input.GetKeyUp(KeyCode.Space))
        {
            m_eStateMachine = StateMachine.EndJump;			
            m_bIsDashing = false;
        }
	}

    void EnergyBarBehavior()
    {
        if (m_fActualEnergy <= m_fEnergy && !m_bIsDashing && m_bIsInCollision)
            m_fActualEnergy += Time.deltaTime;

        float fEnergyScaling = m_fActualEnergy / (m_fEnergy * 10);

        switch(m_ePlayMode)
        {
            case PlayMode.Debug:
                m_pEnergyBar.transform.localScale = new Vector3(fEnergyScaling, m_pEnergyBar.transform.localScale.y, m_pEnergyBar.transform.localScale.z);
                break;
            case PlayMode.VR:
                m_pLeftEnergyBar.transform.localScale = new Vector3(fEnergyScaling, m_pLeftEnergyBar.transform.localScale.y, m_pLeftEnergyBar.transform.localScale.z);
                m_pRightEnergyBar.transform.localScale = new Vector3(fEnergyScaling, m_pRightEnergyBar.transform.localScale.y, m_pRightEnergyBar.transform.localScale.z);
                break;
        }
        
        //Debug.Log(m_pEnergyBar.transform.localScale);
    }

    private void OnCollisionEnter(Collision collision)
    {  
        m_bIsInCollision = true;
        m_pMyController.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

}
