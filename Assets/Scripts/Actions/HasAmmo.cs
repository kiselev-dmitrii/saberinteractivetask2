using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Scripts.Actions {
    [TaskCategory("Custom")]
    [TaskDescription("Проверяет, если ли у персонажа аммуниция")]
    public class HasAmmo : Conditional {
        private Character character;

        public override void OnStart() {
            character = gameObject.GetComponent<Character>();
        }

        public override TaskStatus OnUpdate() {
            if (character.Gun.Magazine > 0) {
                return TaskStatus.Success;
            } else {
                return TaskStatus.Failure;    
            }
        }
    }
}
