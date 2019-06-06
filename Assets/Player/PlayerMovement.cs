using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;

//[RequireComponent(typeof (ThirdPersonCharacter))]
//[RequireComponent(typeof(AICharacterControl))]
[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMovement : MonoBehaviour
{
    #region OLD STUFF

    #endregion

    #region NEW STUFF
    [SerializeField] Transform target;
    Ray lastRay;
    NavMeshAgent mover;
    public bool frozen;

    void Start()
    {
        mover = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        lastRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(lastRay.origin, lastRay.direction * 100, Color.magenta); //this might help when debugging Arik's movement. :(  
        UpdateAnimator();
    }

    public void MoveTo(Vector3 destination)
    {
        mover.destination = destination;
        mover.isStopped = false;
        frozen = false;
    }


    public void Stop()
    {
        frozen = true;
        mover.isStopped = true;
    }
    private void UpdateAnimator()
    {
        Vector3 velocity = mover.velocity;
        //InverseTransformDirection transforms a direction from world space to local space
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;
        GetComponent<Animator>().SetFloat("ForwardSpeed", speed);
        //ForwardSpeed is what we named the Blend on the Blend Tree in the animator
    }


    #endregion
}

