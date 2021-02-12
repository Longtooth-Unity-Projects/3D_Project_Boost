using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    private const float tau = Mathf.PI * 2f;


    [SerializeField] private Vector3 movementVector = new Vector3(10f, 10f, 10f);
    //time for full range of movement
    [SerializeField] private float movementPeriod = 2f;

    private Vector3 startingVector; //must be stored for absolute movement
    private float movementFactor = 0f; // 0 for no movement, 1 for full movement





    


    // Start is called before the first frame update
    void Start()
    {
        startingVector = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        oscillate();
    }

    private void oscillate()
    {
        //we don't want to divide by zero
        if(movementPeriod <= Mathf.Epsilon) { return; }


        // TODO clean this section and see if it can be simplified


        //how many cycles have passed since start of game, don't need delta time since we are using Time.time
        float movementCycles = Time.time / movementPeriod;
        //calculates where you are in the sign function
        float rawSinWave = Mathf.Sin(movementCycles * tau);
        //we want the movement factor to go between 0 and 1
        movementFactor = (rawSinWave / 2f) + 0.5f;

        transform.position = startingVector + (movementVector * movementFactor);
        //transform.position = startingVector + (movementVector * rawSinWave);
        //transform.position = startingVector + (movementVector * movementFactor);
        //transform.position = startingVector + ( movementVector * (((Mathf.Sin((Time.time / movementPeriod) * tau)) / 2f) + 0.5f) );
    }
}
