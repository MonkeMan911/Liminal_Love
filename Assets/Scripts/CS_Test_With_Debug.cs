using UnityEngine;

public class CS_Test_With_Debug : MonoBehaviour
{
    [SerializeField] string message;
    [SerializeField] private GameObject gObject;
    public void MessageConsole() 
    {
        Debug.Log(message);
    }

    public void SwitchImage() 
    {
        gObject.SetActive(false);
    }
}
