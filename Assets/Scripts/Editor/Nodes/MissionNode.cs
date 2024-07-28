using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Birdgame.Editor
{
    public class MissionNode : MissionNodeBase
    {
        private readonly SingleMissionNodeData _nodeData;
        private readonly TextField _idTextField;
        public SingleMissionNodeData NodeData => _nodeData;

        public MissionNode(Action<Node, Port> removePortAction)
        {
            base.removePortAction = removePortAction;
            _nodeData = ScriptableObject.CreateInstance<SingleMissionNodeData>();

            AddButton(() => AddPort(Direction.Input, Port.Capacity.Multi), "+ either/or input");
            AddButton(() => AddPort(Direction.Input, Port.Capacity.Single), "+ input");
            AddButton(() => AddPort(Direction.Output, Port.Capacity.Single), "+ output");

            inputContainer.Add(new Label("Mission is unlocked by:"));
            outputContainer.Add(new Label("Mission unlocks:"));

            _idTextField = new TextField("Mission ID");
            _idTextField.RegisterValueChangedCallback(evt =>
            {
                SetId(evt.newValue);
            });
            mainContainer.Add(_idTextField);

            RefreshExpandedState();
            RefreshPorts();
        }

        public override string GetInputId()
        {
            return _nodeData.Id is not null or "" ? _nodeData.Id : "Mission without Id";
        }

        public override string GetOutputId(Port port)
        {
            return GetInputId();
        }

        public override Port LoadNode(Direction direction, Port.Capacity capacity, string outputMission, string inputMission)
        {
            var newPort = GeneratePort(direction, capacity);
            if (direction == Direction.Output)
            {
                outputContainer.Add(newPort);
            }
            else
            {
                inputContainer.Add(newPort);
            }

            newPort.portName = direction == Direction.Input ? outputMission : inputMission;
            return newPort;
        }

        public void SetId(string newId)
        {
            _idTextField.SetValueWithoutNotify(newId);
            title = newId;
            _nodeData.Id = newId;
            PropagateIdChangeToConnections();
        }

        private void PropagateIdChangeToConnections()
        {
            foreach (var visualElement in outputContainer.Children())
            {
                if (visualElement is not Port port)
                {
                    continue;
                }

                if (!port.connected)
                {
                    continue;
                }

                var connectedInputPort = port.connections.First().input;
                var connectedInputNode = connectedInputPort.node as MissionNodeBase;
                connectedInputNode?.UpdateInputPort(connectedInputPort);
            }

            foreach (var visualElement in inputContainer.Children())
            {
                if (visualElement is not Port port)
                {
                    continue;
                }

                if (!port.connected)
                {
                    continue;
                }

                foreach (var connection in port.connections)
                {
                    var connectedOutputPort = connection.output;
                    var connectedOutputNode = connectedOutputPort.node as MissionNodeBase;
                    connectedOutputNode?.UpdateOutputPort(connectedOutputPort);
                }
            }
        }

        private void AddPort(Direction direction, Port.Capacity capacity)
        {
            var newPort = GeneratePort(direction, capacity);

            if (direction == Direction.Output)
            {
                newPort.portName = "output";
                outputContainer.Add(newPort);
            }
            else
            {
                newPort.portName = capacity == Port.Capacity.Single ? "input" : "either/or input";
                inputContainer.Add(newPort);
            }

            RefreshPorts();
            RefreshExpandedState();
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
    }
}