using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EasyQuestSwitch
{
    public class EQS_Watcher : IActiveBuildTargetChanged
    {
        [InitializeOnLoadMethod]
        private static void OnInitialize()
        {
            EditorSceneManager.sceneOpened += OnSceneOpened;
        }

        private static void OnSceneOpened(Scene scene, OpenSceneMode mode)
        {
            EQS_Data data = GameObject.Find("EQS_DATA")?.GetComponent<EQS_Data>();
            data?.OnSceneOpened();
        }

        public int callbackOrder => 1;

        public void OnActiveBuildTargetChanged(BuildTarget previousTarget, BuildTarget newTarget)
        {
            EQS_Data data = GameObject.Find("EQS_DATA")?.GetComponent<EQS_Data>();
            data?.OnChangedBuildTarget(newTarget);
        }
    }
}

