using UnityEngine;

namespace Assets.Platforming_script_one
{
    public class VFXPlayer : MonoBehaviour
    {
        [SerializeField] protected Animator anim;
        [SerializeField] protected SpriteRenderer sprite;

        [Space]

        [SerializeField] protected AudioClip[] soundEffects;
        [SerializeField] protected AudioSource audioSource;

        protected string CurrentState;
        protected string PreviousState;

        public void SetState(string state)
        {
            if (CurrentState == state)
                return;

            PreviousState = CurrentState;

            CurrentState = state;

            anim.SetBool(state, true);
            anim.SetBool(PreviousState, false);
        }

        public void PlayAudio(string clipName)
        {
            foreach(AudioClip aClip in soundEffects)
            {
                if (aClip.name == clipName)
                {
                    audioSource.clip = aClip;
                    audioSource.Play();
                }
            }
        }
    }
}
