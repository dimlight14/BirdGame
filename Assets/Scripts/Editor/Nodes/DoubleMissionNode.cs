using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Birdgame.Editor
{
    public class DoubleMissionNode : MissionNodeBase
    {
        
        private readonly Label _missionBUnlocksLabel;
        private readonly TextField _missionAIdField;
        private readonly TextField _missionBIdField;
        private readonly DoubleMissionNodeData _nodeData;
        public DoubleMissionNodeData NodeData => _nodeData;

        public DoubleMissionNode(Action<Node, Port> removePort)
        {
            base.removePort = removePort;
            _nodeData = ScriptableObject.CreateInstance<DoubleMissionNodeData>();

            AddButton(() => AddPort(Direction.Input, Port.Capacity.Multi), "+ either/or input");
            AddButton(() => AddPort(Direction.Input, Port.Capacity.Single), "+ input");
            AddButton(() => AddPort(Direction.Output, Port.Capacity.Single, true), "+ A output");
            AddButton(() => AddPort(Direction.Output, Port.Capacity.Single, false), "+ B output");

            inputContainer.Add(new Label("Both are unlocked by:"));
            outputContainer.Add(new Label("Mission A unlocks:"));
            _missionBUnlocksLabel = new Label("Mission B unlocks:");
            outputContainer.Add(_missionBUnlocksLabel);

            _missionAIdField = new TextField("Mission A ID");
            _missionAIdField.RegisterValueChangedCallback(evt =>
            {
                SetId(evt.newValue, true);
            });
            mainContainer.Add(_missionAIdField);
            _missionBIdField = new TextField("Mission B ID");
            _missionBIdField.RegisterValueChangedCallback(evt =>
            {
                SetId(evt.newValue, false);
            });
            mainContainer.Add(_missionBIdField);

            RefreshExpandedState();
            RefreshPorts();
        }

        public override Port LoadNode(Direction direction, Port.Capacity capacity, string outputMission, string inputMission)
        {
            var newPort = GeneratePort(direction, capacity);
            if (direction == Direction.Output)
            {
                var missionAPort = outputMission == _nodeData.MissionAId;
                outputContainer.Insert(missionAPort ? 1 : outputContainer.childCount, newPort);
            }
            else
            {
                inputContainer.Add(newPort);
            }

            newPort.portName = direction == Direction.Input ? outputMission : inputMission;
            return newPort;
        }

        private void AddPort(Direction direction, Port.Capacity capacity, bool missionAPort = true)
        {
            var newPort = GeneratePort(direction, capacity);

            if (direction == Direction.Output)
            {
                newPort.portName = "output";
                outputContainer.Insert(missionAPort ? 1 : outputContainer.childCount, newPort);
            }
            else
            {
                newPort.portName = capacity == Port.Capacity.Single ? "input" : "either/or input";
                inputContainer.Add(newPort);
            }

            RefreshPorts();
            RefreshExpandedState();
        }

        public void SetId(string newId, bool missionA)
        {
            if (missionA)
            {
                _missionAIdField.SetValueWithoutNotify(newId);
                _nodeData.MissionAId = newId;
            }
            else
            {
                _missionBIdField.SetValueWithoutNotify(newId);
                _nodeData.MissionBId = newId;
            }
            title = GetInputId();
        }

        public override void OnUnselected()
        {
            base.OnUnselected();
            Selection.activeObject = null;
        }

        public override void OnSelected()
        {
            base.OnSelected();

            Selection.SetActiveObjectWithContext(_nodeData, _nodeData);
        }

        public override string GetInputId()
        {
            return _nodeData.MissionAId + "/" + _nodeData.MissionBId;
        }

        public override string GetOutputId(Port port)
        {
            var bLabelIndex = outputContainer.IndexOf(_missionBUnlocksLabel);
            var returnString = outputContainer.IndexOf(port) > bLabelIndex ? _nodeData.MissionBId : _nodeData.MissionAId;
            if (returnString == "")
            {
                returnString = "Mission without Id";
            }
            return returnString;
        }
    }
}