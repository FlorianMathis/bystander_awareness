using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour {

    [SerializeField] private Transform pfRadarPing;
    [SerializeField] private LayerMask radarLayerMask;

    private Transform sweepTransform;
    private float rotationSpeed;
    private float radarDistance;
    private List<Collider> colliderList;

    private void Awake() {
        sweepTransform = transform.Find("Sweep");
        rotationSpeed = 180f;
        radarDistance = 150f;
        colliderList = new List<Collider>();
    }

    private void Update() {
        float previousRotation = (sweepTransform.localEulerAngles.z % 360) - 180;
        sweepTransform.localEulerAngles -= new Vector3(0, 0, rotationSpeed * Time.deltaTime);
        float currentRotation = (sweepTransform.localEulerAngles.z % 360) - 180;

        if (previousRotation < 0 && currentRotation >= 0) {
            // Half rotation
            colliderList.Clear();
        }
        // Debug.Log(sweepTransform.localEulerAngles.z);
       //  Debug.Log(GetVectorFromAngle(sweepTransform.localEulerAngles.z*Mathf.Deg2Rad));
        Ray ray = new Ray(transform.position, GetVectorFromAngle(sweepTransform.localEulerAngles.z));
       
        RaycastHit[] raycastHitArray = Physics.RaycastAll(ray, radarDistance, radarLayerMask);
        //Debug.Log(raycastHitArray.Length);
        foreach (RaycastHit raycastHit in raycastHitArray) {
            if (raycastHit.collider != null) {
                // Hit something
                if (!colliderList.Contains(raycastHit.collider)) {
                    // Hit this one for the first time
                    colliderList.Add(raycastHit.collider);
                    // Debug.Log("Hit " + raycastHit.collider.gameObject.name);

                    RadarPing radarPing = Instantiate(pfRadarPing, new Vector3(raycastHit.point.x,1.5f,raycastHit.point.z), Quaternion.Euler(90,0,0)).GetComponent<RadarPing>();
                    //radarPing.transform.SetParent(transform);
                    if (raycastHit.collider.gameObject.GetComponent<Person>() != null) {
                        // Hit an Item
                        radarPing.SetColor(new Color(0, 1, 0));
                        // Debug.Log("Hit Person");
                    }
                    if (raycastHit.collider.gameObject.GetComponent<Attacker>() != null) {
                        // Hit an Enemy
                        radarPing.SetColor(new Color(1, 0, 0));
                        // Debug.Log("Hit Attacker");
                    }
                    radarPing.SetDisappearTimer(360f / rotationSpeed * 1f);
                }
            }
        }
    }

            public static Vector3 GetVectorFromAngle(float angle) {
            // angle = 0 -> 360
            float angleRad = angle * (Mathf.PI/180f);
            return new Vector3(Mathf.Sin(angleRad),0,Mathf.Cos(angleRad));
        }

}
