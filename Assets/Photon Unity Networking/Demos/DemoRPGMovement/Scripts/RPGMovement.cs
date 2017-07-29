using UnityEngine;
using System.Collections;

[RequireComponent( typeof( CharacterController ) )]
public class RPGMovement : MonoBehaviour
{
    public float ForwardSpeed;
    public float BackwardSpeed;
    public float StrafeSpeed;
    public float RotateSpeed;

    CharacterController m_CharacterController;
    Vector3 m_LastPosition;
    Animator m_Animator;
    PhotonView m_PhotonView;
    PhotonTransformView m_TransformView;

    float m_AnimatorSpeed;
    Vector3 m_CurrentMovement;
    float m_CurrentTurnSpeed;

    void Start()
    {
        Debug.Log("Movement start()");
        m_CharacterController = GetComponent<CharacterController>();
        m_Animator = GetComponent<Animator>();
        m_PhotonView = GetComponent<PhotonView>();
        m_TransformView = GetComponent<PhotonTransformView>();
    }

    void Update()
    {
        
        if ( m_PhotonView.isMine == true )
        {
            Debug.Log("Movement Update(if)");
            ResetSpeedValues();

            UpdateRotateMovement();

            UpdateForwardMovement();
            UpdateBackwardMovement();
            UpdateStrafeMovement();

            MoveCharacterController();
            ApplyGravityToCharacterController();

            ApplySynchronizedValues();
        }
        else
        {
            Debug.Log("--------------Movement Update()");
        }

        UpdateAnimation();
    }

    void UpdateAnimation()
    {
        //Debug.Log("Movement UPdateAnimation()");
        Vector3 movementVector = transform.position - m_LastPosition;

        float speed = Vector3.Dot( movementVector.normalized, transform.forward );
        float direction = Vector3.Dot( movementVector.normalized, transform.right );

        if( Mathf.Abs( speed ) < 0.2f )
        {
            speed = 0f;
        }

        if( speed > 0.6f )
        {
            speed = 1f;
            direction = 0f;
        }

        if( speed >= 0f )
        {
            if( Mathf.Abs( direction ) > 0.7f )
            {
                speed = 1f;
            }
        }

        m_AnimatorSpeed = Mathf.MoveTowards( m_AnimatorSpeed, speed, Time.deltaTime * 5f );

        m_Animator.SetFloat( "Speed", m_AnimatorSpeed );
        m_Animator.SetFloat( "Direction", direction );

        m_LastPosition = transform.position;
    }

    void ResetSpeedValues()
    {
        //Debug.Log("Movement ResetSpeedValue()");
        m_CurrentMovement = Vector3.zero;
        m_CurrentTurnSpeed = 0;
    }

    void ApplySynchronizedValues()
    {
        //Debug.Log("Movement ApplySynchronizedV()");
        m_TransformView.SetSynchronizedValues( m_CurrentMovement, m_CurrentTurnSpeed );
    }

    void ApplyGravityToCharacterController()
    {
       // Debug.Log("Movement ApplyGravityToCharac()");
        m_CharacterController.Move( transform.up * Time.deltaTime * -9.81f );
    }

    void MoveCharacterController()
    {
        //Debug.Log("Movement MoveCharacterC()");
        m_CharacterController.Move( m_CurrentMovement * Time.deltaTime );
    }

    void UpdateForwardMovement()
    {
        //Debug.Log("Movement UPdateForwardMovement");
        if ( Input.GetKey( KeyCode.W ) || Input.GetAxisRaw("Vertical") > 0.1f )
        {
            m_CurrentMovement = transform.forward * ForwardSpeed;
        }
    }

    void UpdateBackwardMovement()
    {
       // Debug.Log("Movement UpdateBackwardMovement()");
        if ( Input.GetKey( KeyCode.S ) || Input.GetAxisRaw("Vertical") < -0.1f )
        {
            m_CurrentMovement = -transform.forward * BackwardSpeed;
        }
    }

    void UpdateStrafeMovement()
    {
       // Debug.Log("Movement UPdateStrafeMovement");
        if ( Input.GetKey( KeyCode.Q ) == true )
        {
            m_CurrentMovement = -transform.right * StrafeSpeed;
        }

        if( Input.GetKey( KeyCode.E ) == true )
        {
            m_CurrentMovement = transform.right * StrafeSpeed;
        }
    }

    void UpdateRotateMovement()
    {
        //Debug.Log("Movement UpdateRotateMovement");
        if ( Input.GetKey( KeyCode.A ) || Input.GetAxisRaw("Horizontal") < -0.1f )
        {
            m_CurrentTurnSpeed = -RotateSpeed;
            transform.Rotate(0.0f, -RotateSpeed * Time.deltaTime, 0.0f);
        }

        if( Input.GetKey( KeyCode.D ) || Input.GetAxisRaw("Horizontal") > 0.1f )
        {
            m_CurrentTurnSpeed = RotateSpeed;
            transform.Rotate(0.0f, RotateSpeed * Time.deltaTime, 0.0f);
        }
    }
}