using UnityEngine;

namespace Assets
{
    public class GunnerShoot : MonoBehaviour
    {
        [SerializeField] private float shootTime;
        [SerializeField] private GameObject bullet;
        [SerializeField] private Transform shootPoint;

        private float shootTimeCounter;

        private Transform player;
        
        
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            shootTimeCounter = shootTime;
        }
        
        void Update()
        {
            if (shootTimeCounter <= 0)
            {
                Instantiate(bullet, shootPoint.position, Quaternion.identity);
                shootTimeCounter = shootTime;
            }
            else
            {
                shootTimeCounter -= Time.deltaTime;
            }
        }
    }
}
