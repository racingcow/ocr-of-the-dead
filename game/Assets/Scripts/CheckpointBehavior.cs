using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class CheckpointBehavior : MonoBehaviour
    {
        public bool cameraWaypoint;
        public bool loops = false;
        public bool triggerEnemies;
        public bool waitForEnemiesToDie;
        public List<Enemy> enemiesToTrigger;

        private void OnTriggerEnter(Collider col)
        {
            if (col.tag != "Player") return;

            if (triggerEnemies)
            {
                foreach (var e in enemiesToTrigger)
                {
                    var ocrword = Words.RemoveRandom();
                    Debug.Log(string.Format("Enemy '{0}' has word '{1}'", e.name, ocrword.Word.Text));
                    e.OcrWord = ocrword;
                    e.enabled = true;
                }
                triggerEnemies = false;
                waitForEnemiesToDie = true;
                CameraMovement.cameraMovement.Stop();
                StartCoroutine(Wait());
            }
            else if (cameraWaypoint)
            {
                CameraMovement.cameraMovement.NextWaypoint(loops);
                if (!loops) cameraWaypoint = false;
            }

            if (loops) StartCoroutine(Wait());
        }

        private void OnTriggerExit()
        {
            if (loops) cameraWaypoint = true;
        }

        private void Update()
        {
            if (!waitForEnemiesToDie) return;
            if (enemiesToTrigger.Any(z => !z.dead)) return;

            waitForEnemiesToDie = false;
            CameraMovement.cameraMovement.NextWaypoint(loops);
            if (!loops) cameraWaypoint = false;
        }

        private IEnumerator Wait()
        {
            yield return new WaitForSeconds(10);
            cameraWaypoint = true;
        }
    }
}
