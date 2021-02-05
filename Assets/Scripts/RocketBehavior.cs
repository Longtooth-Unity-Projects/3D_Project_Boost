using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBehavior : MonoBehaviour
{
    [SerializeField] float verticalThrustFactor = 10f;
    [SerializeField] float rotationalThrustFactor = 100f;


    private AudioSource audioSource;
    private Rigidbody rigidBody;

    private Vector3 thrustVector;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        thrustVector = new Vector3(0, verticalThrustFactor, 0);

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
            rigidBody.AddRelativeForce(Vector3.up * verticalThrustFactor);

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


}
