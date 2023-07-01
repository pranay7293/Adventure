using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    [SerializeField]
    private float bulletspeed = 500f;
    [SerializeField]
    private Rigidbody bulletrb;

    private void Awake()
    {
        bulletrb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        bulletrb.velocity = transform.forward * bulletspeed * Time.deltaTime;
        Destroy(gameObject, 6f);
    }
    private void OnTriggerEnter(Collider other)
    {        
        Destroy(this.gameObject);
    }
}
