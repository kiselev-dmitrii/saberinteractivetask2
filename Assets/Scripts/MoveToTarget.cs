using UnityEngine;

namespace Assets.Scripts {
    public class MoveToTarget : MonoBehaviour {
        public Transform Target;
        private NavMeshAgent agent;

        protected void Awake() {
            agent = GetComponent<NavMeshAgent>();
        }
	
        protected void Update () {
            agent.destination = Target.position;
        }
    }
}
