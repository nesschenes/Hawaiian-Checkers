using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Konane.Builder
{
    public class Builder
    {
        [MenuItem("Tools/Build/All")]
        public static void BuildAll()
        {
            BuildWindows();
            BuildAndroid();
        }

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

        static void BuildViaCommandLine()
        {
            ParseCommandLine();

            PlayerSettings.bundleVersion = Version;
            var destinationPath = Path.Combine(DestinationPath, PlayerSettings.productName);
            destinationPath += GetExtension();

            BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, destinationPath, EditorUserBuildSettings.activeBuildTarget, BuildOptions.None);
        }

        static string Version;
        static string DestinationPath;
        static void ParseCommandLine()
        {
            Dictionary<string, Action<string>> cmdActions = new Dictionary<string, Action<string>>
            {
                {
                    "--buildingVersion", delegate(string argument)
                    {
                        Version = argument;
                    }
                },
                {
                    "--buildingFolder", delegate(string argument)
                    {
                        DestinationPath = argument;
                    }
                }
            };

            Action<string> actionCache;
            var cmdArguments = Environment.GetCommandLineArgs();
            for (var count = 0; count < cmdArguments.Length; count++)
            {
                if (cmdActions.ContainsKey(cmdArguments[count]))
                {
                    actionCache = cmdActions[cmdArguments[count]];
                    actionCache(cmdArguments[count + 1]);
                }
            }

            if (string.IsNullOrEmpty(Version))
            {
                Version = Application.version;
            }

            if (string.IsNullOrEmpty(DestinationPath))
            {
                DestinationPath = Path.GetDirectoryName(Application.dataPath);
            }
        }

        static string GetExtension()
        {
            var extension = string.Empty;
            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    extension = ".exe";
                    break;
                case BuildTarget.Android:
                    extension = ".apk";
                    break;
            }

            return extension;
        }
    }
}