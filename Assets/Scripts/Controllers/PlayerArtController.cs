using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;

public enum AnimationCodeEnum { idle, walk, jump, pick, throwObj, landing};

public class PlayerArtController : MonoBehaviour
{
    private Animator animator;

    public Face faces;
    public GameObject SmileBody; // this should adpat to all individual slimes
    private Material faceMaterial;
    private bool wasIdled;

    private AnimationCodeEnum animationCode = AnimationCodeEnum.idle;

    private AnimatorStateInfo curAnimation;

    public EventReference soundWalk;
    public EventReference soundJump;
    public EventReference soundLanding;
    public EventReference soundPick;
    public EventReference soundThrow;

    void Start()
    {
        animator = GetComponent<Animator>();
        faceMaterial = SmileBody.GetComponent<Renderer>().materials[1];
    }

    void Update()
    {
        PlayAnimation();
        animationCode = AnimationCodeEnum.idle;
        curAnimation = animator.GetCurrentAnimatorStateInfo(0);
    }

    public void setAnimationCode(AnimationCodeEnum aCode)
    {
        switch (aCode)
        {
            case AnimationCodeEnum.walk:
                if (curAnimation.IsName("Jump")) return;
                if (curAnimation.IsName("Attack")) return;
                RuntimeManager.PlayOneShot(soundWalk);
                break;
            case AnimationCodeEnum.jump:
                RuntimeManager.PlayOneShot(soundJump);
                break;
            case AnimationCodeEnum.pick:
                RuntimeManager.PlayOneShot(soundPick);
                break;
            case AnimationCodeEnum.throwObj:
                RuntimeManager.PlayOneShot(soundThrow);
                break;
            case AnimationCodeEnum.landing:
                RuntimeManager.PlayOneShot(soundLanding);
                break;
        }
        animationCode = aCode;
    }


    private void PlayAnimation()
    {
        switch (animationCode)
        {
            case AnimationCodeEnum.walk:
                if (curAnimation.IsName("Jump")) return;
                animator.SetTrigger("Jump");
                wasIdled = false;
                //faceMaterial.SetTexture("_MainTex", (heldObj == null) ? faces.IdleFace: faces.WalkFace);
                break;

            case AnimationCodeEnum.throwObj:
                if (curAnimation.IsName("Attack")) return;
                animator.SetTrigger("Attack");
                wasIdled = false;
                //faceMaterial.SetTexture("_MainTex", faces.AttackFace);
                break;
            default:
                // the idle state info is not included in animator
                if (wasIdled) return;
                animator.ResetTrigger("Jump");
                wasIdled = true;
                //faceMaterial.SetTexture("_MainTex", faces.IdleFace);
                break;
        }
    }

    public void AlertObservers()
    {
        // this is triggered after some animation ends
        // simply have this method to remove errors shown in console
    }
}