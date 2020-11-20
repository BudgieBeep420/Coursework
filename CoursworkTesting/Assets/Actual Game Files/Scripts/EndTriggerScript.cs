using UnityEngine;

public class EndTriggerScript : MonoBehaviour
{
    [SerializeField] private FadingScript fadingScript;
    [SerializeField] private LevelManagerScript levelManagerScript;
    [SerializeField] [Range(1,5)] private int level;
    
    private void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("Player")) return;
        
        fadingScript.FadeToBlack();
        
        switch (level)
        {
            case 1:
                levelManagerScript.FinishedM1();
                break;
        }
    }
}
