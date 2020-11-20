using UnityEngine;

public class WeaponReloadAnim : StateMachineBehaviour
{
    private static readonly int IsReloading = Animator.StringToHash("IsReloading");
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(IsReloading, false);
    }
    
    
}
