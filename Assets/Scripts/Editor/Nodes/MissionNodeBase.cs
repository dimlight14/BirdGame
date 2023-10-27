using System;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Birdgame.Editor
{
    public abstract class MissionNodeBase : Node, IEdgeConnectorListener
    {
        protected Action<Node, Port> removePort;

        protected void AddButton(Action onClick, string buttonName)
        {
            var newButton = new Button(onClick);
            newButton.text = buttonName;
            titleButtonContainer.Add(newButton);
        }

        protected Port GeneratePort(Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
        {
            var newPort = InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(string));
            var deleteBtn = new Button(() => removePort(this, newPort))
            {
                text = "X"
            };
            newPort.contentContainer.Add(deleteBtn);
            newPort.AddManipulator(new EdgeConnector<Edge>(this));
            return newPort;
        }

        public void OnDropOutsidePort(Edge edge, Vector2 position)
        {
        }

        public void OnDrop(GraphView graphView, Edge edge)
        {
            var inputNode = edge.input.node as MissionNodeBase;
            var outputNode = edge.output.node as MissionNodeBase;
            inputNode?.UpdateInputPort(edge.input);
            outputNode?.UpdateOutputPort(edge.output);
        }

        public void UpdateInputPort(Port portToUpdate)
        {
            if (portToUpdate.capacity == Port.Capacity.Single)
            {
                var outputNode = portToUpdate.connections.ElementAt(0).output.node as MissionNodeBase;
                portToUpdate.portName = outputNode.GetOutputId(portToUpdate.connections.ElementAt(0).output);
            }
            else
            {
                var portName = "";
                if (portToUpdate.connections.Count() == 1)
                {
                    var output = portToUpdate.connections.ElementAt(0).output.node as MissionNodeBase;
                    portName += output?.GetOutputId(portToUpdate.connections.ElementAt(0).output) + " or Mission without id";
                }
                else if(portToUpdate.connections.Count() > 1)
                {
                    var output = portToUpdate.connections.ElementAt(0).output.node as MissionNodeBase;
                    portName = output?.GetOutputId(portToUpdate.connections.ElementAt(0).output)??"";
                    output = portToUpdate.connections.ElementAt(1).output.node as MissionNodeBase;
                    portName += " or " +output?.GetOutputId(portToUpdate.connections.ElementAt(1).output);
                }

                portToUpdate.portName = portName;
            }
        }

        public void UpdateOutputPort(Port portToUpdate)
        {
            var inputNode = portToUpdate.connections.ElementAt(0).input.node as MissionNodeBase;
            portToUpdate.portName = inputNode.GetInputId();
        }

        public abstract string GetInputId();
        public abstract string GetOutputId(Port port);
        public abstract Port LoadNode(Direction direction, Port.Capacity capacity, string outputMission, string inputMission);
    }
}