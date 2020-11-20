using UnityEngine;

public class KnifePuttingAway : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject.Find("KnifeHolder").GetComponentInChildren<KnifeBehaviour>().CantStab();
    }
}
