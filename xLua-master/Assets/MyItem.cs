using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MyInSpectorEditorTest
{
    public class MyItem : MonoBehaviour
    {
        public UnityAction<string> myAction;
        public UnityEvent myEvent;

        // Start is called before the first frame update
        public GameObject perfab = null;
        public string content;
        public int testUseEnum = 0;
        public bool toogle1 = false;
        public bool toogle2 = false;
        private int cunt = 0;

        void Start()
        {
            Application.logMessageReceived += (string conditib, string contruct, LogType logType) =>
            {
                if (cunt < 100)
                {
                    Debug.Log(logType);
                    Debug.Log(conditib);
                    Debug.Log(contruct);
                }

            };
            Debug.LogWarning("死循环");
        }

        // Update is called once per frame
        void Update()
        {

        }

    }
}
