using UnityEngine;
using UnityEngine.UI;

public class FadingScript : MonoBehaviour
{
    [SerializeField] private Image blackScreen;

    private const float Time = 3;

    private void OnEnable()
    {
        FadeFromBlack();
    }
    
    public void FadeToBlack ()
    {
        blackScreen.color = Color.black;
        blackScreen.canvasRenderer.SetAlpha(0.0f);
        blackScreen.CrossFadeAlpha (1.0f, Time, false);
    }
     
    private void FadeFromBlack ()
    {
        blackScreen.color = Color.black;
        blackScreen.canvasRenderer.SetAlpha(1.0f);
        blackScreen.CrossFadeAlpha (0.0f, Time, false);
    }
}
