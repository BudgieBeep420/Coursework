using UnityEngine;

public class RiflePuttingAwayScript : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject.Find("Rifle Holder").GetComponentInChildren<RifleBehaviourScript>().PuttingAway();
    }
}
