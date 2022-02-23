using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneRadar : MonoBehaviour
{
    private Animator phone;

    public Camera cameraOne;
    public Camera cameraTwo;

    public bool inOut = true;
    public bool usable = true;
    public float timer = 2.75f;


    private void Start()
    {
        cameraTwo.enabled = false;
        phone = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (usable)
        {
            if (inOut)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    phone.Play("Phone Flip In");
                    usable = false;
                    inOut = false;
                }
            }

            if (inOut == false)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    phone.Play("Phone Flip Out");
                    usable = false;
                    inOut = true;
                }
            }
        }

        if (usable == false)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                if (inOut == false)
                {
                    cameraOne.enabled = !cameraOne.enabled;
                    cameraTwo.enabled = !cameraTwo.enabled;
                }
                if (inOut)
                {
                    cameraOne.enabled = !cameraOne.enabled;
                    cameraTwo.enabled = !cameraTwo.enabled;
                }
                timer = 2.75f;
                usable = true;
            }
        }
    }
}
