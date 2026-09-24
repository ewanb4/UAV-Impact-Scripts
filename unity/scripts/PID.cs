using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class PID
{
    public float Kp, Ki, Kd;  // PID coefficients
    private float previousError = 0;
    private float integral = 0;

    public PID(float kp, float ki, float kd)
    {
        Kp = kp;
        Ki = ki;
        Kd = kd;
    }

    public float Calculate(float setpoint, float current, float deltaTime)
    {
        float error = setpoint - current;
        integral += error * deltaTime;
        float derivative = (error - previousError) / deltaTime;
        previousError = error;
        
        return (Kp * error) + (Ki * integral) + (Kd * derivative);
    }
}

