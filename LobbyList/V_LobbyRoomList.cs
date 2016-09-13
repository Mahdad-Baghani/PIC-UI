using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum TypesOfData
{
	String, Int, PingType, Bool
}
public enum PingType
{
	WEAK = 170,
	MEDIUM = 100,
	STRONG = 50
}
public class V_LobbyRoomList : MonoBehaviour 
{
	// fields
	 // tweak all these serialized fields form the inspector
	[SerializeField] GameObject IDBtnPrfb, RoomNameBtnPrfb, GameModeBtnPrfb, MapBtnPrfb, PlayersBtnPrfb, PassBtnPrfb, RoomStatusBtnPrfb, PingBtnPrfb;
	[SerializeField] GameObject RowPrfb;
	[SerializeField] List<V_Header> headers; // we needed the type and the flexibility, so list instead of simple array or ArrayList
	[SerializeField] ArrayList rows = new ArrayList(); // still no sign of need to substitute rows ArrayList wiht a List

	// Methods
	void Awake()
	{
		FindHeaders();
		Test_FindRows();
	}
	public void Sort(int column, bool sortAscending )
	{
		// cached ref to the dataType in a sepcicfic header of the table
		TypesOfData tmpType = headers[column].dataType;
		ArrayList columnValues = new ArrayList();

		if (tmpType == TypesOfData.String || tmpType == TypesOfData.Bool || tmpType == TypesOfData.PingType)
		{
			print("sorting based on string comparison");
			foreach (V_Row row in rows)
			{
				columnValues.Add(row.cells[column].value);
			}
		}
		else if (tmpType == TypesOfData.Int)
		{
			print("sorting based on int comparison");
			int tmpInt = -1; // a temp int to check for column, and in some cases, errors
			foreach (V_Row row in rows)
			{
				if (int.TryParse(row.cells[column].value, out tmpInt))
				{
					columnValues.Add(tmpInt);
				}
			}
		}
		else
		{
			throw new System.Exception("V_LobbyRoomList: Sort(): header does not have a specified dataType.");
		}
		// we have to sort it one time, anyways!!!!,cause when try to revese a sort, we have to have a 'sort' at hand!!
		
		columnValues.Sort();
		if (!sortAscending)
		{
			columnValues.Reverse();
		}
		// UpdateRowsAccordingToNewSort()!!
		foreach (V_Row row in rows)
		{
			for (int i = 0; i < columnValues.Count; i++)
			{
				// hopefully this way we just cast that to string to be able to compare it
				if (columnValues[i] != null && row.cells[column].value == columnValues[i].ToString())
				{
					row.gameObject.transform.SetSiblingIndex(i);
					print(row.gameObject.name + row.transform.GetSiblingIndex());
					columnValues[i] = null;
				}
			}
		}
	}

	public void AddRow(KeyValuePair<string, string>[] rowElements)
	{
		GameObject newRow = Instantiate(RowPrfb) as GameObject;
		V_Row refOfTheRow = newRow.GetComponent<V_Row>();

		// int tmpInt = 0; // to iterate through row.cells from V_Row class
		foreach (KeyValuePair<string, string> element in rowElements)
		{
			switch (element.Key)			
			{
				case "ID":
				refOfTheRow.cells[0].value = element.Value;
				break;

				case "roomName":
				refOfTheRow.cells[1].value = element.Value;
				break;

				case "gameMode":
				refOfTheRow.cells[2].value = element.Value;
				break;

				case "map":
				refOfTheRow.cells[3].value = element.Value;
				break;

				case "players":
				refOfTheRow.cells[4].value = element.Value;
				break;

				case "roomStatus":
				refOfTheRow.cells[5].value = element.Value;
				break;

				case "ping":
				refOfTheRow.cells[6].value = element.Value;
				break;

				default:
				throw new System.Exception("Error in parsing data from the server");
			}
		}
		rows.Add(refOfTheRow);
	}
	void FindHeaders()
	{
		foreach (V_Header header in transform.GetComponentsInChildren<V_Header>())
		{
			headers.Add(header);
		}
	}
	void Test_FindRows()
	{
		foreach (V_Row row in transform.GetComponentsInChildren<V_Row>())
		{
			rows.Add(row);
		}
	}
 
}
