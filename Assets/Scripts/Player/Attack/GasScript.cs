using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasScript : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float cooldownTime = 0.1f;
    private Transform firePoint;

    private List<GameObject> damagedViruses = new List<GameObject>();
    private float lastDamageTime = 0f;

    private Camera mainCam;

    private void Start() {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        firePoint = GameObject.FindGameObjectWithTag("FirePoint").transform;
    }

    private void OnParticleCollision(GameObject collision)
    {
        if (collision.gameObject.CompareTag("Virus") && !damagedViruses.Contains(collision.gameObject))
        {
            if (collision.gameObject.GetComponent<VirusLife>() != null)
            {
                collision.gameObject.GetComponent<VirusLife>().TakeDamage(damage);
                damagedViruses.Add(collision.gameObject);
            }
            else
            {
                Destroy(collision.gameObject);
            }
        }
    }

    private void Update()
    {
        if (Time.time - lastDamageTime >= cooldownTime)
        {
            damagedViruses.Clear();
            lastDamageTime = Time.time;
        }

        float gunRotation = firePoint.eulerAngles.z * Mathf.Deg2Rad;
        transform.position = firePoint.position + new Vector3(-Mathf.Sin(gunRotation) * 0.3f, Mathf.Cos(gunRotation) * 0.3f, 0);

        // Rotation looks weird in certain angles, did not manage to fix it
        Vector3 mousePos = mainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCam.transform.position.z));
        Vector3 rotation = mousePos - transform.position;
        transform.rotation = Quaternion.LookRotation(rotation, Vector3.forward);
    }
}
