using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
    ThirdPersonCharacter m_Character;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentDestination, clickPoint; //where in the world we're clicking to move to
    String layerName;    //

    [SerializeField] float walkMoveStopRadius = 0.2f;
    [SerializeField] float attackMoveStopRadius = 2f;
    [SerializeField] float rangedMoveStopRadius = 0.2f;

    //it's added down here because all the public goes together, all the variables together and all the private, etc 10-25
    bool isInDirectMode = false; //added 10-25 
    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        m_Character = GetComponent<ThirdPersonCharacter>();
        currentDestination = transform.position;
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        // we removed the content and renamed to processmousemovement 
        // by selecting what we want to pull out, right click for 'quick actions and refactoring' then extract. Rename the method

        if (Input.GetKeyDown(KeyCode.G)) //G for gamepad. TODO allow player to read map later 
        {
            isInDirectMode = !isInDirectMode; // toggle mode 10-25
        }

        if (isInDirectMode)
        {
            ProcessDirectMovement();
        }

        else
        {
            ProcessMouseMovement(); // mouse movement.
        }
        
    }
    
    private void ProcessDirectMovement() //all 10-25
    {
        // borrowing this code from the old third person user controller 10-25

        // read inputs
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical"); //change crossplatforminputmanager to input 10-25
        
        //since we always want relative camera, we're gonna steal that too

        //after copy-pasta'ing, add the Vector3s
        //then change the m_Cam to Camera.main.transform
        // calculate camera relative direction to move:
        Vector3 m_CamForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 m_Move = v * m_CamForward + h * Camera.main.transform.right;
        
        m_Character.Move(m_Move, false, false); //steal this from line 93 below, then change Vector3.zero to m_Move
    }
    private void ProcessMouseMovement()
    {
        if (Input.GetMouseButton(0))
        {
            clickPoint = cameraRaycaster.hit.point;
            switch (cameraRaycaster.layerHit)
            {
                case Layer.Walkable:
                    //currentDestination = clickPoint; //after you've explained the change, right click to rename currentClickTarget to currentDestination
                    //then remove this line and add the below
                    currentDestination = ShortDestination(clickPoint, walkMoveStopRadius);
                    break;
                case Layer.Enemy:
                    currentDestination = ShortDestination(clickPoint, attackMoveStopRadius);
                    //print("Nice try");
                    break;
                default:
                    print("NOPE! NOT JUMPING OFF THE CLIFF");
                    return;
            }
        }
        WalkToDestination();
    }

    private void WalkToDestination()
    {
        var playerToClickPoint = currentDestination - transform.position;
        if (playerToClickPoint.magnitude >= 0.1)
        {
            m_Character.Move(playerToClickPoint, false, false);
        }
        else
        {
            m_Character.Move(Vector3.zero, false, false);
        }
    }

    Vector3 ShortDestination(Vector3 destination, float shortening)
    {
        Vector3 reductionVector = (destination - transform.position).normalized * shortening;
        return destination - reductionVector;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, currentDestination);
        Gizmos.DrawSphere(currentDestination, 0.2f);
        //Gizmos.DrawSphere(, 0.2f);

        //add this after the vector changes
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(clickPoint, 0.1f);//.color = Color.blue;

        Gizmos.color = new Color(255f, 0f, 0f, 0.4f);
        //Gizmos.DrawSphere(transform.position, 2f);

        Gizmos.DrawWireSphere(transform.position, 2f);
    }
}

