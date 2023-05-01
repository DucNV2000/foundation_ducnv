﻿using System.Collections;
using UnityEngine;

namespace Pancake.Spring
{
    [EditorIcon("script_local_spring")]
    public class SpringLocalPosition3DComponent : BaseSpringComponent, ISpringTo<Vector3>, INudgeable<Vector3>
    {
        private SpringVector3 _spring;
        private CoroutineHandle _handle;
        private readonly WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();

        private void Awake()
        {
            var position = transform.localPosition;
            _spring = new SpringVector3() {StartValue = position, EndValue = position, Damping = damping, Stiffness = stiffness};
        }

        public void SpringTo(Vector3 target)
        {
            if (_handle is {IsDone: false}) StopCoroutine(_handle);
            CheckInspectorChanges();
            this.RunCoroutine(IeSpringToTarget(target));
        }

        public void Nudge(Vector3 value)
        {
            CheckInspectorChanges();
            if (Math.Approximately(_spring.CurrentVelocity.sqrMagnitude, 0))
            {
                if (_handle is {IsDone: false}) StopCoroutine(_handle);
                _handle = this.RunCoroutine(HandleNudge(value));
            }
            else
            {
                _spring.UpdateEndValue(_spring.EndValue, _spring.CurrentVelocity + value);
            }
        }

        private IEnumerator IeSpringToTarget(Vector3 target)
        {
            if (Math.Approximately(_spring.CurrentVelocity.sqrMagnitude, 0))
            {
                _spring.Reset();
                _spring.StartValue = transform.localPosition;
                _spring.EndValue = target;
            }
            else
            {
                _spring.UpdateEndValue(target, _spring.CurrentVelocity);
            }

            while (!Math.Approximately(Vector3.SqrMagnitude(transform.localPosition - target), 0))
            {
                transform.localPosition = _spring.Evaluate(Time.deltaTime);

                yield return _waitForEndOfFrame;
            }

            _spring.Reset();
        }

        private IEnumerator HandleNudge(Vector3 value)
        {
            _spring.Reset();
            var position = transform.localPosition;
            _spring.StartValue = position;
            _spring.EndValue = position;
            _spring.InitialVelocity = value;
            transform.localPosition = _spring.Evaluate(Time.deltaTime);

            while (!Math.Approximately(0, Vector3.SqrMagnitude(position - transform.localPosition)))
            {
                transform.localPosition = _spring.Evaluate(Time.deltaTime);

                yield return _waitForEndOfFrame;
            }

            _spring.Reset();
        }

        private void CheckInspectorChanges()
        {
            _spring.Damping = damping;
            _spring.Stiffness = stiffness;
        }
    }
}