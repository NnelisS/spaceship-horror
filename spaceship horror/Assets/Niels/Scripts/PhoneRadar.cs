using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneRadar : MonoBehaviour
{
    private Animator phone;

    public GameObject radarCanvas;
    public GameObject informationCanvas;

    public Camera cameraOne;
    public Camera cameraTwo;

    public GameObject camera;

    private Movement playerMovement;

    private bool inOut = false;
    private bool usable = true;
    private float timer = 2.75f;


    private void Start()
    {
        playerMovement = GetComponent<Movement>();

        radarCanvas.SetActive(false);
        inOut = false;
        cameraTwo.enabled = false;
        phone = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (usable)
        {
            if (inOut == true)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    phone.Play("Phone Flip In");
                    cameraTwo.enabled = !cameraTwo.enabled;
                    cameraOne.enabled = !cameraOne.enabled;
                    playerMovement.enabled = true;
                    radarCanvas.SetActive(false);
                    informationCanvas.SetActive(true);
                    usable = false;
                }
            }

            if (inOut == false)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    camera.transform.rotation = Quaternion.Euler(0, transform.rotation.y, transform.rotation.z);
                    playerMovement.enabled = false;
                    phone.Play("Phone Flip Out");
                    StartCoroutine(OpenRader());
                    usable = false;
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
                    inOut = true;
                }
                else if (inOut == true)
                {
                    inOut = false;
                }
                timer = 2.75f;
                usable = true;
            }
        }
    }

    private IEnumerator OpenRader()
    {
        yield return new WaitForSeconds(2.75f);
        cameraOne.enabled = !cameraOne.enabled;
        cameraTwo.enabled = !cameraTwo.enabled;
        radarCanvas.SetActive(true);
        informationCanvas.SetActive(false);
    }
}
