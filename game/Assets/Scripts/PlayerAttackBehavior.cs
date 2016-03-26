using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class PlayerAttackBehavior : MonoBehaviour
    {
        public Enemy targetedEnemy;
        private GameObject _inputPanel;
        private InputField _inputField;

        public void TargetNextEnemy()
        {
            var newTargetedEnemy = targetedEnemy;
            if (newTargetedEnemy != null && (newTargetedEnemy.dying || newTargetedEnemy.dead)) newTargetedEnemy = null;

            var waypoint = CameraMovement.waypoint;
            if (waypoint != null)
            {
                var enemies = waypoint.enemiesToTrigger;
                if (enemies != null && enemies.Count > 0 && enemies.Any(e => !e.dying && !e.dead))
                {
                    var idx = targetedEnemy == null ? 0 : enemies.IndexOf(targetedEnemy);
                    var rotCt = 0;
                    while (rotCt < enemies.Count)
                    {
                        idx = ((idx + 1) % enemies.Count);
                        //Debug.Log("idx = " + idx);
                        var curEnemy = enemies[idx];
                        if (!curEnemy.dead && !curEnemy.dying)
                        {
                            newTargetedEnemy = curEnemy;
                            //Debug.Log("Set targeted enemy to idx " + idx);
                            //if (targetedEnemy == null) Debug.Log("Targeted enemy is null!");
                            _inputField.text = string.Empty;
                            break;
                        }
                        rotCt++;
                        //Debug.Log("rotCt = " + rotCt);
                    }
                
                }
            }

            targetedEnemy = newTargetedEnemy;
        }

        // Use this for initialization
        void Start ()
        {
            _inputPanel = GameObject.Find("InputPanel");
            _inputPanel.SetActive(false);
            _inputField = _inputPanel.GetComponentInChildren<InputField>();
        }
	
        // Update is called once per frame
        void Update ()
        {
            if (targetedEnemy == null || Input.GetKeyDown(KeyCode.Tab)) TargetNextEnemy();
            _inputPanel.SetActive(targetedEnemy != null);
            
            if (targetedEnemy == null)
            {
                if (!CameraMovement.cameraMovement.moving) CameraMovement.cameraMovement.NextWaypoint(false);
                return;
            }

            var enemyPos = targetedEnemy.transform.position;

            //project 3d world space coordinates of targeted enemy onto 2d screen space
            if (_inputPanel == null) return;
            var inputPanelTrans = _inputPanel.GetComponent<RectTransform>();
            var panelPos = Camera.main.WorldToScreenPoint(enemyPos);
            inputPanelTrans.position = panelPos;

            var img = _inputPanel.GetComponentInChildren<RawImage>();
            if (targetedEnemy.OcrWord != null)
            {
                Debug.Log(string.Format("Targeting enemy '{0}' with word '{1}'", targetedEnemy.name, targetedEnemy.OcrWord.Word.Text));
                img.texture = targetedEnemy.OcrWord.Snippet;
            }

            //input box must always be visible
            var corners = new Vector3[4];
            inputPanelTrans.GetWorldCorners(corners);
            var height = corners[2].y - corners[0].y;
            var halfHeight = height/2f;
            if (panelPos.y - halfHeight < 0)
            {
                inputPanelTrans.position = new Vector3(panelPos.x, halfHeight);
            }

            _inputField.ActivateInputField();

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                if (WordFilter.Contains(_inputField.text))
                {
                    // show filter graphic on bad word
                    var naughty = GetComponentsInChildren<RawImage>().Single(i => i.name == "Naughty");
                    naughty.GetComponent<Animation>().Play();
                }
                else
                {
                    // kill enemy and send correction to server
                    targetedEnemy.Die(_inputField.text);
                }
            }
        }
    }
}
