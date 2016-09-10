using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace Racingcow.OcrOfTheDead.Views
{
    public class WaypointView : View
    {
        public bool cameraWaypoint;
        public bool loops = false;
        public bool triggerEnemies;
        public bool waitForEnemiesToDie;
        public List<EnemyView> enemiesToTrigger;

        public Signal triggerSignal = new Signal();

        private void OnTriggerEnter(Collider col)
        {
            if (col.tag != "Player") return;

            //if (triggerEnemies)
            //{
            //    foreach (var e in enemiesToTrigger)
            //    {
            //        var ocrword = Words.RemoveRandom();
            //        Debug.Log(string.Format("Enemy '{0}' has word '{1}'", e.name, ocrword.Word.Text));
            //        e.OcrWord = ocrword;
            //        //e.enabled = true;
            //    }
            //    triggerEnemies = false;
            //    waitForEnemiesToDie = true;
            //    CameraView.cameraMovement.Stop();
            //    StartCoroutine(Wait());
            //}
            //else if (cameraWaypoint)
            //{
            //    CameraView.cameraMovement.NextWaypoint(loops); //TODO: Remove move when things are working with signal
            //    if (!loops) cameraWaypoint = false;
            //}

            Debug.Log(string.Format("WaypointView {0} sending trigger signal", name));
            triggerSignal.Dispatch();

            //if (loops) StartCoroutine(Wait());
        }

        //private void OnTriggerExit()
        //{
        //    if (loops) cameraWaypoint = true;
        //}

        //private void Update()
        //{
        //    if (!waitForEnemiesToDie) return;
        //    if (enemiesToTrigger.Any(z => !z.dead)) return;

        //    waitForEnemiesToDie = false;
        //    //CameraView.cameraMovement.NextWaypoint(loops);
        //    if (!loops) cameraWaypoint = false;
        //}

        //private IEnumerator Wait()
        //{
        //    yield return new WaitForSeconds(10);
        //    cameraWaypoint = true;
        //}
    }
}
