using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public class NewScript : MonoBehaviour
{
    
    // This is the reference to another object in the scene.
    [SerializeField] private GameObject cube;
    
    // This is the reference to another Component in the scene, which is of type RigidBody.
    [SerializeField] private Rigidbody cubeRigidBody;
    
    private bool isFirstScript = true;

    // Start is called before the first frame update
    // This, and the Update method, are given by MonoBehaviour
    private void Start()
    {
        if(isFirstScript) Debug.Log("This is the first script that we have made!");
        
        
        // This changes the cubes mass
        ChangeCubeMass(6);
        
        // This turns the numbers one to ten into a string, which is then outputted.

        var tempList = new List<int>();
        
        for (var i = 0; i < 11; i++)
        {
            tempList.Add(i);
        }
        
        // This is a useful LINQ expression, which turns the first 10 numbers into a list, then outputs it.
        var newString = tempList.Aggregate("", (current, item) => current + item);
        Debug.Log(newString);
        
        // This starts the Deletion IEnumerable
        StartCoroutine(DeleteCubeInSeconds(15));
    }

    private void ChangeCubeMass(int newMass)
    {
        cubeRigidBody.mass = newMass;
    }

    private IEnumerator DeleteCubeInSeconds(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(cube);
    }
}


















