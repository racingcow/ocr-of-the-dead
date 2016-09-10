using System;
using System.Collections.Generic;
using strange.extensions.mediation.impl;
using UnityEngine;
using System.Linq;

namespace Racingcow.OcrOfTheDead.Views
{
    //TODO: NEED MEDIATOR / REFACTOR IN HERE
    public class CameraView : View
    {
        public static CameraView cameraMovement;
        private WaypointView _waypoint;
        //public List<Transform> waypoints;
        public float speed;
        public float rotateSpeed;
        public bool moving = true;

        //private void UpdateWayPoint()
        //{
        //    var waypointTrans = waypoints[0];
        //    var behavior = waypointTrans.GetComponentInChildren<WaypointView>();
        //    waypoint = behavior;
        //    //Debug.Log("Waypoint name = " + waypoint.name);
        //}

        public void Start()
        {
            moving = true;
        }

        public void Stop()
        {
            moving = false;
        }

        public void SetNextWaypoint(string nextWaypointName)
        {
            _waypoint = FindObjectsOfType<WaypointView>().Where(x => x.name == nextWaypointName).FirstOrDefault();
            if (_waypoint == null) Stop();
        }
    
        //public void NextWaypoint(bool loops)
        //{
        //    //Debug.Log("NextWaypoint!");
        //    if (loops) waypoints.Add(waypoints[0]);
        //    waypoints.RemoveAt(0);
        //    UpdateWayPoint();
        //    moving = true;
        //}

        // Use this for initialization
        void Awake()
        {
            cameraMovement = this;

            //waypoints[0].LookAt(transform.position);
            //for (var x = 1; x < waypoints.Count; x++)
            //{
            //    waypoints[x].LookAt(waypoints[x-1].transform.position);
            //}
            //UpdateWayPoint();
        }

        // Update is called once per frame
        void Update()
        {
            //if (waypoints == null) throw new NullReferenceException("waypoints is null. Add some through designer.");
            //if (waypoints.Count == 0) throw new NullReferenceException("No waypoints exist. Add some through designer.");
            if (!moving) return;

            var targetDirection = _waypoint.transform.position - transform.position;
            var newDirection = Vector3.RotateTowards(transform.forward, targetDirection, Time.deltaTime * rotateSpeed, 0);
            newDirection.y = 0;
            transform.rotation = Quaternion.LookRotation(newDirection);
            var normalized = targetDirection.normalized;
            normalized.y = 0;
            transform.position += normalized * speed * Time.deltaTime;

        }
    }
}