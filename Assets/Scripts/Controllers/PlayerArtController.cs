using UnityEngine;
using System.Collections;
using FMODUnity;

public enum SoundCodeEnum { none, jump, pick, throwObj };
public enum AnimationCodeEnum { idle, walk, throwObj };

public class PlayerArtController : MonoBehaviour
{
    private Animator animator;

    public Face faces;
    public GameObject SmileBody; // this should adpat to all individual slimes
    private Material faceMaterial;
    private bool wasIdled;

    public AnimationCodeEnum animationCode = AnimationCodeEnum.idle;
    public SoundCodeEnum soundCode = SoundCodeEnum.none;

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
        switch (soundCode)
        {
            case SoundCodeEnum.jump:
                RuntimeManager.PlayOneShot(soundJump);
                soundCode = SoundCodeEnum.none;
                break;
            case SoundCodeEnum.pick:
                RuntimeManager.PlayOneShot(soundPick);
                soundCode = SoundCodeEnum.none;
                break;
            case SoundCodeEnum.throwObj:
                RuntimeManager.PlayOneShot(soundThrow);
                soundCode = SoundCodeEnum.none;
                break;
        }
    }


    private void PlayAnimation()
    {
        switch (animationCode)
        {
            case AnimationCodeEnum.idle:
                // the idle state info is not included in animator
                if (wasIdled) return;
                wasIdled = true;
                animator.ResetTrigger("Jump");
                //faceMaterial.SetTexture("_MainTex", faces.IdleFace);
                break;

            case AnimationCodeEnum.walk:
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump")) return;
                animator.SetTrigger("Jump");
                wasIdled = false;
                //faceMaterial.SetTexture("_MainTex", (heldObj == null) ? faces.IdleFace: faces.WalkFace);
                break;

            case AnimationCodeEnum.throwObj:
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) return;
                animator.SetTrigger("Attack");
                wasIdled = false;
                //faceMaterial.SetTexture("_MainTex", faces.AttackFace);
                break;
        }
    }

    public void AlertObservers()
    {
        // this is triggered after some animation ends
        // simply have this method to remove errors shown in console
    }


    public void playByRequest(AnimationCodeEnum aCode, SoundCodeEnum sCode)
    {
        this.animationCode = aCode;
        this.soundCode = sCode;
        PlaySound();
        PlayAnimation();
    }
}