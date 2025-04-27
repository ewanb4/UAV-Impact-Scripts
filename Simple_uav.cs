using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simple_uav : MonoBehaviour
{
    Rigidbody rb;
    public Transform[] motors; // Assign 4 motor positions
    float upAxis, forwardAxis, horizontalAxis;
    float forwardAngle = 0, horizontalAngle = 0;
    public float acceleration ;
    float angle = 25;
    bool grounded = false;
    private PID pidAltitude;
    private float targetPitch = 0f;
    public float thrust = 10f;
    
    public float targetAltitude;
    bool verticalBlock = false;
    private float cooldownTime = 0f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pidAltitude = new PID(3.0f, 0.1f, 1.5f);

    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        Controls();
        changeAlt();
        float dt = Time.fixedDeltaTime;
        rb.AddRelativeForce(horizontalAxis * rb.mass,0,forwardAxis * rb.mass);
        transform.localEulerAngles = Vector3.back * horizontalAngle + Vector3.right * forwardAngle;
        float altitudeCorrection = pidAltitude.Calculate(targetAltitude, transform.position.y, dt);
        thrust = Mathf.Clamp(altitudeCorrection, 0, 20f); // Prevent too much thrust

        ApplyMotorForces();

    }

    void Controls()
    {
        if (Input.GetKey(KeyCode.W))
        {
            forwardAngle = Mathf.Lerp(forwardAngle, angle, Time.fixedDeltaTime);
            forwardAxis = acceleration;
        }

        else if (Input.GetKey(KeyCode.S))
        {
            forwardAngle = Mathf.Lerp(forwardAngle, -angle, Time.fixedDeltaTime);
            forwardAxis = -acceleration;
        }
        else
        {
            forwardAngle = Mathf.Lerp(forwardAngle, 0, Time.deltaTime);
            forwardAxis = 0;
        }

        if (Input.GetKey(KeyCode.D))
        {
            horizontalAngle = Mathf.Lerp(horizontalAngle, angle, Time.deltaTime);
            horizontalAxis = acceleration;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            horizontalAngle = Mathf.Lerp(horizontalAngle, -angle, Time.deltaTime);
            horizontalAxis = -acceleration;
        }
        else
        {
            horizontalAngle = Mathf.Lerp(horizontalAngle, 0, Time.deltaTime);
            horizontalAxis = 0;
        }

        if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
        {
            forwardAngle = Mathf.Lerp(forwardAngle, angle, Time.deltaTime);
            horizontalAngle  = Mathf.Lerp(horizontalAngle, angle, Time.deltaTime);
            forwardAxis = 0.5f * acceleration;
            horizontalAxis = 0.5f *acceleration;
        }
        if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
        {
            forwardAngle = Mathf.Lerp(forwardAngle, angle, Time.deltaTime);
            horizontalAngle  = Mathf.Lerp(horizontalAngle, -angle, Time.deltaTime);
            forwardAxis = 0.5f * acceleration;
            horizontalAxis = -0.5f *acceleration;
        }
                if(Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            forwardAngle = Mathf.Lerp(forwardAngle, -angle, Time.deltaTime);
            horizontalAngle  = Mathf.Lerp(horizontalAngle, angle, Time.deltaTime);
            forwardAxis = -0.5f * acceleration;
            horizontalAxis = 0.5f *acceleration;
        }
                if(Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
        {
            forwardAngle = Mathf.Lerp(forwardAngle, -angle, Time.deltaTime);
            horizontalAngle  = Mathf.Lerp(horizontalAngle, -angle, Time.deltaTime);
            forwardAxis = -0.5f * acceleration;
            horizontalAxis = -0.5f * acceleration;
        }
    }

    void ApplyMotorForces()
    {
        float[] motorForces = new float[4];

        motorForces[0] = thrust; //+ pitch - roll + yaw;  // Front-left
        motorForces[1] = thrust; //+ pitch + roll - yaw;  // Front-right
        motorForces[2] = thrust; //- pitch + roll + yaw;  // Back-left
        motorForces[3] = thrust; //- pitch - roll - yaw;  // Back-right

        for (int i = 0; i < 4; i++)
        {
            rb.AddForceAtPosition(transform.up * motorForces[i], motors[i].position, ForceMode.Force);
        }
    }

    void changeAlt()
    {

        if (Time.time>cooldownTime+0.2)
        {
            verticalBlock = false;
            cooldownTime=Time.time;
        }
        
        if (Input.GetKey(KeyCode.I) && !Input.GetKey(KeyCode.K) && verticalBlock == false)
        {
            targetAltitude += 1;
            verticalBlock = true;
        }

        if (Input.GetKey(KeyCode.K) && !Input.GetKey(KeyCode.I) && verticalBlock == false)
        {
            targetAltitude -= 1;
            verticalBlock = true;
        }

        else
        {
            //verticalBlock = false;
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
        grounded = true;
    }

}
