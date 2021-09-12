using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

enum MoveDirection {up, down, right, left, crash}
public class SpaceshipMovement : MonoBehaviour
{
    [SerializeField] private float speedX;
    [SerializeField] private float speedY;
    [SerializeField] private float rotationSpeed;
    
    private float currentRotation;
    private MoveDirection currentDirection;
    
    private void Awake()
    {
        currentRotation = 0f;
        currentDirection = MoveDirection.up;
    }

    void Update()
    {
        transform.localPosition += Vector3.up * (Time.deltaTime * speedY);
        transform.localPosition += Vector3.right * (Time.deltaTime * speedX);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Vector3 planetDirection = (other.transform.position - transform.position).normalized;

        if (currentDirection == MoveDirection.up || currentDirection == MoveDirection.down)
        {
            currentDirection = planetDirection.x > 0f ? 
                MoveDirection.right : planetDirection.x < 0f ? 
                    MoveDirection.left : MoveDirection.crash;
        }
        
        Debug.Log(currentDirection);
        
        SetDirection(currentDirection);
    }

    void SetDirection(MoveDirection direction)
    {
        
    }
    
    IEnumerator RotatingCoroutine(float finalRotation)
    {
        float rotationSum = currentRotation;

        while (rotationSum < finalRotation)
        {
            float rotationRad = rotationSum;
            
            transform.localRotation = Quaternion.Euler(Vector3.forward * rotationSum);

            rotationSum += rotationSpeed;
            
            yield return null;
        }

        currentRotation = finalRotation;
    }

    IEnumerator ChangingDirectionCoroutine(int newDirection)
    {
        float deltaSpeedX = 0f, deltaSpeedY = 0f;
        float finalSpeedX = 0f, finalSpeedY = 0f;

        switch (newDirection)
        {
            case 0:
                finalSpeedX = 0f;
                finalSpeedY = 1f;
            break;
            
            case 1:
                finalSpeedX = 1f;
                finalSpeedY = 0f;

                deltaSpeedX = 0.0025f;
                //deltaSpeedY = -0.1f;
            break;
        }
        
        while (speedX < finalSpeedX)
        {
            speedX += deltaSpeedX;
            speedY += deltaSpeedY;
            yield return null;
        }
        
        speedX = finalSpeedX;
        speedY = finalSpeedY;
        
        //currentDirection = newDirection;
    }
}
