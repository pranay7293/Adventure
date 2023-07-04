using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ShootingCtrl : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private GameObject HandPistol;
    [SerializeField]
    private GameObject SocketPistol;   
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private Transform aimCameraPosition;
    [SerializeField]
    private Transform mainCameraPosition;   
    [SerializeField]
    private Image crossHair;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform bulletFirePosition;
    [SerializeField]
    private float bulletspeed = 500f;
    [SerializeField]
    private Rigidbody bulletrb;
   
    

    private bool isAim = false;    
    public bool isGame = false;
    
    private void Update()
    {
       if (!isGame)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (isAim)
                {
                    mainCamera.transform.position = mainCameraPosition.position;
                    animator.SetBool("PistolAim", false);
                    CloseShootMode();
                }
                else
                {
                    mainCamera.transform.position = aimCameraPosition.position;
                    GetIntoShootMode();
                }
            }
            if (Input.GetKeyDown(KeyCode.Mouse0) && isAim)
            {
                animator.SetBool("PistolAim", true);
                Shoot();
            }
        }
    }

   
    private void GetIntoShootMode()
    {
        isAim = true;
        SoundManager.Instance.PlaySound(Sounds.PistolSocket);
        SocketPistol.gameObject.SetActive(false);
        HandPistol.gameObject.SetActive(true);
        crossHair.gameObject.SetActive(true);
    }
    private void CloseShootMode()
    {
        isAim = false;
        SoundManager.Instance.PlaySound(Sounds.PistolSocket);
        SocketPistol.gameObject.SetActive(true);
        HandPistol.gameObject.SetActive(false);
        crossHair.gameObject.SetActive(false);
    }

    private void Shoot()
    {
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if(Physics.Raycast(ray, out RaycastHit rayCastHit, 999f))
        {
            SoundManager.Instance.PlaySound(Sounds.PistolFire);
            Vector3 aimDirection = (rayCastHit.point - bulletFirePosition.transform.position).normalized;
            GameObject bulletObject = Instantiate(bulletPrefab, bulletFirePosition.position, Quaternion.LookRotation(aimDirection));

            bulletrb = bulletObject.GetComponent<Rigidbody>();
            bulletrb.velocity = transform.forward * bulletspeed * Time.deltaTime;
            Destroy(bulletObject.gameObject, 6f);

            GameObject hitObject = rayCastHit.collider.gameObject;
            
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
                            damage = Random.Range(30, 30 + 1);
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
            else
            {
                Destroy(bulletObject.gameObject);
            }

        }
    }    

}
