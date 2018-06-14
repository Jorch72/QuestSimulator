using Rondo.Generic.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Gameplay {

    public class CameraController : MonoBehaviourSingleton<CameraController> {

        public Transform target;
        public float followSpeed = 0.1f;
        public float centerLookBias = 0.15f;
        public float centerPositionBias = 0.1f;

        private Vector3 m_StartLookLocation;
        private Vector3 m_LookLocation;
        private Vector3 m_StartLocation;

        private Vector3 m_StartLookLocationTarget;
        private Vector3 m_StartLocationTarget;

        private Coroutine m_SmoothPositionRoutine;
        private Coroutine m_SmoothLookatRoutine;

        private void Start() {
            m_StartLookLocation = target.position;
            m_LookLocation = m_StartLookLocation;
            m_StartLocation = transform.position;

            m_StartLookLocationTarget = m_StartLookLocation;
            m_StartLocationTarget = m_StartLocation;
        }

        void FixedUpdate() {
            Vector3 biasLookLocation = m_StartLookLocation + ((target.position - m_StartLookLocation) * centerLookBias);
            m_LookLocation = Vector3.Lerp(m_LookLocation, biasLookLocation, followSpeed);
            transform.LookAt(m_LookLocation);

            Vector3 biasLocation = m_StartLocation + ((target.position - m_StartLocation) * centerPositionBias);
            transform.position = Vector3.Lerp(transform.position, biasLocation, followSpeed);
        }

        public Vector3 SwitchCameraPosition(Vector3 newPosition) {
            if(m_SmoothPositionRoutine != null) {
                StopCoroutine(m_SmoothPositionRoutine);
            }

            Vector3 prevLocation = m_StartLocationTarget;
            m_SmoothPositionRoutine = StartCoroutine(SmoothCameraPositionTransition(newPosition));
            return prevLocation;
        }

        public Vector3 SwitchCameraLookAt(Vector3 newPosition) {
            if (m_SmoothLookatRoutine != null) {
                StopCoroutine(m_SmoothLookatRoutine);
            }

            Vector3 prevPosition = m_StartLookLocationTarget;
            m_SmoothLookatRoutine = StartCoroutine(SmoothCameraLookatTransition(newPosition));
            return prevPosition;
        }

        private IEnumerator SmoothCameraPositionTransition(Vector3 nextPosition) {
            m_StartLocationTarget = nextPosition;
            while(Vector3.Distance(m_StartLocation, nextPosition) >= 0.01f) {
                m_StartLocation = Vector3.Lerp(m_StartLocation, m_StartLocationTarget, 0.1f);
                yield return null;
            }
            m_StartLocation = nextPosition;
            m_SmoothPositionRoutine = null;
        }

        private IEnumerator SmoothCameraLookatTransition(Vector3 nextPosition) {
            m_StartLookLocationTarget = nextPosition;
            while (Vector3.Distance(m_StartLookLocation, nextPosition) >= 0.01f) {
                m_StartLookLocation = Vector3.Lerp(m_StartLookLocationTarget, nextPosition, 0.05f);
                yield return null;
            }
            m_StartLookLocation = nextPosition;
            m_SmoothPositionRoutine = null;
        }
    }

}