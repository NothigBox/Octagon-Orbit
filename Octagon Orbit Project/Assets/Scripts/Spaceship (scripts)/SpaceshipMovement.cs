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
    [SerializeField] private float currentSpeedX;
    [SerializeField] private float currentSpeedY;
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
        transform.localPosition += Vector3.up * (Time.deltaTime * currentSpeedY);
        transform.localPosition += Vector3.right * (Time.deltaTime * currentSpeedX);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Vector3 planetDirection = (other.transform.position - transform.position).normalized;
        MoveDirection towardsDirection = currentDirection;

        if (currentDirection == MoveDirection.up || currentDirection == MoveDirection.down)
        {
            towardsDirection = planetDirection.x > 0f ? 
                MoveDirection.right : planetDirection.x < 0f ? 
                    MoveDirection.left : MoveDirection.crash;
        }
        else if (currentDirection == MoveDirection.right || currentDirection == MoveDirection.left)
        {
            towardsDirection = planetDirection.y > 0f ? 
                MoveDirection.up : planetDirection.y < 0f ? 
                    MoveDirection.down : MoveDirection.crash;
        }

        StartCoroutine(SetDirectionDelay(towardsDirection));

        other.enabled = false;
    }

    IEnumerator SetDirectionDelay(MoveDirection towardsDirection)
    {
        yield return new WaitForSeconds(0.5f/rotationSpeed);
        
        SetDirection(towardsDirection);
    }

    void SetDirection(MoveDirection direction)
    {
        float finalRotation = currentRotation;

        switch (currentDirection)
        {
            case MoveDirection.up:
                switch (direction)
                {
                    case MoveDirection.right:
                        finalRotation = currentRotation - 90f;
                        break;
                    case MoveDirection.left:
                        finalRotation = currentRotation + 90f;
                        break;
                }
                break;
            
            case MoveDirection.down:
                switch (direction)
                {
                    case MoveDirection.right:
                        finalRotation = currentRotation + 90f;
                        break;
                    case MoveDirection.left:
                        finalRotation = currentRotation - 90f;
                        break;
                }
                break;
            
            case MoveDirection.right:
                switch (direction)
                {
                    case MoveDirection.up:
                        finalRotation = currentRotation + 90f;
                        break;
                    case MoveDirection.down:
                        finalRotation = currentRotation - 90f;
                        break;
                }
                break;
            
            case MoveDirection.left:
                switch (direction)
                {
                    case MoveDirection.up:
                        finalRotation = currentRotation - 90f;
                        break;
                    case MoveDirection.down:
                        finalRotation = currentRotation + 90f;
                        break;
                }
                break;
        }

        currentDirection = direction;

        StartCoroutine(RotatingCoroutine(finalRotation));
    }
    
    IEnumerator RotatingCoroutine(float finalRotation)
    {
        float rotationSum = currentRotation;
        float rotationSence = (finalRotation - currentRotation > 0? 
            1f : (finalRotation - currentRotation < 0? 
                -1 : 0f));
        
        while (Mathf.Abs(finalRotation - rotationSum) > 0f)
        {
            transform.localRotation = Quaternion.Euler(Vector3.forward * rotationSum);
            
            currentSpeedY = Mathf.Cos(rotationSum * Mathf.Deg2Rad);
            currentSpeedX = -Mathf.Sin(rotationSum * Mathf.Deg2Rad);
            
            rotationSum += rotationSpeed * rotationSence;
            
            yield return null;
        }
        
        currentRotation = finalRotation;
    }
}
