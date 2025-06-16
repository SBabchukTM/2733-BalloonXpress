using System;
using UnityEngine;

namespace Runtime.Services.ApplicationState
{
    public class ApplicationStateService
    {
        public event Action ApplicationQuitEvent;
        public event Action<bool> ApplicationPauseEvent;

        private ApplicationStateMonoHelper _helper;

        public void Initialize()
        {
            GameObject applicationStateHelper = new GameObject("ApplicationStateHelper");
            _helper = applicationStateHelper.AddComponent<ApplicationStateMonoHelper>();

            _helper.ApplicationQuitEvent += NotifyApplicationQuitEvent;
            _helper.ApplicationPauseEvent += NotifyApplicationPauseEvent;
        }

        public void Dispose()
        {
            if (_helper == null)
                return;

            _helper.ApplicationQuitEvent -= NotifyApplicationQuitEvent;
            _helper.ApplicationPauseEvent -= NotifyApplicationPauseEvent;
        }

        private void NotifyApplicationQuitEvent()
        {
            ApplicationQuitEvent?.Invoke();
        }

        private void NotifyApplicationPauseEvent(bool isPause)
        {
            ApplicationPauseEvent?.Invoke(isPause);
        }
    }
}