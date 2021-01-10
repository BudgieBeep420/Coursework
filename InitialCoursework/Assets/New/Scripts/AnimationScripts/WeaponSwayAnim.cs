using UnityEngine;

public class WeaponSwayAnim : StateMachineBehaviour
{
    private static readonly int HasShot = Animator.StringToHash("HasShot");
    
    private float _normalizedLeavingPoint;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        /*
        if (Math.Abs(_normalizedLeavingPoint) > 0.1f)
        {
            animator.Play("Weaponsway", 0, _normalizedLeavingPoint);
            _normalizedLeavingPoint = 0;
        }
        */

        animator.SetBool(HasShot, false);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _normalizedLeavingPoint = stateInfo.normalizedTime;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
