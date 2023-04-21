using UnityEngine;
using System.Collections;
using FMODUnity;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;

    public Face faces;
    public GameObject SmileBody; // this should adpat to all individual slimes
    private Material faceMaterial;
    private bool wasIdled;

    public animationCode aCode = animationCode.idle;
    public soundCode sCode = soundCode.none;

    public EventReference soundJump;
    public EventReference soundPick;
    public EventReference soundThrow;

    void Start()
    {
        animator = GetComponent<Animator>();
        faceMaterial = SmileBody.GetComponent<Renderer>().materials[1];
    }

    void Update()
    {
        PlaySound();
        PlayAnimation();
    }

    private void PlaySound()
    {
        switch (sCode)
        {
            case soundCode.jump:
                RuntimeManager.PlayOneShot(soundJump);
                sCode = soundCode.none;
                break;
            case soundCode.pick:
                RuntimeManager.PlayOneShot(soundPick);
                sCode = soundCode.none;
                break;
            case soundCode.throwObj:
                RuntimeManager.PlayOneShot(soundThrow);
                sCode = soundCode.none;
                break;
        }
    }


    private void PlayAnimation()
    {
        switch (aCode)
        {
            case animationCode.idle:
                // the idle state info is not included in animator
                if (wasIdled) return;
                wasIdled = true;
                animator.ResetTrigger("Jump");
                //faceMaterial.SetTexture("_MainTex", faces.IdleFace);
                break;

            case animationCode.walk:
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump")) return;
                animator.SetTrigger("Jump");
                wasIdled = false;
                //faceMaterial.SetTexture("_MainTex", (heldObj == null) ? faces.IdleFace: faces.WalkFace);
                break;

            case animationCode.throwObj:
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) return;
                animator.SetTrigger("Attack");
                wasIdled = false;
                //faceMaterial.SetTexture("_MainTex", faces.AttackFace);
                break;
        }
    }


    public void setCodeFromRequest(animationCode aCode, soundCode sCode)
    {
        this.aCode = aCode;
        this.sCode = sCode;
    }
}
