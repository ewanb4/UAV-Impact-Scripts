using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

public class Drone : MonoBehaviour
{
    // Start is called before the first frame update
    public float Speed = 5f;
    private float horizontalMovement;
    private float forwardMovement;
    private float tiltAmountForward = 0;
    private float tiltAmountSideways = 0;
    private float tiltVelocityForward;
    private float tiltVelocitySideways;
    public float upForce;
    private Rigidbody drone;
    public float mass;
    private string filePath;

    void Start()
    {
        drone = GetComponent<Rigidbody>();
        drone.mass = mass;

        // Debug.Log(Application.persistentDataPath);
        // filePath = Path.Combine(Application.persistentDataPath, "collisionData.csv");
        // if (!File.Exists(filePath))
        // {
        //     string header = "Time, Collision With, Impact Force, Relative Velocity, Global Position\n";
        //     File.WriteAllText(filePath, header);
        // }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        movementVertical();
        movementForward();
        drone.AddRelativeForce(Vector3.up * upForce);

        clampingSpeedValues();

        // horizontalMovement = Input.GetAxis("Horizontal");
        // forwardMovement = Input.GetAxis("Vertical");
        // transform.Translate(Vector3.forward * Time.deltaTime * Speed * forwardMovement);
        // transform.Translate(Vector3.right * Time.deltaTime * Speed * horizontalMovement);

        tilting();
        rotation();
    }

    void movementVertical(){

        if((Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f || Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)){
            if(Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.LeftShift)){
                drone.velocity = drone.velocity;
            }
            if(!Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)){
                drone.velocity = new Vector3(drone.velocity.x, Mathf.Lerp(drone.velocity.y, 0, Time.deltaTime * 5), drone.velocity.z);
                upForce = 281;
            }
            if(!Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.A) || !Input.GetKey(KeyCode.D)){
                drone.velocity = new Vector3(drone.velocity.x, Mathf.Lerp(drone.velocity.y, 0, Time.deltaTime * 5), drone.velocity.z);
                upForce = 185;
            }
            // if(Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.L)){
            //     upForce = 410;
            // }
        }

        if(Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f){
            upForce = 185;
        }
        
        if (Input.GetKey(KeyCode.Space))
        {
            upForce = (mass * 10) + 450;
            if(Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f){
                upForce = 500;
            }
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            upForce = (mass * 10) - 200;
        }
        else if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.Space) && (Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f))
        {
            upForce = Convert.ToSingle(drone.mass) * 9.81f;
        }
    }

    private float movementForwardSpeed = 250.0f;
    void movementForward(){
        if(Input.GetAxis("Vertical") != 0){
            drone.AddRelativeForce(Vector3.forward * Input.GetAxis("Vertical") * movementForwardSpeed);
        }
        if(Input.GetAxis("Horizontal") != 0){
            drone.AddRelativeForce(Vector3.right * Input.GetAxis("Horizontal") * movementForwardSpeed);
        }
    }

    void tilting(){
        tiltAmountForward = Mathf.SmoothDamp(tiltAmountForward, 20 * Input.GetAxis("Vertical"), ref tiltVelocityForward, 0.1f);
        tiltAmountSideways = Mathf.SmoothDamp(tiltAmountSideways, 20 * Input.GetAxis("Horizontal"), ref tiltVelocitySideways, 0.1f);

        drone.rotation = Quaternion.Euler(
            new Vector3(tiltAmountForward, currentYRotation, -tiltAmountSideways)
        );

    }

    private float wantedYRotation;
    public float currentYRotation;
    private float rotateAmountByKeys = 1.0f;
    private float rotationYVelocity;

    void rotation(){
        if(Input.GetKey(KeyCode.J)){
            wantedYRotation -= rotateAmountByKeys;
        }
        if(Input.GetKey(KeyCode.L)){
            wantedYRotation += rotateAmountByKeys;
    }
    currentYRotation = Mathf.SmoothDamp(currentYRotation, wantedYRotation, ref rotationYVelocity, 0.25f);
    }

    private Vector3 velocityToSmoothDamp;
    void clampingSpeedValues(){
        if(Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f){
            drone.velocity = Vector3.ClampMagnitude(drone.velocity, Mathf.Lerp(drone.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
        }
        if(Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f){
            drone.velocity = Vector3.ClampMagnitude(drone.velocity, Mathf.Lerp(drone.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
        }
        if(Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f){
            drone.velocity = Vector3.ClampMagnitude(drone.velocity, Mathf.Lerp(drone.velocity.magnitude, 5.0f, Time.deltaTime * 5f));
        }
        if(Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Math.Abs(Input.GetAxis("Horizontal")) < 0.2f){
            drone.velocity = Vector3.SmoothDamp(drone.velocity, Vector3.zero, ref velocityToSmoothDamp, 0.95f);
        }
    }   

    // void OnCollisionEnter(Collision collision)
    // {   
    //     if (collision.gameObject.tag == "Collision")
    //     {

    //         // Debug.Log("Collision from: " + collision.gameObject.name);
    //         string timeStamp = Time.time.ToString("F2");
    //         float collisionForce = collision.impulse.magnitude;
    //         float collisionVelocity = collision.relativeVelocity.magnitude;
    //         Vector3 worldPos = collision.contacts[0].point;

    //         string data = $"{timeStamp}, {collision.gameObject.name}, {collisionForce}, {collisionVelocity}, {worldPos[1]}\n";
    //         File.AppendAllText(filePath, data);


    //         Debug.Log(collision.gameObject.name + " Impact Force: " + collisionForce + " Impact Velocity: " + collisionVelocity + " Position: " + worldPos);
    //     }
    // }
}

