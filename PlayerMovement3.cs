using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement3 : MonoBehaviour
{
    [Header("Inertia")]
    public float maxInertia;
    public float inertiaSpeedFactor = 10;
    public float inertiaDecceleration;
    public float currentInertiaDeccel;
    public Vector2 inertiaVector;
    private bool isSlipping = false;

    [Header("Desired Movement")]
    public float maxSpeed;
    public float speedFactor = 10;
    public float decceleration;
    public Vector2 desiredVector;
    public AudioSource startMoveSFX;

    [Header("Turn")]
    public float turnAccel;
    public float turnMaxSpeed;
    public float turnDeccel;
    public float turnStartSpeed = 0.1f;
    public float currentTurnSpeed;

    [Header("Input")]
    public string VerticalAxis1 = "";
    public string HorizontalAxis1 = "";
    public string VerticalAxis2 = "Vertical";
    public string HorizontalAxis2 = "Horizontal";
    public Vector2 mainJoystick;
    public Vector2 secondJoystick;
    public float secondStickMagnitude;
    public float closeEnough = 0.01f;

    public Vector2 maxPos;
    public Vector2 minPos;

    // Start is called before the first frame update
    void Start()
    {
        currentInertiaDeccel = inertiaDecceleration;
    }

    void Update()
    {
        if (!isSlipping)
            currentInertiaDeccel = inertiaDecceleration;
    }


    // Update is called once per frame
    void LateUpdate()
    {
        mainJoystick = new Vector2(Input.GetAxis(HorizontalAxis1), Input.GetAxis(VerticalAxis1));

        secondJoystick = new Vector2(Input.GetAxis(HorizontalAxis2), Input.GetAxis(VerticalAxis2));
        secondStickMagnitude = secondJoystick.magnitude;
        secondJoystick = secondJoystick.normalized;

        setTurnSpeed();

        move();
    }
    

    private void setTurnSpeed()
    {
        // Calculate target speed
        if (secondJoystick == Vector2.zero && currentTurnSpeed != 0)  // no input but hasn't stopped yet
        {
            currentTurnSpeed = Mathf.Lerp(currentTurnSpeed, 0, turnDeccel * Time.deltaTime);
            if (currentTurnSpeed < closeEnough)
                currentTurnSpeed = 0;
        }
        else if (currentTurnSpeed < turnMaxSpeed)   // stick has input and not yet at max speed
        {
            if (currentTurnSpeed <= turnStartSpeed)
                currentTurnSpeed = turnStartSpeed;

            currentTurnSpeed = currentTurnSpeed * turnAccel * secondStickMagnitude;//Mathf.Lerp(currentTurnSpeed, turnMaxSpeed, turnAccel * joystickMagnitude);
            if (currentTurnSpeed > turnMaxSpeed - closeEnough)
                currentTurnSpeed = turnMaxSpeed;
        }
        if (secondJoystick != Vector2.zero)
        {
            float angle = Mathf.Atan2(secondJoystick.y, secondJoystick.x) * Mathf.Rad2Deg;
            Quaternion rotationQuat = Quaternion.AngleAxis(angle, Vector3.forward);
            this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, rotationQuat, currentTurnSpeed * Time.deltaTime);
        }
        
    }

    private void move()
    {
        // Calculate inertia

        if ((mainJoystick == Vector2.zero && inertiaVector.magnitude != 0) || isSlipping) // no input but hasn't stopped yet
        {
            inertiaVector = Vector2.ClampMagnitude(inertiaVector, Mathf.Lerp(inertiaVector.magnitude, 0, currentInertiaDeccel * Time.deltaTime));  // decreases magnitude of movement vector to 0
            if (inertiaVector.magnitude < closeEnough)
            {
                inertiaVector = Vector2.zero;
                isSlipping = false;
            }
        }
        else   // modify vector by current input
        {
            inertiaVector += mainJoystick * inertiaSpeedFactor * Time.deltaTime;
            if (inertiaVector.magnitude > (maxInertia - closeEnough) * Time.deltaTime)
                inertiaVector = Vector2.ClampMagnitude(inertiaVector, maxInertia * Time.deltaTime);  // decreases magnitude of movement vector to 0
        }

        // Calculate the movement in the desired direction

        if ((mainJoystick == Vector2.zero && desiredVector.magnitude != 0) || isSlipping)  // no input but hasn't stopped yet
        {
            desiredVector = Vector2.ClampMagnitude(desiredVector, Mathf.Lerp(desiredVector.magnitude, 0, decceleration * Time.deltaTime));  // decreases magnitude of movement vector to 0
            if (desiredVector.magnitude < closeEnough * Time.deltaTime)
                desiredVector = Vector2.zero;
        }
        else   // modify vector by current input
        {
            if (desiredVector.magnitude < closeEnough && mainJoystick != Vector2.zero)
                startMoveSFX.Play();
                
            desiredVector += mainJoystick * speedFactor * Time.deltaTime;
            if (desiredVector.magnitude > (maxSpeed - closeEnough) * Time.deltaTime)
                desiredVector = Vector2.ClampMagnitude(desiredVector, maxSpeed * Time.deltaTime);  // decreases magnitude of movement vector to 0
        }

        // ensure not going out of bounds
        Vector3 pos = this.transform.position + new Vector3(inertiaVector.x + desiredVector.x, inertiaVector.y + desiredVector.y, 0);
        if (pos.x > maxPos.x)
            pos.x = maxPos.x;
        if (pos.y > maxPos.y)
            pos.y = maxPos.y;
        if (pos.x < minPos.x)
            pos.x = minPos.x;
        if (pos.y < minPos.y)
            pos.y = minPos.y;
        this.transform.position = pos;

    }

    public void addForce(Vector2 force, float slipFactor)
    {
        inertiaVector += force;
        isSlipping = true;
        currentInertiaDeccel = inertiaDecceleration * slipFactor;
    }

    public void reset()
    {
        currentInertiaDeccel = inertiaDecceleration;
        inertiaVector = Vector2.zero;
        isSlipping = false;
        desiredVector = Vector2.zero;
        currentTurnSpeed = 0;
    }

    //public IEnumerator temporarySlipperyness(int time, float slipFactor)
    //{
    //    slipCount++;
    //    currentInertiaDeccel = inertiaDecceleration * slipFactor;
    //    yield return new WaitForSeconds(time);
    //    slipCount--;
    //}
}
