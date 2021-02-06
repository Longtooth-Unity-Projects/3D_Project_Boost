using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RocketBehavior : MonoBehaviour
{
    [SerializeField] float verticalThrustFactor = 900f;
    [SerializeField] float rotationalThrustFactor = 100f;

    private const string friendlyTag = "Friendly";
    private const string fuelTag = "Fuel";
    private const string finishTag = "Finish";

    private AudioSource audioSource;
    private Rigidbody rigidBody;


    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        processVerticalThrust();
        processRotation();
    }



    private void processVerticalThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * verticalThrustFactor * Time.deltaTime);

            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
        }
    }//end of method processThrust


    private void processRotation()
    {
        //take manual control of rotation
        

        if (Input.GetKey(KeyCode.A))
        {
            rigidBody.freezeRotation = true;
            transform.Rotate(Vector3.forward * rotationalThrustFactor * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rigidBody.freezeRotation = true;
            transform.Rotate(Vector3.back * rotationalThrustFactor * Time.deltaTime);
        }

        //relinquish control of rotation
        rigidBody.freezeRotation = false;
    }//end of method processRotation

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case friendlyTag:
                //do nothing
                break;
            case fuelTag:
                Debug.Log("Collision is Fuel");
                break;
            case finishTag:
                Debug.Log("Collision is Finish");
                break;
            default:
                Debug.Log("Dead");
                break;
        }

    }
}
