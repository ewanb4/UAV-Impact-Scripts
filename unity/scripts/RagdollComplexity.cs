using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

public class RagdollComplexity : MonoBehaviour
{

    private Rigidbody rb;
    private string filePath;
    private string folderPath;
    private string accellerationFilePath;
    private Vector3 acceleration = new Vector3 (0,0,0);
    private Vector3 angularAcceleration = new Vector3 (0,0,0);
    private Vector3 lastVelocity = new Vector3 (0,0,0);
    private Vector3 lastAngularVelocity = new Vector3 (0,0,0);
    private Vector3 lastAcceleration = new Vector3 (0,0,0);
    private Vector3 lastAngularAcceleration = new Vector3 (0,0,0);
    private Vector3 Jerk = new Vector3 (0,0,0);
    private Vector3 angularJerk = new Vector3 (0,0,0);
    public Transform point;

    // Start is called before the first frame update
    void Start()
    {

        Debug.Log(Application.persistentDataPath);
        folderPath = Path.Combine(Application.persistentDataPath, gameObject.tag);
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        filePath = Path.Combine(folderPath, $"Rigidbody-CollisionData - {gameObject.name}.csv");
        File.Delete(filePath);

        if (!File.Exists(filePath))
        {
            string header = "Time, Body Part Collided with: , Impact Force (N), Relative Velocity (m/s)\n";
            File.WriteAllText(filePath, header);
        }

        accellerationFilePath = Path.Combine(folderPath, $"Rigidbody-accelerationData-{gameObject.name}.csv");
        File.Delete(accellerationFilePath);

        if (!File.Exists(accellerationFilePath))
        {
            string header = "Time, Body Part Collided with: , Body Part Angular Acceleration (rad/s^2), Body Part Angular Jerk (rad/s^3), Angular Velocity (rad/s), Acceleration (m/s^2), Jerk (m/s^3), Velocity (m/s) \n";
            File.WriteAllText(accellerationFilePath, header);
        }

        else{
            File.AppendAllText(accellerationFilePath,"\n");
        }

        rb = GetComponent<Rigidbody>();
        lastAngularVelocity = rb.angularVelocity;
        lastVelocity = rb.velocity;
        // Debug.Log(lastVelocity);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        angularAcceleration = (rb.angularVelocity - lastAngularVelocity) / Time.fixedDeltaTime;
        //Debug.Log("Accel: " + angularAcceleration);
        lastAngularVelocity = rb.angularVelocity;

        angularJerk = (angularAcceleration - lastAngularAcceleration) / Time.fixedDeltaTime;
        lastAngularAcceleration = angularAcceleration;

        acceleration = (rb.velocity - lastVelocity) / Time.fixedDeltaTime;
        lastVelocity = rb.velocity;

        Jerk = (acceleration - lastAcceleration) / Time.fixedDeltaTime;
        lastAcceleration = acceleration;

        //Debug.Log("Velocity: " + lastVelocity);

        //Debug.Log(gameObject.name + " Acceleration: " + acceleration);
        float RotationX, RotationY, RotationZ;

        if(transform.eulerAngles.x <= 180f)
        {
            RotationX = transform.eulerAngles.x;
        }
        else
        {
            RotationX = transform.eulerAngles.x - 360f;
        }

        // if(transform.eulerAngles.y <= 180f)
        // {
        //     RotationY = transform.eulerAngles.y;
        // }
        // else
        // {
        //     RotationY = transform.eulerAngles.y - 360f;
        // }

        RotationY = transform.eulerAngles.y;

        if(transform.eulerAngles.z <= 180f)
        {
            RotationZ = transform.eulerAngles.z;
        }
        else
        {
            RotationZ = transform.eulerAngles.z - 360f;
        }
        
        //RotationY = RotationY - 90;
        Debug.Log("Rotation Y: " + RotationY);
        Debug.Log("Rotation X: " + RotationX);
        Debug.Log("Rotation Z: " + RotationZ);

        string time = Time.time.ToString("F2");
        string Aceldata = $"{time}, {gameObject.name}, {angularAcceleration}, {angularJerk}, {rb.angularVelocity}, {acceleration}, {Jerk}, {rb.velocity}, {RotationX}, {RotationY-90}, {RotationZ} \n";
        File.AppendAllText(accellerationFilePath, Aceldata);
    }

    private Vector3 distanceToImpactCoords;
    private float distanceToImpact;

    void OnCollisionEnter(Collision collision)
    {   
        if (collision.gameObject.tag == "CollisionDrone")
        {

            // Debug.Log("Local Anchor: " + localAnchor);
            // Debug.Log("World Anchor: " + worldAnchor);

            // Debug.Log("Collision from: " + collision.gameObject.name);
            // Debug.Log(gameObject.name);
            string timeStamp = Time.time.ToString("F2");
            Vector3 worldPos = collision.contacts[0].point;
            float collisionForce = collision.impulse.magnitude;
            float collisionVelocity = collision.relativeVelocity.magnitude;
            // Debug.Log("Joint Pos: " + worldAnchor[0]);
            // Debug.Log("Collision Pos: " + worldPos[0]);
            string data = $"{timeStamp}, {gameObject.name}, {collisionForce}, {collisionVelocity}\n";
            File.AppendAllText(filePath, data);

            // Debug.Log("Body Part: " + gameObject.name + " Impact Force: " + collisionForce + "N" + " Impact Velocity: " + collisionVelocity + "m/s");
            // Debug.Log("Collision Point " + worldPos);
            // Debug.Log("Distance from Anchor" + distanceToImpact);
            
        }
    }
}
