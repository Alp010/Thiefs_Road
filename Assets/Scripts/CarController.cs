using System;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Wheel Settings")]
    [SerializeField] private Wheel[] wheels;
    [SerializeField] private float maxSteerAngle = 30f;

    [SerializeField] private CarStats carStats;
    private GameTouchManager gameTouchManager;
    private Rigidbody carRb;

    private float horizontalInput = 0f;

    private enum AxelType { Front, Rear }

    [Serializable]
    private struct Wheel
    {
        public Transform wheelModel;
        public WheelCollider wheelCollider;
        public AxelType axelType;
    }

    private void Start()
    {
        gameTouchManager = FindAnyObjectByType<GameTouchManager>();
        carRb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        HandleInput();
        HandleSteering();
        ApplyThrottle();
        UpdateWheelVisuals();
    }

    private void HandleInput()
    {
        switch (gameTouchManager.CurrentTouchDirection)
        {
            case GameTouchManager.TouchDir.Left:
                horizontalInput = -1f;
                break;
            case GameTouchManager.TouchDir.Right:
                horizontalInput = 1f;
                break;
            default:
                horizontalInput = 0f;
                break;
        }

#if UNITY_EDITOR
        Debug.Log($"Horizontal Input: {horizontalInput}");
#endif
    }

    private void HandleSteering()
    {
        float steerAngle = maxSteerAngle * horizontalInput;

        foreach (var wheel in wheels)
        {
            if (wheel.axelType == AxelType.Front)
            {
                wheel.wheelCollider.steerAngle = steerAngle;
            }
        }
    }

    private void ApplyThrottle()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.axelType == AxelType.Rear)
            {
                wheel.wheelCollider.motorTorque = -carStats.maxTorque;
            }
        }
    }

    private void UpdateWheelVisuals()
    {
        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.GetWorldPose(out Vector3 pos, out Quaternion rot);
            wheel.wheelModel.position = pos;
            wheel.wheelModel.rotation = rot;
        }
    }
}