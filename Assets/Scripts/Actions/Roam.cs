using System;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Action = BehaviorDesigner.Runtime.Tasks.Action;
#if !(UNITY_4_3 || UNITY_4_4)
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
#endif

namespace Assets.Scripts.Actions {
    [TaskCategory("Custom")]
    [TaskDescription("Бродит по случайным точкам")]
    public class Roam : Action {
        public float WalkRadius = 20.0f;
        private NavMeshAgent agent;

        public override void OnAwake() {
            agent = gameObject.GetComponent<NavMeshAgent>();
        }

        public override void OnStart() {
            agent.destination = GetNextNearbyPoint(agent.transform);
        }

        public override TaskStatus OnUpdate() {
            //Считаем позицию до точки назначения. Если она меньше, то переходим на следующую
            if (!agent.pathPending) {
                Vector2 agentPos = new Vector2(agent.transform.position.x, agent.transform.position.z);
                Vector2 dstPos = new Vector2(agent.destination.x, agent.destination.z);

                if ((agentPos - dstPos).magnitude < 1) {
                    agent.destination = GetNextNearbyPoint(agent.transform);
                }
            }

            return TaskStatus.Running;
        }

        public Vector3 GetNextNearbyPoint(Transform t) {
            Vector2 pointOnDisk = UnityEngine.Random.insideUnitCircle * WalkRadius;
            Vector3 nextPosition = new Vector3(t.position.x + pointOnDisk.x, t.position.y, t.position.z + pointOnDisk.y);
            NavMeshHit hit;
            NavMesh.SamplePosition(nextPosition, out hit, WalkRadius, 1);
            return hit.position;
        }
    }
}
