using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerManager : MonoBehaviour
{
    private StarterAssetsInputs input;
    private ThirdPersonController controller;
    private Animator animator;

    [Header("Aim")]
    [SerializeField]
    private CinemachineVirtualCamera aimCam;
    [SerializeField]
    private GameObject aimImage;
    [SerializeField]
    private float aimSpeed = 50f;
    [SerializeField]
    private GameObject aimObject;
    [SerializeField]
    private float aimObjDis = 10f;
    [SerializeField]
    private LayerMask targetLayer;

    [Header("IK")]
    [SerializeField]
    private Rig handRig;
    [SerializeField]
    private Rig aimRig;

    void Start()
    {
        input = GetComponent<StarterAssetsInputs>();
        controller = GetComponent<ThirdPersonController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        AimCheck();
    }

    void AimCheck()
    {
        if (input.reroad)
        {
            input.reroad = false;

            if (controller.isReroad)
            {
                return;
            }

            AimController(false);
            SetRigWeight(0);
            animator.SetLayerWeight(1, 1);
            animator.SetTrigger("Reroad");
            controller.isReroad = true;
        }

        if (controller.isReroad)
        {
            return;
        }

        if (input.aim)
        {
            AimController(true);
            animator.SetLayerWeight(1, 1);

            Vector3 targetPosition = Vector3.zero;
            Transform camTransform = Camera.main.transform;
            RaycastHit hit;

            if (Physics.Raycast(camTransform.position, camTransform.forward, out hit, Mathf.Infinity, targetLayer))
            {
                targetPosition = hit.point;
                aimObject.transform.position = hit.point;
            }
            else
            {
                targetPosition = camTransform.position + camTransform.forward * aimObjDis;
                aimObject.transform.position = camTransform.position + camTransform.forward * aimObjDis;
            }

            Vector3 targetAim = targetPosition;
            targetAim.y = transform.position.y;
            Vector3 aimDir = (targetAim - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * aimSpeed);

            SetRigWeight(1);

            if (input.shoot)
            {
                animator.SetBool("Shoot", true);
                GameManager.instance.Shooting(targetPosition);
            }
            else
            {
                animator.SetBool("Shoot", false);
            }
        }
        else
        {
            AimController(false);
            SetRigWeight(0);
            animator.SetLayerWeight(1, 0);
            animator.SetBool("Shoot", false);
        }
    }

    void AimController(bool isCheck)
    {
        aimCam.gameObject.SetActive(isCheck);
        aimImage.SetActive(isCheck);
        controller.isAimMove = isCheck;
    }

    public void Reroad()
    {
        controller.isReroad = false;
        SetRigWeight(1);
        animator.SetLayerWeight(1, 0);
    }

    void SetRigWeight(float weight)
    {
        aimRig.weight = weight;
        handRig.weight = weight;
    }
}