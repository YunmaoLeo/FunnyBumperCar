using System;
using Unity.VisualScripting;
using UnityEngine;
using XNode;

public class StateNode : Node
{
    [Input] public Empty enter;
    [Output] public Empty exit;
    
    public void MoveNext() {
        StateGraph fmGraph = graph as StateGraph;

        if (fmGraph.current != this) {
            Debug.LogWarning("Node isn't active");
            return;
        }

        NodePort exitPort = GetOutputPort("exit");

        if (!exitPort.IsConnected) {
            Debug.LogWarning("Node isn't connected");
            return;
        }

        StateNode node = exitPort.Connection.node as StateNode;
        fmGraph.current = node;
        if (node == null)
        {
            Debug.Log("exit Node port is Null");
        }
    }

    public void OnEnter() {
        StateGraph fmGraph = graph as StateGraph;
        fmGraph.current = this;
        Debug.Log("State Node On Enter");
    }


    [Serializable]
    public class Empty
    {
    }
}