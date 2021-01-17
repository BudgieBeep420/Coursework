using UnityEngine;

public class RiflePuttingAwayScript : StateMachineBehaviour
{
    /* This makes sure you cannot start another action while already doing one with the rifle */
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject.Find("Rifle Holder").GetComponentInChildren<RifleBehaviourScript>().PuttingAway();
    }
}
