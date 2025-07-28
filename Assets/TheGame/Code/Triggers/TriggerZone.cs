using System;
using UnityEngine;

namespace TheGame.Code
{
    [RequireComponent(typeof(Collider))]
    public abstract class TriggerZone<T> : MonoBehaviour where T : Component
    {
        [SerializeField] protected RaceManager _raceManager;
        protected Action _onTriggerAction;

        protected virtual void Awake()
        {
            SetTriggerAction(() => { });
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.transform.parent.TryGetComponent(out T component))
                return;
            _onTriggerAction?.Invoke();
        }

        protected void SetTriggerAction(Action action)
        {
            if (action == null || action.GetInvocationList().Length == 0)
            {
                Debug.LogWarning($"{gameObject.name} has an empty or null Action assigned in TriggerZone.");
            }

            _onTriggerAction = action;
        }
    }
}