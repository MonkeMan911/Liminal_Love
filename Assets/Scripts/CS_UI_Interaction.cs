using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CS_UI_Interaction : MonoBehaviour
{
    [SerializeField] RectTransform canvasTransform;
    [SerializeField] GraphicRaycaster raycaster;
    List<GameObject> dragTargets = new();

    public void OnCursorInput(Vector2 InNormalisedPosition) 
    {
        //Get mouse pos in canvas space
        Vector3 InputPos = new Vector3(canvasTransform.sizeDelta.x * InNormalisedPosition.x, canvasTransform.sizeDelta.y * InNormalisedPosition.y, 0);

        //build a pointer event
        PointerEventData PointerEvent = new PointerEventData(EventSystem.current);
        PointerEvent.position = InputPos;

        //Determine whats hit
        List<RaycastResult> results = new();
        raycaster.Raycast(PointerEvent, results);
#if ENABLE_LEGACY_INPUT_MANAGER
        bool bMouseDownThisFrame = Input.GetMouseButtonDown(0);
        bool bMouseUpThisFrame = Input.GetMouseButtonUp(0);
        bool bIsMouseDown = Input.GetMouseButton(0);
#else
        bool bMouseDownThisFrame = UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame
        bool bMouseUpThisFrame = UnityEngine.InputSystem.Mouse.current.leftButton.wasReleasedThisFrame
        bool bIsMouseDown = UnityEngine.InputSystem.Mouse.current.leftButton.isPressed
#endif
        //has mouse button gone up this frame
        if (bMouseDownThisFrame) 
        {
            foreach (var Target in dragTargets) 
            {
                if (ExecuteEvents.Execute(Target, PointerEvent, ExecuteEvents.endDragHandler))
                    break;
            }
            dragTargets.Clear();
        }

        foreach (var Result in results) 
        {
            //Create a new event data
            PointerEventData PointerEventForResult = new PointerEventData(EventSystem.current);
            PointerEventForResult.position = InputPos;
            PointerEventForResult.pointerCurrentRaycast = Result;
            PointerEventForResult.pointerPressRaycast = Result;

            //is the button down
            if (bIsMouseDown)
                PointerEventForResult.button = PointerEventData.InputButton.Left;

            //Retrive slider if hit
            var HitSlider = Result.gameObject.GetComponentInParent<Slider>();

            if (bMouseDownThisFrame) 
            {
                if (ExecuteEvents.Execute(Result.gameObject, PointerEventForResult, ExecuteEvents.beginDragHandler))
                    dragTargets.Add(Result.gameObject);

                if (HitSlider != null) 
                {
                    HitSlider.OnInitializePotentialDrag(PointerEventForResult);
                    if (dragTargets.Contains(Result.gameObject))
                        dragTargets.Add(Result.gameObject);
                }
            }
            else if (dragTargets.Contains(Result.gameObject)) 
            {
                PointerEventForResult.dragging = true;
                ExecuteEvents.Execute(Result.gameObject, PointerEventForResult, ExecuteEvents.dragHandler);

                if (HitSlider != null)
                    HitSlider.OnDrag(PointerEventForResult);
            }

            if (bMouseDownThisFrame) 
            {
                if (!ExecuteEvents.Execute(Result.gameObject, PointerEventForResult, ExecuteEvents.pointerDownHandler))
                    break;
            }
            else if (bMouseUpThisFrame)
            {
                bool bDidRun = false;
                bDidRun |= ExecuteEvents.Execute(Result.gameObject, PointerEventForResult, ExecuteEvents.pointerUpHandler);
                bDidRun |= ExecuteEvents.Execute(Result.gameObject, PointerEventForResult, ExecuteEvents.pointerClickHandler);

                if (bDidRun)
                    break;
            }
        }
    }
}
