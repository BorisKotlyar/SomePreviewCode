using System;
using GameSettings;
using UnityEngine;
using Zenject;

namespace UserData
{
    public abstract class User : ITickable, IInitializable
    {
        [Inject] protected readonly Settings Settings;

        public Action OnSave;

        private ISaveble _saveble;
        private float _currentTime;

        public virtual void Initialize()
        {
            // Load saved state
            _saveble = Load();
        }

        public virtual void Tick()
        {
            _currentTime += Time.deltaTime;
            if (_currentTime > Settings.SaveTimePeriod)
            {
                _currentTime -= Settings.SaveTimePeriod;

                Save(_saveble);

                if (OnSave != null)
                    OnSave.Invoke();
            }
        }

        public virtual void Save(ISaveble saveble)
        {
        }

        public virtual ISaveble Load()
        {
            return null;
        }
    }
}