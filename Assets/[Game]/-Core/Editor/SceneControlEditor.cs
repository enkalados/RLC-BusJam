using LevelDataSystem.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Base.Init
{
	[InitializeOnLoad]
	public static class SceneControlEditor
	{
		#region Variables
		//private static string scenePath = "Assets/[Game]/Scenes/Init.unity";
		static int activeSceneIndex = 0;
		#endregion
		#region Properties 
		#endregion
		#region Editor Methods
		static SceneControlEditor()
		{
			SceneView.duringSceneGui += OnSceneGUI;
		}
		#endregion
		#region My Methods

		private static void OnSceneGUI(SceneView sceneView)
		{
			Handles.BeginGUI();
			GUILayout.BeginArea(new Rect(10, 10, 150, 50));

			GUILayout.BeginHorizontal(EditorStyles.toolbar);


			//if (GUILayout.Button("Start With Init", EditorStyles.toolbarButton))
			//{
			//	GetActiveSceneBuildIndex();
			//	OpenScene(scenePath, true);
			//}
			if (GUILayout.Button("Open Level Editor", EditorStyles.toolbarButton))
			{
				LevelSaveLoadEditorWindow.ShowWindow();
			}
			GUILayout.EndHorizontal();
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(10, 50, 150, 5000));
															  // Build Settings'teki sahneleri listele ve her biri için bir buton ekle
			for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
			{
				GUILayout.BeginHorizontal(EditorStyles.toolbar);
				if (EditorBuildSettings.scenes[i].enabled)
				{
					if (GUILayout.Button($"{System.IO.Path.GetFileNameWithoutExtension(EditorBuildSettings.scenes[i].path)}", GUILayout.Height(20)))
					{
						OpenScene(EditorBuildSettings.scenes[i].path, false);
					}
				}
				GUILayout.EndHorizontal();
			}

			GUILayout.EndArea();
			Handles.EndGUI();
		}
		private static void OpenScene(string path, bool startGame)
		{
			if (!System.IO.File.Exists(path))
			{
				Debug.LogError($"Scene not found at path: {path}");
				return;
			}

			if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
			{
				EditorSceneManager.OpenScene(path);
				Debug.Log($"Opened scene: {path}");
			}

			if (startGame)
				StartGame();
		}
		private static void StartGame()
		{
			if (!EditorApplication.isPlaying)
			{
				EditorApplication.isPlaying = true;
			}
			else
			{
				Debug.LogWarning("Game is already running!");
			}
		}
		private static void GetActiveSceneBuildIndex()
		{
			Scene activeScene = SceneManager.GetActiveScene();

			string activeScenePath = activeScene.path;

			// Build Settings'teki sahneleri döngüyle kontrol et
			for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
			{
				if (EditorBuildSettings.scenes[i].path == activeScenePath)
				{
					activeSceneIndex = i;
				}
			}
			PlayerPrefs.SetInt(Utilities.GlobalVariable.GlobalVariables.OpenedSceneOnEditor, activeSceneIndex);
		}
		#endregion
	}
}