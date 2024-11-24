using System.Collections;
using UnityEngine;

namespace System
{
    public class MusicHandling : MonoBehaviour
    {
        public float FadeTime;
        public float IntensePercent;
        public AudioSource normalSource, intenseSource;
        private bool _normal;
        private bool _isFading;

        private void Update()
        {
            if (_normal && !_isFading && Timer.Instance.Percent <= IntensePercent)
            {
                StartCoroutine(Crossfade(normalSource, intenseSource));
                _normal = false;
            }
            else if (!_normal && !_isFading && Timer.Instance.Percent > IntensePercent)
            {
                StartCoroutine(Crossfade(intenseSource, normalSource));
                _normal = true;
            }
        }

        IEnumerator Crossfade(AudioSource fadeOutSource, AudioSource fadeInSource)
        {
            _isFading = true;

            float elapsed = 0f;
            while (elapsed < FadeTime)
            {
                elapsed += Time.deltaTime;
                var t = elapsed / FadeTime;
                fadeOutSource.volume = Mathf.Lerp(1f, 0f, t);
                fadeInSource.volume = Mathf.Lerp(0f, 1f, t);
                yield return null;
            }
            fadeOutSource.volume = 0f;
            fadeInSource.volume = 1f;
            _isFading = false;
        }
    }
}