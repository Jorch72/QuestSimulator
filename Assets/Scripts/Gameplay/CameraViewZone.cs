using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Gameplay {

    [RequireComponent(typeof(BoxCollider))]
    public class CameraViewZone : MonoBehaviour {

        public Transform cameraTargetPosition;
        private Vector3 m_PreviousCameraPosition;
        private Vector3 m_PreviousCameraLookAtPosition;

        void OnTriggerEnter(Collider other) {
            if (other.GetComponent<PlayerController>() == null) return;
            m_PreviousCameraPosition = CameraController.Instance.SwitchCameraPosition(cameraTargetPosition.position);
            m_PreviousCameraLookAtPosition = CameraController.Instance.SwitchCameraLookAt(transform.position);
        }

        void OnTriggerExit(Collider other) {
            if (other.GetComponent<PlayerController>() == null) return;
            CameraController.Instance.SwitchCameraPosition(m_PreviousCameraPosition);
            CameraController.Instance.SwitchCameraLookAt(m_PreviousCameraLookAtPosition);
        }

    }

}