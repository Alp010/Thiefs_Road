using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;

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

            // Her karede kontrol: Eğer UI üzerindeyse yönü kapat
            if (IsPointerOverUI(currentTouchPosition))
            {
                touchDirection = TouchDir.None;
            }
            else
            {
                DetectTouchDirection();
            }
        }
        else
        {
            touchDirection = TouchDir.None;
        }
    }

    private void OnTouchStarted(InputAction.CallbackContext context)
    {
        currentTouchPosition = touchPositionAction.ReadValue<Vector2>();

        if (IsPointerOverUI(currentTouchPosition))
        {
            isTouchActive = false;
            return;
        }

        isTouchActive = true;
        startTouchPosition = currentTouchPosition;
    }

    private void OnTouchCanceled(InputAction.CallbackContext context)
    {
        isTouchActive = false;
        touchDirection = TouchDir.None;
    }

    private void UpdateTouchPosition()
    {
        currentTouchPosition = touchPositionAction.ReadValue<Vector2>();
    }

    private void DetectTouchDirection()
    {
        float screenCenterX = Screen.width / 2f;

        if (currentTouchPosition.x < screenCenterX)
            touchDirection = TouchDir.Left;
        else
            touchDirection = TouchDir.Right;
    }

    private bool IsPointerOverUI(Vector2 screenPosition)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = screenPosition;

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            if (result.gameObject.CompareTag("BlockInput"))
            {
                return true; // Engellenen UI'ya basıldı
            }
        }


        return false; // UI'ya basıldı ama önemli değil veya hiç UI'ya basılmadı
    }
}