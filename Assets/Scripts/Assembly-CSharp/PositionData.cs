using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PositionData
{
	public PositionPlace place;

	public Transform centerPosition;

	public List<Transform> points;

	public PositionType type;

	public bool needSchoolOpened;
}
