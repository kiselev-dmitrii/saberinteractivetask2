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
    [TaskDescription("Проверяет, есть ли поблизости враг")]
    public class IsEnemyNearby : Conditional {
        public float ViewDistance;
        public float ViewAngle;
        public String EnemyTag;
        public SharedTransform Enemy;

        private NavMeshAgent agent;
        private GameObject[] enemies;

        public override void OnAwake() {
            agent = gameObject.GetComponent<NavMeshAgent>();
        }

        public override void OnStart() {
            enemies = GameObject.FindGameObjectsWithTag(EnemyTag);
        }

        public override TaskStatus OnUpdate() {
            foreach (var enemy in enemies) {
                if (Patrol.IsInFieldOfView(agent.transform, enemy.transform, ViewAngle, ViewDistance)) {
                    Enemy.Value = enemy.transform;
                    return TaskStatus.Success;
                }
            }
            return TaskStatus.Failure;
        }
    }
}