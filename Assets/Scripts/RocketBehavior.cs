using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RocketBehavior : MonoBehaviour
{
    enum PlayerState
    {
        alive,
        dying,
        transcending
    }
    PlayerState playerState = PlayerState.alive;


    [SerializeField] float verticalThrustFactor = 900f;
    [SerializeField] float rotationalThrustFactor = 100f;
    [SerializeField] float sceneLoadDelay = 1f;
    [SerializeField] float sceneRestartDelay = 3f;

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
        if (playerState == PlayerState.alive)
        {
            processVerticalThrust();
            processRotation();
        }
        else if (playerState == PlayerState.dying && audioSource.isPlaying)
            audioSource.Stop();
    }// end of method Update



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
        if (playerState != PlayerState.alive) { return; }

        switch (collision.gameObject.tag)
        {
            case friendlyTag:
                //do nothing
                break;
            case fuelTag:
                break;
            case finishTag:
                playerState = PlayerState.transcending;
                FindObjectOfType<SceneLoader>().LoadNextScene(sceneLoadDelay);
                break;
            default:
                playerState = PlayerState.dying;
                FindObjectOfType<SceneLoader>().RestartScene(sceneRestartDelay);
                break;
        }

    }
}
