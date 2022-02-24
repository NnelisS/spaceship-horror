using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class Movement : MonoBehaviour
{
    [Header("Movement Varibles")]
    [SerializeField] float moveSpeed = 5;
    [SerializeField] float sprintMultiplier = 1.25f;
    [SerializeField] float jumpForce = 5;
    [SerializeField] float gravity = -9.81f;

    [Header("Audio sources")]
    [SerializeField] AudioSource walk;
    [SerializeField] AudioSource run;

    [Header("Movement smoothing")]
    [SerializeField, Range(0.0f, 0.5f)] float smoothTime = 0.3f;

    [Header("Other")]
    [SerializeField] Slider slider;

    Vector2 targetDir = Vector2.zero;
    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;
    float velocityY;

    bool disableMovement = false;
    bool isSprinting = false;

    public CharacterController controller;

    float runTimer = 100f;


    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (runTimer <= 0) { isSprinting = false; }
        if (isSprinting && runTimer > 0) { runTimer -= Time.deltaTime * 10; }
        else if (runTimer < 100 && !isSprinting) { runTimer += Time.deltaTime * 7.5f; }

        slider.value = runTimer / 100;

        if (disableMovement) { return; }

        float speed = isSprinting ? moveSpeed * sprintMultiplier : moveSpeed;

        if (targetDir != Vector2.zero) {
            walk.enabled = !isSprinting;
            run.enabled = isSprinting;
        } else {
            walk.enabled = false;
            run.enabled = false;
        }

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, smoothTime);

        if (controller.isGrounded && velocityY < 0) { velocityY = 0; }
        velocityY += gravity * Time.deltaTime;

        Vector3 vel = (transform.forward * currentDir.y + transform.right * currentDir.x) * speed + Vector3.up * velocityY;

        controller.Move(vel * Time.deltaTime);
    }

    public void UpdateHorizontal(float axis)
    {
        targetDir.x = axis;
        targetDir.Normalize();
    }

    public void UpdateVertical(float axis)
    {
        targetDir.y = axis;
        targetDir.Normalize();
    }

    public void Jump()
    {
        if(!controller.isGrounded) { return; }
        velocityY += jumpForce;
    }

    public void Hide()
    {
        disableMovement = !disableMovement;
    }

    public void Sprint(bool check)
    {
        isSprinting = check;
    }
}
