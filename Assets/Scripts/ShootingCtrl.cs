using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum GunType
{
    Pistol,
    Rifle    
}

public class ShootingCtrl : MonoBehaviour
{
    [SerializeField]
    private GunType gunType;

    [SerializeField]
    private Animator animator;


    [SerializeField]
    private GameObject HandPistol;
    [SerializeField]
    private GameObject SocketPistol;

    public Camera mainCamera;
    public GameObject aimCameraPosition;
    public GameObject mainCameraPosition;
    public Image crossHair;


    [SerializeField]
    private GameObject bulletPrefab;
    public Transform bulletFirePosition;
    public bool isAim = false;

   
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (isAim)
            {
                mainCamera.transform.position = mainCameraPosition.transform.position;
                CloseShootMode();
            }
            else
            {
                mainCamera.transform.position = aimCameraPosition.transform.position;
                GetIntoShootMode();
            }
        }
        if(Input.GetKeyDown(KeyCode.Mouse0) && isAim)
        {
            Shoot();
        }

    }

    private void GetIntoShootMode()
    {
        isAim = true;
        SocketPistol.gameObject.SetActive(false);
        HandPistol.gameObject.SetActive(true);
        animator.SetTrigger("PistolAim");
        crossHair.gameObject.SetActive(true);
    }
    private void CloseShootMode()
    {
        isAim = false;
        SocketPistol.gameObject.SetActive(true);
        HandPistol.gameObject.SetActive(false);
        crossHair.gameObject.SetActive(false);
    }

    private void Shoot()
    {
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if(Physics.Raycast(ray, out RaycastHit rayCastHit, 999f))
        {
            GameObject hitObject = rayCastHit.collider.gameObject;
            string hitObjectName = rayCastHit.collider.gameObject.name;
            Debug.Log("Hit object name: " + hitObjectName);

            if (hitObject.CompareTag("Enemy"))
            {
                EnemyController enemyController = hitObject.GetComponent<EnemyController>();
                if (enemyController != null)
                {
                    EnemyType enemyType = enemyController.GetEnemyType();

                    int damage;
                    switch (enemyType)
                    {
                        case EnemyType.BlackSpider:
                            damage = Random.Range(20, 30 + 1);
                            break;
                        case EnemyType.SandSpider:
                            damage = Random.Range(20, 30 + 1);
                            break;
                        case EnemyType.Turtle:
                            damage = Random.Range(30, 40 + 1);
                            break;
                        case EnemyType.Slime:
                            damage = Random.Range(30, 40 + 1);
                            break;
                        case EnemyType.Minotaur:
                            damage = Random.Range(10, 20 + 1);
                            break;
                        default:
                            damage = 0;
                            break;
                    }                    
                        enemyController.TakeDamage(damage);                    
                }
            }
            Vector3 aimDirection = (rayCastHit.point - bulletFirePosition.transform.position).normalized;
            Instantiate(bulletPrefab, bulletFirePosition.position, Quaternion.LookRotation(aimDirection));
        }
    }
}
