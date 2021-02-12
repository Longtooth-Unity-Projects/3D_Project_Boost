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

    
    [SerializeField] private float verticalThrustFactor = 900f;
    [SerializeField] private float rotationalThrustFactor = 100f;
    [SerializeField] private float sceneLoadDelay = 1f;
    [SerializeField] private float sceneRestartDelay = 3f;

    [SerializeField] private AudioClip sfx_rocketThrust;
    [SerializeField] private AudioClip sfx_rocketCrash;
    [SerializeField] private AudioClip sfx_levelComplete;

    [SerializeField] private ParticleSystem particle_rocketThrust; //TODO fix this, doesnt show correctly, currently stop is commented out
    [SerializeField] private ParticleSystem particle_rocketExplosion;
    [SerializeField] private ParticleSystem particle_rocketSuccess;

    private const string friendlyTag = "Friendly";
    private const string fuelTag = "Fuel";
    private const string finishTag = "Finish";

    bool collisionsEnabled = true;

    //cached references
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
            processVerticalThrustInput();
            processRotationInput();
        }

        if(Debug.isDebugBuild)
        {
            respondToDebugKeys();
        }
        
    }// end of method Update



    private void processVerticalThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * verticalThrustFactor * Time.deltaTime);

            if (!audioSource.isPlaying)
                audioSource.PlayOneShot(sfx_rocketThrust);

            particle_rocketThrust.Play();
        }
        else
        {
            audioSource.Stop();
            //particle_rocketThrust.Stop();
        }
    }//end of method processThrust


    private void processRotationInput()
    {

        //remove rotation due to physics, if we use here, there is less movement as opposed to using it for each keystroke
        //rigidBody.angularVelocity = Vector3.zero;

        if (Input.GetKey(KeyCode.A))
        {
            rigidBody.angularVelocity = Vector3.zero;
            transform.Rotate(Vector3.forward * rotationalThrustFactor * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rigidBody.angularVelocity = Vector3.zero;
            transform.Rotate(Vector3.back * rotationalThrustFactor * Time.deltaTime);
        }
    }//end of method processRotation

    private void OnCollisionEnter(Collision collision)
    {
        if (playerState != PlayerState.alive || !collisionsEnabled) { return; }

        switch (collision.gameObject.tag)
        {
            case friendlyTag:
                //do nothing
                break;
            case fuelTag:
                break;
            case finishTag:
                audioSource.Stop();
                audioSource.PlayOneShot(sfx_levelComplete);
                particle_rocketSuccess.Play();
                playerState = PlayerState.transcending;
                FindObjectOfType<SceneLoader>().LoadNextScene(sceneLoadDelay);
                break;
            default:
                //destruction of ship
                audioSource.Stop();
                audioSource.PlayOneShot(sfx_rocketCrash);
                particle_rocketExplosion.Play();
                playerState = PlayerState.dying;
                FindObjectOfType<SceneLoader>().RestartScene(sceneRestartDelay);
                break;
        }//end of switch
    }//end of OnCollisionEnter

    void respondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            FindObjectOfType<SceneLoader>().LoadNextScene();
        }
        else if(Input.GetKeyDown(KeyCode.C))
        {
            collisionsEnabled = !collisionsEnabled;
        }
    }//end of respond to debug keys
}
