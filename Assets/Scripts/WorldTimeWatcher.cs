using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace WorldTime
{
    public class WorldTimeWatcher : MonoBehaviour
    {
        [SerializeField]
        private WorldTimes _worldTime;
        [Serializable]
        private class Schedule
        {
            public int Hour;
            public int Minute;
            public UnityEvent _action;

        }
        [SerializeField]
        private List<Schedule> _schedule;


        [SerializeField]
        private void Start()
        {
            _worldTime.WorldTimeChanged += CheckSchedule;
        }
        private void OnDestroy()
        {
            _worldTime.WorldTimeChanged -= CheckSchedule;
        }

        private void CheckSchedule(object sender, TimeSpan newTime)
        {
            var schedule = 
                _schedule.FirstOrDefault(s =>
                s.Hour == newTime.Hours && s.Minute == newTime.Minutes);
            schedule?._action?.Invoke();
            //throw new NotImplementedException();
        }
    }
}