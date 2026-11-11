using UnityEngine;
using UnityEngine.Events;

public class CS_UI_Display : MonoBehaviour
{
    [SerializeField] LayerMask raycastMask = ~0;
    [SerializeField] float raycastDistance = 5.0f;
    [SerializeField] UnityEvent<Vector2> OnCursorInput = new(); 

    void Update()
    {
#if ENABLE_LEGACY_INPUT_MANAGER
        Vector3 MousePos = Input.mousePosition;
#else
        Vector3 MousePosition = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
#endif

        //construct ray from mouse pos
        Ray MouseRay = Camera.main.ScreenPointToRay(MousePos);

        RaycastHit HitResult;
        if(Physics.Raycast(MouseRay, out HitResult, raycastDistance, raycastMask, QueryTriggerInteraction.Ignore)) 
        {
            if (HitResult.collider.gameObject != null) 
            {
                OnCursorInput.Invoke(HitResult.textureCoord);
            }
        }
    }
}
