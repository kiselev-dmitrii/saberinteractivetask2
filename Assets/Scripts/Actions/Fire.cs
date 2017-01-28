using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
#if !(UNITY_4_3 || UNITY_4_4)
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
#endif

namespace Assets.Scripts.Actions {
    [TaskCategory("Custom")]
    [TaskDescription("Стреляет в пртивника target")]
    public class Fire : Action {
        public SharedTransform Target;
        
        private Character character;
        private GameController controller;

        public override void OnAwake() {
            character = gameObject.GetComponent<Character>();
            controller = MonoBehaviour.FindObjectOfType<GameController>();
        }

        public override TaskStatus OnUpdate() {
            if (character.Gun.IsReloading) return TaskStatus.Running;

            if (character.Gun.Magazine <= 0) return TaskStatus.Failure;

            character.transform.LookAt(Target.Value);
            character.Gun.transform.LookAt(Target.Value);
            var victim = controller.Shoot(character.Gun);
            if (victim == null) return TaskStatus.Failure;
            else return TaskStatus.Success;
        }
    }
}
