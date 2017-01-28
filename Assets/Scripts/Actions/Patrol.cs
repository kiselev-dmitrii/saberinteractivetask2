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
    [TaskDescription("Патрулирование по точкам. Как только находится противник возвращает true. При достижении конца обхода возвращает false")]
    public class Patrol : Action {
        public float ViewDistance;
        public float ViewAngle;
        public Transform[] Waypoints;
        public String EnemyTag;
        public SharedTransform Enemy;

        private NavMeshAgent agent;
        private int idx;
        private GameObject[] enemies;
        private bool isStart;

        public override void OnAwake() {
            agent = gameObject.GetComponent<NavMeshAgent>();
        }

        public override void OnStart() {
            idx = 0;
            float minDst = Mathf.Infinity;
            for (int i = 0; i < Waypoints.Length; ++i) {
                float curDst = (transform.position - Waypoints[i].position).magnitude;
                if (curDst < minDst) {
                    minDst = curDst;
                    idx = i;
                }
            }

            enemies = GameObject.FindGameObjectsWithTag(EnemyTag);

            agent.enabled = true;
            agent.destination = Waypoints[idx].position;
            isStart = true;
        }

        public override TaskStatus OnUpdate() {
            foreach (var enemy in enemies) {
                if (IsInFieldOfView(agent.transform, enemy.transform, ViewAngle, ViewDistance)) {
                    Enemy.Value = enemy.transform;
                    return TaskStatus.Success;
                }
            }

            //Считаем позицию до точки назначения. Если она меньше, то переходим на следующую
            if (!agent.pathPending) {
                var thisPosition = transform.position;
                thisPosition.y = agent.destination.y; // ignore y
                if (Vector3.SqrMagnitude(thisPosition - agent.destination) < 1) {
                    if (idx == 0 && !isStart) return TaskStatus.Failure;
                    else {
                        idx = (idx + 1) % Waypoints.Length;
                        agent.destination = Waypoints[idx].position;
                        isStart = false;
                    }
                }
            }

            return TaskStatus.Running;
        }

        public override void OnEnd() {
            agent.enabled = false;
        }

        public override void OnReset() {
            ViewDistance = 0;
            Waypoints = null;
            EnemyTag = "";
        }

        public override void OnDrawGizmos() {
            if (Application.isPlaying) {
                UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, ViewDistance);
            }
        }

        public static bool IsInFieldOfView(Transform agent, Transform target, float fov, float disance) {
            Vector3 v = target.position - agent.position;
            float angle = Vector3.Angle(v, agent.forward);

            if (angle < fov && v.magnitude < disance) {
                RaycastHit hit;
                if (Physics.Raycast(agent.position, v.normalized, out hit)) {
                    if (hit.transform == target) {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}