using UnityEngine;
using UnityEngine.EventSystems;

public class CS_Draggable_Window : MonoBehaviour
{
    private Vector3 mouseDragStartPos;
    private Vector3 mouseDragEndPos;
    public bool isDragging;
    [SerializeField] GameObject dragObject;

    private void Start()
    {
        isDragging= false;
    }
    public void DragWindow() 
    {
        if (isDragging)
            dragObject.transform.localPosition = Input.mousePosition - mouseDragStartPos;
    }

    public void StartDrag() 
    {
        mouseDragStartPos = Input.mousePosition - dragObject.transform.localPosition;
    }

    public void EndDrag() 
    {
        Debug.Log(transform.localPosition);
        mouseDragEndPos = dragObject.transform.localPosition;
        dragObject.transform.localPosition = mouseDragEndPos;
        Debug.Log(transform.localPosition);
    }

    public void isDraggingTrue() 
    {
        isDragging = true;
    }
    public void isDraggingFalse() 
    {
        isDragging = false;
    }
}