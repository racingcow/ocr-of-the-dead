using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace Racingcow.OcrOfTheDead.Views
{
    public class ReticleView : View
    {
        private EnemyView _targetedEnemy;
        private GameObject _inputPanel;
        private InputField _inputField;
        private RawImage _img;

        public Signal<bool> tabKeySignal = new Signal<bool>();
        public Signal enterKeySignal = new Signal();

        public void SetImage(byte[] bytes, int width, int height)
        {
            var tex = new Texture2D(width, height);
            if (bytes != null) tex.LoadImage(bytes);
            _img.texture = tex;
        }

        public void ClearTextbox()
        {
            _inputField.text = string.Empty;
        }

        public void MoveToEnemy(string enemyName)
        {
            _targetedEnemy = FindObjectsOfType<EnemyView>().FirstOrDefault(e => e.name == enemyName);
        }

        //public void TargetNextEnemy()
        //{
        //    var newTargetedEnemy = _targetedEnemy;
        //    if (newTargetedEnemy != null && (newTargetedEnemy.dying || newTargetedEnemy.dead)) newTargetedEnemy = null;

        //    var waypoint = CameraView.waypoint;
        //    if (waypoint != null)
        //    {
        //        var enemies = waypoint.enemiesToTrigger;
        //        if (enemies != null && enemies.Count > 0 && enemies.Any(e => !e.dying && !e.dead))
        //        {
        //            var idx = _targetedEnemy == null ? 0 : enemies.IndexOf(_targetedEnemy);
        //            var rotCt = 0;
        //            while (rotCt < enemies.Count)
        //            {
        //                idx = ((idx + 1) % enemies.Count);
        //                //Debug.Log("idx = " + idx);
        //                var curEnemy = enemies[idx];
        //                if (!curEnemy.dead && !curEnemy.dying)
        //                {
        //                    newTargetedEnemy = curEnemy;
        //                    //Debug.Log("Set targeted enemy to idx " + idx);
        //                    //if (_targetedEnemy == null) Debug.Log("Targeted enemy is null!");
        //                    _inputField.text = string.Empty;
        //                    break;
        //                }
        //                rotCt++;
        //                //Debug.Log("rotCt = " + rotCt);
        //            }

        //        }
        //    }

        //    _targetedEnemy = newTargetedEnemy;
        //}

        protected override void Start()
        {
            _inputPanel = GameObject.Find("InputPanel");
            _inputPanel.SetActive(false);
            _inputField = _inputPanel.GetComponentInChildren<InputField>();
            _img = _inputPanel.GetComponentInChildren<RawImage>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                Debug.Log("ReticleView raising tabKeySignal");
                tabKeySignal.Dispatch(Input.GetKey(KeyCode.LeftShift));
                return;
            }

            _inputPanel.SetActive(_targetedEnemy != null);
            if (_targetedEnemy == null) return;

            var enemyPos = _targetedEnemy.transform.position;

            //project 3d world space coordinates of targeted enemy onto 2d screen space
            var inputPanelTrans = _inputPanel.GetComponent<RectTransform>();
            var panelPos = Camera.main.WorldToScreenPoint(enemyPos);
            inputPanelTrans.position = panelPos;

            //input box must always be visible
            var corners = new Vector3[4];
            inputPanelTrans.GetWorldCorners(corners);
            var height = corners[2].y - corners[0].y;
            var halfHeight = height / 2f;
            if (panelPos.y - halfHeight < 0)
            {
                inputPanelTrans.position = new Vector3(panelPos.x, halfHeight);
            }

            _inputField.ActivateInputField();

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                enterKeySignal.Dispatch();
                //if (WordFilter.Contains(_inputField.text))
                //{
                //    // show filter graphic on bad word
                //    var naughty = GetComponentsInChildren<RawImage>().Single(i => i.name == "Naughty");
                //    naughty.GetComponent<Animation>().Play();
                //}
                //else
                //{
                //    // kill enemy and send correction to server
                //    _targetedEnemy.Die(_inputField.text);
                //}
            }
        }

        // Update is called once per frame
        //void Update ()
        //{
        //    if (Input.GetKeyDown(KeyCode.Tab))
        //    {
        //        Debug.Log("ReticleView raising tabKeySignal");
        //        tabKeySignal.Dispatch(Input.GetKey(KeyCode.LeftShift));
        //    }

        //    //if (_targetedEnemy == null || Input.GetKeyDown(KeyCode.Tab)) TargetNextEnemy();
        //    _inputPanel.SetActive(_targetedEnemy != null);

        //    //if (_targetedEnemy == null)
        //    //{
        //    //    if (!CameraView.cameraMovement.moving) CameraView.cameraMovement.NextWaypoint(false);
        //    //    return;
        //    //}

        //    var enemyPos = _targetedEnemy.transform.position;

        //    //project 3d world space coordinates of targeted enemy onto 2d screen space
        //    if (_inputPanel == null) return;
        //    var inputPanelTrans = _inputPanel.GetComponent<RectTransform>();
        //    var panelPos = Camera.main.WorldToScreenPoint(enemyPos);
        //    inputPanelTrans.position = panelPos;

        //    var img = _inputPanel.GetComponentInChildren<RawImage>();
        //    if (_targetedEnemy.OcrWord != null)
        //    {
        //        Debug.Log(string.Format("Targeting enemy '{0}' with word '{1}'", _targetedEnemy.name, _targetedEnemy.OcrWord.Word.Text));
        //        img.texture = _targetedEnemy.OcrWord.Snippet;
        //    }

        //    //input box must always be visible
        //    var corners = new Vector3[4];
        //    inputPanelTrans.GetWorldCorners(corners);
        //    var height = corners[2].y - corners[0].y;
        //    var halfHeight = height/2f;
        //    if (panelPos.y - halfHeight < 0)
        //    {
        //        inputPanelTrans.position = new Vector3(panelPos.x, halfHeight);
        //    }

        //    _inputField.ActivateInputField();

        //    if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        //    {
        //        enterKeySignal.Dispatch();

        //        if (WordFilter.Contains(_inputField.text))
        //        {
        //            // show filter graphic on bad word
        //            var naughty = GetComponentsInChildren<RawImage>().Single(i => i.name == "Naughty");
        //            naughty.GetComponent<Animation>().Play();
        //        }
        //        else
        //        {
        //            // kill enemy and send correction to server
        //            _targetedEnemy.Die(_inputField.text);
        //        }
        //    }
        //}
        public void Hide()
        {
            _inputPanel.SetActive(false);
        }

        public void Show()
        {
            _inputPanel.SetActive(true);
        }
    }
}
