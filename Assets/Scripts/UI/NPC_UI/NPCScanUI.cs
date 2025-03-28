using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using TMPro;
using UnityEngine;

public class NPCScanUI : MonoBehaviour
{

    MobData mobData;
    // This controls the scanning UI for the player upon the npc
    public GameObject npcName;
    public GameObject npcDescription;
    public GameObject npcLevel;
    EnemyStats enemyStats;

    public GameObject npcScanUI;
    public GameObject player;
    // Get camera loc for scanUI
    public GameObject scanCamLocation;
    public GameObject mainCamLocation;
    public Camera mainCamera;

    public float transitionSpeed = 25f;

    // Start is called before the first frame update
    void Start()
    {
        enemyStats = transform.GetComponentInParent<EnemyStats>();
        mobData = enemyStats.mobData;

        npcScanUI.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        // If player is within scan range, looking at npc, and presses E, then show the scan UI
        if (Input.GetKeyDown(KeyCode.Z))
        {
            LayerMask layerMask = LayerMask.GetMask("Player");
            int mask = ~layerMask;
            if (Physics.Raycast(player.transform.position, player.transform.forward, out RaycastHit hit, 35, mask))
            {
                if (!hit.transform.CompareTag("NPC") && !hit.transform.CompareTag("Enemy"))
                {
                    return;
                }
                // Get data from NPC
                enemyStats = hit.transform.GetComponent<EnemyStats>();
                npcName.GetComponent<TMPro.TextMeshProUGUI>().text = mobData.mobName;
                npcLevel.GetComponent<TMPro.TextMeshProUGUI>().text = "Level: " + mobData.mobLevel.ToString();
                npcDescription.GetComponent<TMPro.TextMeshProUGUI>().text = mobData.mobDescription;

                npcScanUI.SetActive(true);


                Time.timeScale = 0.1f;
            }
        }

        if (npcScanUI.activeSelf)
        {
            ScanCam();
        }
        else
        {
            ScanCam(false);
        }


        if (Input.GetKeyUp(KeyCode.Z))
        {
            npcScanUI.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    void ScanCam(bool forward=true)
    {
        if (forward)
        {
            // Zoom camera to scan Cam
            mainCamera.transform.SetPositionAndRotation(Vector3.Slerp(mainCamera.transform.position, scanCamLocation.transform.position, Time.deltaTime*transitionSpeed), Quaternion.Slerp(mainCamera.transform.rotation, scanCamLocation.transform.rotation, Time.deltaTime*transitionSpeed));
            // Set parent of camera to scanCamLocation
            mainCamera.transform.SetParent(scanCamLocation.transform);
        }
        else
        {
            // Zoom camera back to main cam
            mainCamera.transform.SetPositionAndRotation(Vector3.Slerp(mainCamera.transform.position, mainCamLocation.transform.position, Time.deltaTime*transitionSpeed/3), Quaternion.Slerp(mainCamera.transform.rotation, mainCamLocation.transform.rotation, Time.deltaTime*transitionSpeed/3));
            // Set parent of camera to mainCamLocation
            mainCamera.transform.SetParent(mainCamLocation.transform);
        }
    }
}
