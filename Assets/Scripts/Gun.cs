using System.Collections;
using UnityEngine;

namespace Assets.Scripts {
    public class Gun : MonoBehaviour {
        public LineRenderer Trace;

        public Transform GunPoint;
        public int Magazine = 10;
        public float Range = 30;
        public int Damage = 10;

        public bool IsReloading { get; private set; }

        private void Awake() {
            Trace.gameObject.SetActive(false);
        }

        public Character Shoot() {
            if (IsReloading) return null;
            --Magazine;
            var hit = Target();
            if (hit == null) return null;
            StartCoroutine(ShootCoroutine(hit));

            var character = hit.Value.collider.GetComponent<Character>();
            return character;
        }

        public RaycastHit? Target() {
            Vector3 dir = GunPoint.TransformDirection(0, 0, 1);
            var ray = new Ray(GunPoint.position, dir);
            Debug.DrawRay(ray.origin, ray.direction);
            RaycastHit[] hits = Physics.RaycastAll(ray, Range);
            if (hits.Length > 0) return hits[0];
            return null;
        }

        public IEnumerator ShootCoroutine(RaycastHit? hit) {
            IsReloading = true;
            Trace.SetPosition(0, GunPoint.position);
            if (hit != null) {
                Trace.SetPosition(1, hit.Value.point);
            } else {
                Vector3 dir = GunPoint.TransformDirection(0, 0, 1);
                var ray = new Ray(GunPoint.position, dir);
                Trace.SetPosition(1, ray.origin + ray.direction*Range);
            }

            Trace.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            Trace.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.4f);
            IsReloading = false;
        }
    }
}
