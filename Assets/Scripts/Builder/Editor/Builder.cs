using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Konane.Builder
{
    public class Builder
    {
        [MenuItem("Tools/Build/Windows")]
        public static void BuildWindows()
        {
            Debug.Log("Start building windows exe");

            var buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = EditorBuildSettings.scenes.Select(s => s.path).ToArray();
            buildPlayerOptions.locationPathName = "Build/Windows/Konane.exe";
            buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
            buildPlayerOptions.options = BuildOptions.None;

            var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            var summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log("[Windows] Build succeeded: " + summary.totalSize + " bytes");
            }

            if (summary.result == BuildResult.Failed)
            {
                Debug.Log("[Windows] Build failed");
            }
        }

        [MenuItem("Tools/Build/Android")]
        public static void BuildAndroid()
        {
            Debug.Log("Start building android apk");

            var buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = EditorBuildSettings.scenes.Select(s => s.path).ToArray();
            buildPlayerOptions.locationPathName = "Build/Android/Konane.apk";
            buildPlayerOptions.target = BuildTarget.Android;
            buildPlayerOptions.options = BuildOptions.None;

            var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            var summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log("[Android] Build succeeded: " + summary.totalSize + " bytes");
            }

            if (summary.result == BuildResult.Failed)
            {
                Debug.Log("[Android] Build failed");
            }
        }
    }
}