using System;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Action = BehaviorDesigner.Runtime.Tasks.Action;
#if !(UNITY_4_3 || UNITY_4_4)
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
#endif

namespace Assets.Scripts {
    [TaskCategory("Custom")]
    [TaskDescription("Убегание от Target")]
    public class RunOut : Action {
        public SharedTransform Target;
        public float ThresholdDistance;
        private NavMeshAgent agent;

        public override void OnAwake() {
            agent = gameObject.GetComponent<NavMeshAgent>();
        }

        public override void OnStart() {
            agent.enabled = true;
        }

        public override TaskStatus OnUpdate() {
            if (Target.Value == null) return TaskStatus.Success;

            Vector3 vec = Target.Value.position - agent.transform.position;
            float dst = vec.magnitude;

            if (dst > ThresholdDistance) {
                return TaskStatus.Success;
            }

            agent.destination = agent.transform.position - vec.normalized;

            return TaskStatus.Running;
        }
    }
}