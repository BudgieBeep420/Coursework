using System.Collections;
using System.Collections.Generic;
using Actual_Game_Files.Scripts;
using UnityEngine;

public class WeaponReloadAnim : StateMachineBehaviour
{
    private static readonly int IsReloading = Animator.StringToHash("IsReloading");
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(IsReloading, false);
    }
    
    
}
