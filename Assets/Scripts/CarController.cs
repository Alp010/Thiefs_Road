using System;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private CarStats carStats;
    private GameTouchManager gameTouchManager;

    enum AxelType
    {
        Front,
        Rear
    }

    [Serializable]
    struct Wheel
    {
        public Transform wheelModel;
        public WheelCollider wheelCollider;
        public AxelType axelType;
    }

    [SerializeField] private Wheel[] wheels;

    private Rigidbody carRb;
    private float maxSteerAngle = 30f;
    private float currentSteerAngle;
    private bool isBraking = false;
    private int horizontalInput = 0;

    private void Start()
    {
        carStats = FindAnyObjectByType<CarStats>();
        gameTouchManager = FindAnyObjectByType<GameTouchManager>();


        carRb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        GetHorizontalInput();
        
        ApplyThrottle(1);
    }

    private void ApplyThrottle(float amount)
    {
        foreach (var wheel in wheels)
        {
            if (wheel.axelType == AxelType.Rear)
            {
                wheel.wheelCollider.motorTorque = amount * carStats.maxTorque;
            }
        }
    }

    private void GetHorizontalInput()
    {
        if (gameTouchManager.CurrentTouchDirection == GameTouchManager.TouchDir.Left)
        {
            horizontalInput = -1;
        }
        else if (gameTouchManager.CurrentTouchDirection == GameTouchManager.TouchDir.Right)
        {
            horizontalInput = 1;
        }
        else if (gameTouchManager.CurrentTouchDirection == GameTouchManager.TouchDir.None)
        {
            horizontalInput = 0;
        }

#if UNITY_EDITOR
        Debug.Log(horizontalInput);
#endif
    }
}