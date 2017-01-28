using UnityEngine;

namespace Assets.Scripts {
    public class Character : MonoBehaviour {
        public int Health = 100;
        public Gun Gun;

        public void Die() {
            GameObject.Destroy(gameObject);
        }
    }
}
