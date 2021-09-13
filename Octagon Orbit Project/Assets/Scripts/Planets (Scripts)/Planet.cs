using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlanetType {rect, diagonal}
public class Planet : MonoBehaviour
{
    [SerializeField] private PlanetType type;

    private Collider2D collider;

    public PlanetType _Type => type; 
        
    private void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    public void StartCooldown()
    {
        StartCoroutine(CooldownCoroutine());
    }

    IEnumerator CooldownCoroutine()
    {
        collider.enabled = false;
        yield return new WaitForSecondsRealtime(1f);
        collider.enabled = true;
    }
}
