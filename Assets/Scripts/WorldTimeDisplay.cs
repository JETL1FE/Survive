using System;
using TMPro;
using UnityEngine;

namespace WorldTime
{
    [RequireComponent(typeof(TMP_Text))]
    public class WorldTimeDisplay : MonoBehaviour
    {
        [SerializeField]
        private WorldTimes _worldTime;
        private TMP_Text _text;
        // Start is called before the first frame update
        void Awake()
        {
            _text = GetComponent<TMP_Text>();
            _worldTime.WorldTimeChanged += OnWorldTimeChanged;
        }

        private void OnWorldTimeChanged(object sender, TimeSpan newTime)
        {
            _text.SetText(newTime.ToString(@"hh\:mm"));
        }
        private void OnDestroy()
        {
            _worldTime.WorldTimeChanged -= OnWorldTimeChanged;
        }
    }
}