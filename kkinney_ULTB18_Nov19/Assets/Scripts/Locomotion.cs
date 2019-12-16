using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;


public class Locomotion : MonoBehaviour
{
    public float m_Gravity = 30.0f;
    public float m_Sensitivity = 0.1f;
    public float m_MaxSpeed = 1.0f;

    public SteamVR_Action_Boolean m_MovePress = null;
    public SteamVR_Action_Vector2 m_MoveValue = null;

    private float m_Speed = 0.0f;

    private CharacterController m_CharacterController = null;
    private Transform m_CameraRig = null;
    private Transform m_Head = null;

    private void Awake()
    {
        m_CharacterController = GetComponent<CharacterController>();
    }


    private void Start()
    {
        m_CameraRig = SteamVR_Render.Top().origin;
        m_Head = SteamVR_Render.Top().head;
    }


    private void Update()
    {
        //HandleHead();
        HandleHeight();
        CalculateMovement();
    }

    #region HandleHead Unused
    /*
    private void HandleHead()
    {
        // Store current
        Vector3 oldPosition = m_CameraRig.position;
        Quaternion oldRotation = m_CameraRig.rotation;

        // Rotation
        transform.eulerAngles = new Vector3(0, m_Head.rotation.eulerAngles.y, 0);

        // Restore
        m_CameraRig.position = oldPosition;
        m_CameraRig.rotation = oldRotation;
    }
    */
    #endregion

    private void CalculateMovement()
    {
        // Figure out movement orientation
        Quaternion orientation = CalculateOrientation();
        Vector3 movement = Vector3.zero;

        // If not moving
        //if (m_MovePress.GetStateUp(SteamVR_Input_Sources.Any)) // Using any of the specified input from either controller
        if (m_MoveValue.axis.magnitude == 0)
            m_Speed = 0;

        // Add, Clamp
        //m_Speed = m_MoveValue.axis.y * m_Sensitivity; // Input with direction
        m_Speed = m_MoveValue.axis.magnitude * m_Sensitivity; // Input is x distance
        m_Speed = Mathf.Clamp(m_Speed, -m_MaxSpeed, m_MaxSpeed); // Use multiplier on -m_MaxSpeed to decrease walking backwards speed

        // Orientation and Gravity
        movement += orientation * (m_Speed * Vector3.forward);
        movement.y -= m_Gravity * Time.deltaTime;

        // Apply
        m_CharacterController.Move(movement * Time.deltaTime);
    }

    private void HandleHeight()
    {
        // Get the head in local space
        float headHeight = Mathf.Clamp(m_Head.localPosition.y, 1, 2);
        m_CharacterController.height = headHeight;

        // Cut in half
        Vector3 newCenter = Vector3.zero;
        newCenter.y = m_CharacterController.height / 2f;
        newCenter.y += m_CharacterController.skinWidth;

        #region Stops Movement into Walls
        // Move capusle in local space
        newCenter.x = m_Head.localPosition.x;
        newCenter.z = m_Head.localPosition.z;

        // Rotate
        newCenter = Quaternion.Euler(0, -transform.eulerAngles.y, 0) * newCenter;
        #endregion

        // Apply
        m_CharacterController.center = newCenter;
    }

    private Quaternion CalculateOrientation()
    {
        float rotation = Mathf.Atan2(m_MoveValue.axis.x, m_MoveValue.axis.y);
        rotation *= Mathf.Rad2Deg;

        Vector3 orientationEuler = new Vector3(0, m_Head.eulerAngles.y + rotation, 0);
        Quaternion orientation = Quaternion.Euler(orientationEuler);

        return orientation;
    }
}

