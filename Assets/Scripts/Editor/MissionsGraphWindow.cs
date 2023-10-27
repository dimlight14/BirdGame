using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Birdgame.Editor
{
    public class MissionsGraphWindow : EditorWindow
    {
        private MissionsGraphView _graphView;
        private IMGUIContainer _editorContainer;
        private MissionsGraphDataSaver _dataSaver;
        private MissionGraphDataLoader _dataLoader;
        
        [MenuItem("BirdGame/Missions Graph")]
        public static void OpenWindow()
        {
            var window = GetWindow<MissionsGraphWindow>();
            window.titleContent = new GUIContent("Missions Graph");
        }

        private void OnEnable()
        {
            CreateGraph();
            AddToolBar();
            _dataSaver = new MissionsGraphDataSaver();
            _dataLoader = new MissionGraphDataLoader();
        }

        private void AddToolBar()
        {
            var toolBar = new Toolbar();
            var createNodeButton = new Button(() =>
            {
                _graphView.AddSingleMissionNodeToView();
            });
            createNodeButton.text = "Add single mission";
            toolBar.Add(createNodeButton);
            
            var createDoubleNodeButton = new Button(() =>
            {
                _graphView.AddDoubleMissionNodeToView();
            });
            createDoubleNodeButton.text = "Add double mission";
            toolBar.Add(createDoubleNodeButton);

            var saveDataBtn = new Button(() =>
            {
                _dataSaver.SaveGraph("Assets/Settings/MissionsDatabase.asset", _graphView);
            });
            saveDataBtn.text = "Save data";
            toolBar.Add(saveDataBtn);

            var loadDataBtn = new Button(() =>
            {
                _dataLoader.LoadGraph("Assets/Settings/MissionsDatabase.asset", _graphView);
            });
            loadDataBtn.text = "Load data";
            toolBar.Add(loadDataBtn);

            var label = new Label("     Данные миссии отображаются в инспекторе");
            toolBar.Add(label);
            
            rootVisualElement.Add(toolBar);
        }

        private void CreateGraph()
        {
            _graphView = new MissionsGraphView()
            {
                name = "Missions Graph"
            };
            _graphView.StretchToParentSize();
            rootVisualElement.Add(_graphView);
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(_graphView);
        }
    }
}