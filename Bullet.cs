using System.Security.Cryptography;
using UnityEngine;

namespace Assets
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        
        [SerializeField] private float detectRadiWall;
        [SerializeField] private float detectRadiPlayer;
        
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private LayerMask wallLayer;
        
        private GameObject manager;
        private Score scoreManager;

        private float speed;
        
        private float aliveTimeCounter;

        private Transform player;
        private Vector3 target;
        private Vector3 direction;
        
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            
            manager = GameObject.FindGameObjectWithTag("Manager");
            scoreManager = manager.GetComponent<Score>();
            
            speed = moveSpeed * scoreManager.score;

            target = player.position;
            direction = target - transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            transform.Translate(new Vector2(direction.x, direction.y).normalized * speed * Time.deltaTime);

            bool hasHit = Physics2D.OverlapCircle(transform.position, detectRadiWall, wallLayer);
            bool playerHit = Physics2D.OverlapCircle(transform.position, detectRadiPlayer, playerLayer);
            
            if (hasHit)
                Destroy(gameObject);

            if (playerHit)
            {
                player.gameObject.GetComponent<PlayerDeath>().Dead();
                Destroy(gameObject);
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectRadiWall);
        }
    }
}
