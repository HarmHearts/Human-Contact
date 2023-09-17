#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using HC.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using Sirenix.Utilities;

namespace HC.Middleware
{
    public class BodyPlanEditorWindow : OdinEditorWindow
    {
        [UnityEditor.MenuItem("Tools/Body Pose")]
        private static void OpenWindow()
        {
            GetWindow<BodyPlanEditorWindow>().Show();
        }

        public BodyPlan plan;
    }
}


#endif