using UnityEngine;

public class RifleShootingAnim : StateMachineBehaviour
{
    private static readonly int HasShot = Animator.StringToHash("HasShot");
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(HasShot, false);
    }
}

