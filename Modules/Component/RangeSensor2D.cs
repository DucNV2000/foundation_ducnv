using System.Collections.Generic;
using Pancake.Apex;
using Pancake.Scriptable;
using UnityEngine;

namespace Pancake.Component
{
    [HideMonoScript]
    public class RangeSensor2D : Sensor
    {
        [Space(8)] [SerializeField] private LayerMask layer;
        [SerializeField] private float range;

        [Space(8)] [SerializeField] private bool stopAfterFirstHit;
        [SerializeField] private bool detectOnStart = true;
#if UNITY_EDITOR
        [SerializeField] private bool showGizmos = true;
#endif
        [Space(8)] [SerializeField, NotNull] private Transform center;
        [SerializeField, NotNull] private Transform source;
        [SerializeField] private ScriptableEventGameObject detectedEvent;

        private Collider2D[] _hits;
        private HashSet<Collider2D> _hitObjects = new HashSet<Collider2D>();
        private bool _isPlaying;
        private int _frames;

        private void Awake()
        {
            if (detectOnStart) Pulse();
        }

        public override void Pulse()
        {
            _hitObjects.Clear();
            _isPlaying = true;
        }

        public override void Stop() { _isPlaying = false; }

        protected override void FixedTick()
        {
            if (!_isPlaying) return;
            _frames++;
            if (_frames % raycastRate != 0) return;
            _frames = 0;
            Procedure();
        }

        private void Procedure()
        {
            var currentPosition = source.TransformPoint(center.localPosition);
            Raycast(currentPosition);
        }


        private void Raycast(Vector2 center)
        {
            bool hitDetected = false;
            _hits = Physics2D.OverlapCircleAll(center, range, layer);
            foreach (var hit in _hits)
            {
                if (hit != null && hit.transform != source)
                {
                    hitDetected = true;
                    HandleHit(hit);
                }
            }

#if UNITY_EDITOR
            if (showGizmos) DebugEditor.DrawCircle(center, range, 32, hitDetected ? Color.red : Color.cyan, 0.4f);
#endif
        }
        

        private void HandleHit(Collider2D hit)
        {
            if (_hitObjects.Contains(hit)) return;
            _hitObjects.Add(hit);
            if (detectedEvent != null) detectedEvent.Raise(hit.gameObject);
            if (stopAfterFirstHit) Stop();

#if UNITY_EDITOR
            if (showGizmos)
            {
                Debug.DrawRay((Vector2) hit.transform.position + new Vector2(0, 0.2f), Vector2.down * 0.4f, Color.red, 0.6f);
                Debug.DrawRay((Vector2) hit.transform.position + new Vector2(-0.2f, 0), Vector2.right * 0.4f, Color.red, 0.6f);
            }
#endif
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (center != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(center.position, 0.1f);
                
                if (showGizmos)
                {
                    Gizmos.color = new Color(1f, 1f, 1f, 0.5f);
                    Gizmos.DrawWireSphere(center.position, range);
                }
            }
        }
#endif
    }
}