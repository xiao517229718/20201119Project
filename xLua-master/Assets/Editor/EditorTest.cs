using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MyInSpectorEditorTest
{
    public enum TestUseEnum
    {
        类型1,
        类型2,
        类型3,

    }
    [CustomEditor(typeof(MyItem))]
    public class EditorTest : Editor
    {

        private bool _showJsonText = true;
        MyItem myItem;
        private void OnEnable()
        {
            myItem = (MyItem)target;
        }

        public override void OnInspectorGUI()
        {
            _showJsonText = EditorGUILayout.Toggle("显示序列化文本", _showJsonText);
            if (GUILayout.Button("保存数据"))
            {
                GUILayout.TextArea(myItem.content);
                Debug.LogWarning("保存成功");
            }
            myItem.content = EditorGUILayout.TextField("我是测试", myItem.content);
            myItem.testUseEnum = (int)(TestUseEnum)EditorGUILayout.EnumPopup("类型", (TestUseEnum)myItem.testUseEnum);
            myItem.toogle1 = EditorGUILayout.Toggle("toogle1", myItem.toogle1);
            myItem.toogle2 = EditorGUILayout.Toggle("toogle2", myItem.toogle2);
            if (myItem.toogle1)
            {
                Debug.LogWarning(11);

            }
            if (myItem.toogle2) {
                Debug.LogWarning(22);
            }


            //int animType = (int)(TestUseEnum)EditorGUILayout.EnumPopup("类型", (TestUseEnum)(int)myItem.testUseEnum);
            //if (animType != myItem.testUseEnum)
            //{
            //    myItem.testUseEnum = animType;
            //}
            myItem.perfab = EditorGUILayout.ObjectField("GameObject", myItem.perfab,
               typeof(GameObject), true) as GameObject;
            switch ((TestUseEnum)myItem.testUseEnum)
            {
                case TestUseEnum.类型1:
                    break;
                case TestUseEnum.类型2:
                    break;
                case TestUseEnum.类型3:
                    break;
            }
        }
    }
}