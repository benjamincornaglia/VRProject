using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;
using Valve.VR;
using DG.Tweening;

//[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{

    //Character
    [SerializeField]
    private float m_fMoveSpeed = 10f;
    [SerializeField]
    private float m_fTravelSpeed = 10f;

    //Camera
    [SerializeField]
    private float m_fSpeedH = 2.0f;
    [SerializeField]
    private float m_fSpeedV = 2.0f;

    public GameObject Projectile;


    private float m_fYaw = 0.0f;
    private float m_fPitch = 0.0f;


	private Valve.VR.EVRButtonId dPadUp = Valve.VR.EVRButtonId.k_EButton_Axis0;
    public SteamVR_TrackedObject trackedObject;
    private SteamVR_Controller.Device device;

    private GameObject m_pTravelTarget;

    public GameObject m_pVRController;

    bool m_bIsTravelling = false;
    bool m_bIsInCollision = true;

    enum StateMachine{ StartJump, Jump, EndJump, Airborn, StartRun, Run, EndRun };
    StateMachine m_eStateMachine;

    GameObject m_pTmpTravelTarget;

    // Use this for initialization
    void Start()
    {

        m_eStateMachine = StateMachine.Run;
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
		VRControllerInput ();
        //DetectTravelTarget();

        if(m_pVRController == null)
            Look();

        Shoot();

        StateMachineControl();

        
    }

    void StateMachineControl()
    {
        switch (m_eStateMachine)
        {
            case StateMachine.StartJump:
                Debug.Log("StartJump");
                m_pTmpTravelTarget = m_pTravelTarget;
                m_eStateMachine = StateMachine.Jump;
                m_bIsInCollision = false;
                break;
            case StateMachine.Jump:
                JumpToTarget(m_pTmpTravelTarget.transform.position);
                //EndJumpToTarget(m_pTmpTravelTarget.transform.position);
                break;
            case StateMachine.EndJump:
                Debug.Log("End Jump");
                m_eStateMachine = StateMachine.Airborn;
                break;
            case StateMachine.Airborn:
                if (m_bIsInCollision)
                    m_eStateMachine = StateMachine.StartRun;
                else
                    MoveAirborn();
                break;
            case StateMachine.StartRun:
                m_eStateMachine = StateMachine.Run;
                break;
            case StateMachine.Run:
                Move();
                break;
        }
    }

    void Shoot()
    {
        if (Input.GetMouseButtonDown(0) && Projectile != null)
        {
            var p = Instantiate(Projectile, transform.position, transform.rotation);
            p.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 100.0f, ForceMode.Impulse);
        }
    }

    void Move()
    {
        if (Camera.main == null)
            return;
        if (m_pVRController != null)
            return;

        
        if (Input.GetKey(KeyCode.Z))
            transform.position += transform.forward * m_fMoveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S))
            transform.position -= transform.forward * m_fMoveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D))
            transform.position += Camera.main.transform.right * m_fMoveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.Q))
            transform.position -= Camera.main.transform.right * m_fMoveSpeed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {

            if (m_pTravelTarget != null)
            {
                m_eStateMachine = StateMachine.StartJump;
            }
        }
    }

    void MoveAirborn()
    {
        if (Input.GetKey(KeyCode.Z))
            transform.position += transform.forward * m_fMoveSpeed/2 * Time.deltaTime;
        if (Input.GetKey(KeyCode.S))
            transform.position -= transform.forward * m_fMoveSpeed/2 * Time.deltaTime;
        if (Input.GetKey(KeyCode.D))
            transform.position += Camera.main.transform.right * m_fMoveSpeed/2 * Time.deltaTime;
        if (Input.GetKey(KeyCode.Q))
            transform.position -= Camera.main.transform.right * m_fMoveSpeed/2 * Time.deltaTime;
    }

    void Look()
    {
        m_fYaw += m_fSpeedH * Input.GetAxis("Mouse X");
        m_fPitch -= m_fSpeedV * Input.GetAxis("Mouse Y");

        Camera.main.transform.eulerAngles = new Vector3(m_fPitch, transform.eulerAngles.y, 0.0f);
        transform.eulerAngles = new Vector3(0, m_fYaw, 0.0f);
    }

    void JumpToTarget(Vector3 vPos)
    {
        //Debug.Log(vPos);
        if(m_pVRController == null)
             transform.position = Vector3.Lerp(transform.position, vPos, Time.deltaTime * m_fTravelSpeed);
         else
			m_pVRController.transform.DOJump(vPos,20f,1,2,false);
		
             //m_pVRController.transform.position = Vector3.Lerp(m_pVRController.transform.position, vPos, Time.deltaTime * m_fTravelSpeed);

        
    }

    void VRControllerInput()
    {
        if (device == null)
        {
            return;
        }

		if (device.GetPressDown(dPadUp))
        {
			Debug.Log ("dPadUp Pressed");
			if (m_pTravelTarget != null) {
				
				m_eStateMachine = StateMachine.StartJump;
			}

        }

		if(device.GetPressUp(dPadUp))
			m_eStateMachine = StateMachine.EndJump;
    }

    void EndJumpToTarget(Vector3 vPos)
    {
        if (m_pVRController == null && Vector3.Distance(transform.position, vPos) <= 5f)
        {
            
            //Debug.Log("EndJump");
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        m_bIsInCollision = true;
    }
    #region Public methods
    public void DetectTravelTarget(GameObject pTravelTarget, bool bHasTarget)
    {
        if (bHasTarget && pTravelTarget != null)
        {
            m_pTravelTarget = pTravelTarget;
            m_pTravelTarget.transform.localScale = new Vector3(7, 7, 7);
            m_pTravelTarget.GetComponent<Renderer>().material.color = new Color(255, 255, 255);
        }
        else if (!bHasTarget && m_pTravelTarget != null)
        {
            m_pTravelTarget.transform.localScale = new Vector3(5, 5, 5);
            m_pTravelTarget.GetComponent<Renderer>().material.color = new Color(255, 0, 0);
            m_pTravelTarget = null;
        }
    }
    #endregion
}
