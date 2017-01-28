using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Assets.Scripts.Actions {
    [TaskCategory("Custom")]
    [TaskDescription("Проверяет, что трансформация находится к агенту ближе чем на Distance")]
    public class IsNearby : Conditional {
        public SharedFloat Distance;
        public SharedTransform Target;

        public override TaskStatus OnUpdate() {
            float dst = (Target.Value.position - transform.position).magnitude; 
            if (dst <= Distance.Value) return TaskStatus.Success;
            else return TaskStatus.Failure;
        }
    }
}
