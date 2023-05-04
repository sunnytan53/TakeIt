using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;

public enum AnimationCodeEnum { idle, walk, jump, pick, throwObj, landing };

public class PlayerArtController : MonoBehaviour
{
    private Animator animator;

    public Face faces;
    public GameObject SmileBody; // this should adpat to all individual slimes
    private Material faceMaterial;
    private bool wasIdled;

    private AnimatorStateInfo curAnimation;

    public EventReference soundWalk;
    public EventReference soundJump;
    public EventReference soundLanding;
    public EventReference soundPick;
    public EventReference soundThrow;

    private NetworkManager networkManager;

    private bool wasWalking;
    private float lastWalkTime;

    private bool sendNow;

    void Start()
    {
        animator = GetComponent<Animator>();
        faceMaterial = SmileBody.GetComponent<Renderer>().materials[1];
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    }

    public void setAnimationCode(AnimationCodeEnum aCode, bool fromRequest = false)
    {
        curAnimation = animator.GetCurrentAnimatorStateInfo(0);
        sendNow = false;
        if (Time.time - lastWalkTime > 1)
        {
            wasWalking = false;
        }

        switch (aCode)
        {
            case AnimationCodeEnum.walk:
                if (curAnimation.IsName("Jump")) return;
                if (curAnimation.IsName("Attack")) return;
                if (wasWalking) return;
                animator.SetTrigger("Jump");
                wasIdled = false;
                wasWalking = true;
                lastWalkTime = Time.time;
                RuntimeManager.PlayOneShot(soundWalk, transform.position);
                sendNow = true;
                break;
            case AnimationCodeEnum.jump:
                wasIdled = false;
                RuntimeManager.PlayOneShot(soundJump, transform.position);
                sendNow = true;
                break;
            case AnimationCodeEnum.pick:
                RuntimeManager.PlayOneShot(soundPick, transform.position);
                wasIdled = false;
                sendNow = true;
                break;
            case AnimationCodeEnum.throwObj:
                if (curAnimation.IsName("Attack")) return;
                animator.SetTrigger("Attack");
                wasIdled = false;
                RuntimeManager.PlayOneShot(soundThrow, transform.position);
                sendNow = true;
                break;
            case AnimationCodeEnum.landing:
                wasIdled = false;
                RuntimeManager.PlayOneShot(soundLanding, transform.position);
                sendNow = true;
                break;
            default:
                // the idle state info is not included in animator
                if (wasIdled) return;
                animator.ResetTrigger("Jump");
                wasIdled = true;
                //faceMaterial.SetTexture("_MainTex", faces.IdleFace);
                break;
        }

        if (sendNow && !fromRequest)
        {
            sendRequest(aCode);
        }
    }

    public void AlertObservers()
    {
        // this is triggered after some animation ends
        // simply have this method to remove errors shown in console
    }

    public void sendRequest(AnimationCodeEnum code)
    {
        networkManager.SendArtRequest(code);
    }
}