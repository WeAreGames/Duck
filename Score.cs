using UnityEngine;
using TMPro;

namespace Assets
{
    public class Score : MonoBehaviour
    {
        [SerializeField] private float extraUpTime;
        private float extraUpTimeCounter;
        
        [SerializeField] private TMP_Text scoreText;

        [HideInInspector] public int score;
        
        void Start()
        {
            extraUpTimeCounter = extraUpTime;
            score = 1;
        }
        
        void Update()
        {
            if (extraUpTimeCounter <= 0f)
            {
                score += 1;
                extraUpTimeCounter = extraUpTime;
            }
            else
            {
                extraUpTimeCounter -= Time.deltaTime;
            }
            
            scoreText.SetText(score.ToString());
        }
    }
}
