using MixedReality.Toolkit;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace MRTKExtensions.Utilities
{
    public class BrowserActivator : MRTKBaseInteractable
    {
        [SerializeField]
        private string urlToOpen;

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);
#if WINDOWS_UWP
            UnityEngine.WSA.Launcher.LaunchUri(urlToOpen, false);
#else
            Application.OpenURL(urlToOpen);
#endif
            TryPlaySound();
        }

        private AudioSource audioSource;

        private void TryPlaySound()
        {
            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }

            if (audioSource != null)
            {
                audioSource.Play();
            }
        }
    }
}
