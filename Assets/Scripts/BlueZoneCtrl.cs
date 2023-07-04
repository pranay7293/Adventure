        using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueZoneCtrl : MonoBehaviour
{
    [SerializeField]
    private Light blueZoneLight;
    [SerializeField]
    private GameObject playerObject;
    [SerializeField]
    private PlayerController playerController;

    private float zoneLifetime = 4f;
    private float damageRate = 1f;  
    private float minInterval = 8f;
    private float maxInterval = 10f;
    private bool isZoneActive = false;
   
    void Start()
    {
        StartRandomInterval();
    }

    private void Update()
    {
        if (isZoneActive)
        {
            if (playerObject != null)
            { 
                float distanceToPlayer = Vector3.Distance(transform.position, playerObject.transform.position);
                if (distanceToPlayer <= blueZoneLight.range)
                {                   
                    float damageAmount = damageRate * Time.deltaTime; ;
                    playerController.TakeDamage(damageAmount);
                }
            }
        }
    }

    private void StartRandomInterval()
    {
        float randomInterval = Random.Range(minInterval, maxInterval);
        Invoke("ActivateZone", randomInterval);
    }
    private void ActivateZone()
    {
        isZoneActive = true;
        blueZoneLight.gameObject.SetActive(true);
        Vector3 playerPosition = playerObject.transform.position;
        transform.position = new Vector3(playerPosition.x, 5f, playerPosition.z);
        Invoke("DeactivateZone", zoneLifetime);
    }

    private void DeactivateZone()
    {
        isZoneActive = false;
        blueZoneLight.gameObject.SetActive(false);

        StartRandomInterval();
    }
}
