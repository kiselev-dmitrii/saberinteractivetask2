using UnityEngine;

namespace Assets.Scripts {
    public class GameController : MonoBehaviour {
        public Character Shoot(Gun gun) {
            var character = gun.Shoot();
            if (character != null) {
                character.Health -= gun.Damage;

                if (character.Health <= 0) {
                    character.Die();
                }
            }
            return character;

        }
    }
}
