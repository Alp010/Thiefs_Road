using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class GameTouchManager : MonoBehaviour
{
    public enum TouchDir
    {
        Left,
        Right,
        None
    }

    private GameTouchControls touchControls;
    private InputAction touchPressAction;
    private InputAction touchPositionAction;

    private bool isTouchActive = false;
    private Vector2 startTouchPosition;
    private Vector2 currentTouchPosition;

    private TouchDir touchDirection = TouchDir.None;
    public TouchDir CurrentTouchDirection => touchDirection;

    private void Awake()
    {
        touchControls = new GameTouchControls();
        touchPressAction = touchControls.Touch.TouchPress;
        touchPositionAction = touchControls.Touch.TouchPosition;
    }

    private void OnEnable()
    {
        touchControls.Touch.Enable();
        touchPressAction.started += OnTouchStarted;
        touchPressAction.canceled += OnTouchCanceled;
    }

    private void OnDisable()
    {
        touchPressAction.started -= OnTouchStarted;
        touchPressAction.canceled -= OnTouchCanceled;
        touchControls.Touch.Disable();
    }

    private void Update()
    {
        if (isTouchActive)
        {
            UpdateTouchPosition();
            DetectTouchDirection();
        }
        else
        {
            touchDirection = TouchDir.None;
        }
    }

    private void OnTouchStarted(InputAction.CallbackContext context)
    {
        if (IsPointerOverUI())
            return;

        isTouchActive = true;
        startTouchPosition = touchPositionAction.ReadValue<Vector2>();
        currentTouchPosition = startTouchPosition;
        Debug.Log("Touch Started at: " + startTouchPosition);
    }

    private void OnTouchCanceled(InputAction.CallbackContext context)
    {
        isTouchActive = false;
        touchDirection = TouchDir.None;
    }

    private void UpdateTouchPosition()
    {
        currentTouchPosition = touchPositionAction.ReadValue<Vector2>();

        if (currentTouchPosition != startTouchPosition)
        {
            Debug.Log("Dokunma anlık pozisyonu" + currentTouchPosition);
        }
    }

    private void DetectTouchDirection()
    {
        float screenCenterX = Screen.width / 2f;

        if (currentTouchPosition.x < screenCenterX)
            touchDirection = TouchDir.Left;
        else
            touchDirection = TouchDir.Right;
    }

    private bool IsPointerOverUI()
    {
#if UNITY_EDITOR
        return EventSystem.current.IsPointerOverGameObject();
#else
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            return EventSystem.current.IsPointerOverGameObject(Touchscreen.current.primaryTouch.touchId.ReadValue());
        }
        return false;
#endif
    }
}