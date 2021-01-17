using UnityEngine;

public class KnifePuttingAway : StateMachineBehaviour
{
    
    /* This essentially makes sure that you cannot use the knife while already in an animation */
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject.Find("KnifeHolder").GetComponentInChildren<KnifeBehaviour>().CantStab();
    }
}
