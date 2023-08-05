using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class TestNode : Node
{

	[Input] public float moveDistance = 10f;
	

	public override object GetValue(NodePort port) {
		return null; // Replace this
	}
}