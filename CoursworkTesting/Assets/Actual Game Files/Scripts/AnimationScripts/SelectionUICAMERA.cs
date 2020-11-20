using UnityEngine;

public class SelectionUICAMERA : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    
    public void EnableUI()
    {
        canvas.SetActive(true);
    }
}
