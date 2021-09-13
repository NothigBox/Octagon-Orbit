using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

enum MoveDirection {nw, n, ne, e, w, sw, s, se, crash}
public class SpaceshipMovement : MonoBehaviour
{
    [SerializeField] private float currentSpeedX;
    [SerializeField] private float currentSpeedY;
    [SerializeField] private float rotationSpeed;
    
    private float currentRotation;
    private MoveDirection currentDirection;
    private PlanetType movementType;
    
    private void Awake()
    {
        currentRotation = 0f;
        currentDirection = MoveDirection.n;
        movementType = PlanetType.rect;
    }

    void Update()
    {
        transform.localPosition += Vector3.up * (Time.deltaTime * currentSpeedY);
        transform.localPosition += Vector3.right * (Time.deltaTime * currentSpeedX);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Planet"))
        {
            Vector3 planetDirection = (other.transform.position - transform.position).normalized;
            MoveDirection towardsDirection = currentDirection;
            
            int y = planetDirection.y > 0f ? 
                    1 : planetDirection.y < 0f ? 
                        -1 : 0;
            int x = planetDirection.x > 0f ? 
                    1 : planetDirection.x < 0f ? 
                        -1 : 0;
            
            switch (y)
            {
                case 1:
                    switch (x)
                    {
                        case 1:
                            towardsDirection = MoveDirection.ne;
                            break;
                        case 0:
                            towardsDirection = MoveDirection.n;
                            break;
                        case -1:
                            towardsDirection = MoveDirection.nw;
                            break;
                    }   
                    break;
                case 0:
                    switch (x)
                    {
                        case 1:
                            towardsDirection = MoveDirection.e;
                            break;
                        case 0:
                            Debug.Log("A");
                            break;
                        case -1:
                            towardsDirection = MoveDirection.w;
                            break;
                    }   
                    break;
                case -1:
                    switch (x)
                    {
                        case 1:
                            towardsDirection = MoveDirection.se;
                            break;
                        case 0:
                            towardsDirection = MoveDirection.s;
                            break;
                        case -1:
                            towardsDirection = MoveDirection.sw;
                            break;
                    }   
                    break;
            }
            
            StartCoroutine(SetDirectionDelay(towardsDirection, other.GetComponent<Planet>()._Type));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Planet"))
        {
            other.GetComponent<Planet>().StartCooldown();
        }
    }

    IEnumerator SetDirectionDelay(MoveDirection towardsDirection, PlanetType planetType)
    {
        float rotationDistance = 0f;
        
        switch (planetType)
        {
            case PlanetType.rect:
                rotationDistance = 0.5f;
                break;
            
            case PlanetType.diagonal:
                if (movementType == PlanetType.rect)
                {
                    rotationDistance = 0.25f;   
                }
                else
                {
                    rotationDistance = 0.7f;   
                }
                break;
        }
        
        movementType = planetType;
        
        yield return new WaitForSecondsRealtime(rotationDistance/rotationSpeed);
        
        SetDirection(towardsDirection, planetType);
    }

    void SetDirection(MoveDirection direction, PlanetType planetType)
    {
        if (direction == MoveDirection.crash)
        {
            Debug.Log("Crashed");
            return;
        }
        
        float finalRotation = currentRotation;
        float rotationType = 0f;

        switch (planetType)
        {
            case PlanetType.rect:
                rotationType = 90f;
                break;
            
            case PlanetType.diagonal:
                rotationType = 45f;
                break;
        }
        
        switch (currentDirection)
        {
            case MoveDirection.n:
                switch (direction)
                {
                    case MoveDirection.e:
                        finalRotation = currentRotation - rotationType;
                        break;
                    case MoveDirection.w:
                        finalRotation = currentRotation + rotationType;
                        break;
                }
                break;
            
            case MoveDirection.s:
                switch (direction)
                {
                    case MoveDirection.e:
                        finalRotation = currentRotation + rotationType;
                        break;
                    case MoveDirection.w:
                        finalRotation = currentRotation - rotationType;
                        break;
                }
                break;
            
            case MoveDirection.e:
                switch (direction)
                {
                    case MoveDirection.n:
                        finalRotation = currentRotation + rotationType;
                        break;
                    case MoveDirection.s:
                        finalRotation = currentRotation - rotationType;
                        break;
                }
                break;
            
            case MoveDirection.w:
                switch (direction)
                {
                    case MoveDirection.n:
                        finalRotation = currentRotation - rotationType;
                        break;
                    case MoveDirection.s:
                        finalRotation = currentRotation + rotationType;
                        break;
                }
                break;
            
            
            
            case MoveDirection.nw:
                switch (direction)
                {
                    case MoveDirection.ne:
                        finalRotation = currentRotation - rotationType;
                        break;
                    case MoveDirection.sw:
                        finalRotation = currentRotation + rotationType;
                        break;
                }
                break;
            
            case MoveDirection.ne:
                switch (direction)
                {
                    case MoveDirection.nw:
                        finalRotation = currentRotation - rotationType;
                        break;
                    case MoveDirection.se:
                        finalRotation = currentRotation + rotationType;
                        break;
                }
                break;
            
            case MoveDirection.sw:
                switch (direction)
                {
                    case MoveDirection.nw:
                        finalRotation = currentRotation - rotationType;
                        break;
                    case MoveDirection.se:
                        finalRotation = currentRotation + rotationType;
                        break;
                }
                break;
            
            case MoveDirection.se:
                switch (direction)
                {
                    case MoveDirection.ne:
                        finalRotation = currentRotation - rotationType;
                        break;
                    case MoveDirection.sw:
                        finalRotation = currentRotation + rotationType;
                        break;
                }
                break;
        }

        Debug.Log($"Current {currentDirection} | Direction {direction}");
        
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
