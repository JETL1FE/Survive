using System;
using System.Collections;
using UnityEngine;
namespace WorldTime
{
    public class WorldTimes : MonoBehaviour
    {
        public event EventHandler<TimeSpan> WorldTimeChanged;
        [SerializeField]
        private float _dayLength;
        private TimeSpan _currentTime;
        private float _minuteLength => _dayLength / WorldTimeConstants.MinuteInDay;
        private void Start()
        {            
            StartCoroutine(AddMinute());
        }
        private IEnumerator AddMinute()
        {
            _currentTime += TimeSpan.FromMinutes(1);            
            WorldTimeChanged?.Invoke(this, _currentTime);
            yield return new WaitForSeconds(_minuteLength);
            StartCoroutine(AddMinute());
        }
        public TimeSpan CurrentTimeGetter()
        {
            return _currentTime;
        }
    }
}