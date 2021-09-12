using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpaceshipMovement : MonoBehaviour
{
    [SerializeField] private float speedX;
    [SerializeField] private float speedY;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Transform camera;
    
    [SerializeField] private Tilemap tilemap1;
    
    private Tilemap tilemap;
    private float currentRotation;
    private float currentDirection;
    
    private void Awake()
    {
        currentRotation = 0f;
            
        tilemap = GetComponent<Tilemap>();
    }

    void Update()
    {
        transform.localPosition += Vector3.up * (Time.deltaTime * speedY);
        transform.localPosition += Vector3.right * (Time.deltaTime * speedX);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Vector3 a = tilemap.CellToWorld(Vector3Int.zero);
        Vector3 offset = new Vector3(2.5f, 4.5f, 0f);
        Vector3 direction = (transform.position - offset) - camera.position;
        
        //for (float i = a.; i < 1f; i += 0.5f)
        {
            //for (float h = -1f; h < 1f; h += 0.5f)
            {
                Debug.DrawRay(camera.position, direction, Color.yellow, 1000f, false);
                if (Physics.Raycast(camera.position, direction, Single.PositiveInfinity))
                    Debug.Log($"hola");
            }
        }
        StartCoroutine(RotatingCoroutine(90f));
        StartCoroutine(ChangingDirectionCoroutine(1));
    }

    IEnumerator RotatingCoroutine(float finalRotation)
    {
        float rotationSum = currentRotation;

        while (rotationSum < finalRotation)
        {
            float rotationRad = rotationSum * Mathf.Deg2Rad;
            
            tilemap.orientationMatrix = new Matrix4x4
            (
                new Vector4(Mathf.Cos(rotationRad), -Mathf.Sin(rotationRad), 0f, 0f),
                new Vector4(Mathf.Sin(rotationRad), Mathf.Cos(rotationRad), 0f, 0f),
                new Vector4(0f, 0f, 1f, 0f),
                new Vector4(0f, 0f, 0f, 1f)
            );

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
        
        currentDirection = newDirection;
    }
}
