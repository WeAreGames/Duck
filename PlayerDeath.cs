using Assets.Platforming_script_one;
using UnityEngine;

namespace Assets
{
    public class PlayerDeath : MonoBehaviour
    {

        [SerializeField] private float oofRadius;
        [SerializeField] private LayerMask oofMask;
        
        private Player player;
        private PlayerVfx vfx;
        
        private GameObject manager;
        private SceneManaging sceneManager;
        
        void Start()
        {
            manager = GameObject.FindGameObjectWithTag("Manager");
            
            player = GetComponent<Player>();
            vfx = GetComponent<PlayerVfx>();
            
            sceneManager = manager.GetComponent<SceneManaging>();
        }

        void Update()
        {
            bool gotHit = Physics2D.OverlapCircle(transform.position, oofRadius, oofMask);

            if (gotHit)
            {
                Dead();
            }
        }
        
        public void Dead()
        {
            vfx.SetState("death");
            sceneManager.ReloadScene();
        }
        

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(transform.position, oofRadius);
        }
        
    }
}
