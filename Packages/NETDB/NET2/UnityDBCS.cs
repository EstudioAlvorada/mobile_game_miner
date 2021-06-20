using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

// [Assets/Reimport iBoxDB.NET2.dll]
using IBoxDB.LocalServer;

public class UnityDBCS : MonoBehaviour
{

	public AutoBox auto = null;

	void Start ()
	{
		if (auto == null) {

			DB.Root (Application.persistentDataPath);

			DB db = new DB (6);
			//load from Resources
			//db = new DB(((TextAsset)(UnityEngine.Resources.Load("db2"))).bytes);

			// two tables(Player,Item) and their keys(ID,Name)
			db.GetConfig ().EnsureTable<Player> ("Player", "Id");

			// set max-length to 20 , default is 32
			db.GetConfig ().EnsureTable<Item> ("Item", "Name(20)");

			{
				// [Optional]
				// if device has small memory & disk
				db.MinConfig ();
				// smaller DB file size
				db.GetConfig ().FileIncSize = 1;
			}

			//for Mobiles, set ReadThread to 1. default is 4 used in Servers.
			db.GetConfig ().ReadStreamCount = 1;
			auto = db.Open ();

		}
		//Transaction
		using (var box = auto.Cube ()) {
			// set " limit 0,1 " will faster
			if (box.Count ("from Item limit 0,1") == 0) {
				// insert player's score to database 
				var player = new Player {
					Name = "Player_" + (int)Time.realtimeSinceStartup,
					Score = DateTime.Now.Second + (int)Time.realtimeSinceStartup + 1,
					Id = box.NewId ()
				};
				box ["Player"].Insert (player);


				//dynamic data, each object has different properties
				var shield = new Item () { Name = "Shield", Position = 1 };
				shield ["attributes"] = new string[] { "earth" };
				box ["Item"].Insert (shield);


				var spear = new Item () { Name = "Spear", Position = 2 };
				spear ["attributes"] = new string[] { "metal", "fire" };
				spear ["attachedSkills"] = new string[] { "dragonFire" };
				box ["Item"].Insert (spear);


				var composedItem = new Item () { Name = "ComposedItem", Position = 3, XP = 0 };
				composedItem ["Source1"] = "Shield";
				composedItem ["Source2"] = "Spear";
				composedItem ["level"] = 0;
				box ["Item"].Insert (composedItem);

			}
			CommitResult cr = box.Commit ();
		}
		DrawToString ();
	}

	void DrawToString ()
	{
		_context = "";
		//SQL-like Query
		foreach (Item item in auto.Select<Item>("from Item order by Position")) {
			string s = DB.ToString (item);
			if (item.Name == "ComposedItem") {
				s += " XP=" + item.XP;
			}
			s += "\r\n\r\n";
			_context += Format (s);
		}
		_context += "Player \r\n";
		foreach (Player player in auto.Select<Player>("from Player where Score >= ? order by Score desc", 0)) {
			_context += player.Name + " Score:" + player.Score + "\r\n";
		}
	}

	private string _context;

	void OnGUI ()
	{
		if (GUI.Button (new Rect (0, 0, Screen.width / 2, 50), "NewScore")) {

			long sequence = auto.NewId (0, 1);
			var player = new Player {
				Name = "Player_" + sequence,
				Score = DateTime.Now.Second + 1,
				Id = auto.NewId ()
			};
			auto.Insert ("Player", player);

			DrawToString ();
		}
		if (GUI.Button (new Rect (Screen.width / 2, 0, Screen.width / 2, 50), "LevelUp")) {

			// use ID to read item from db then update <level> and <experience points> 
			var composedItem = auto.Get<Item> ("Item", "ComposedItem");
			composedItem.XP = (long)(Time.fixedTime * 100);
			composedItem ["level"] = (int)composedItem ["level"] + 1;
			auto.Update ("Item", composedItem);

			DrawToString ();
		}
		String exinfo = IntPtr.Size == 4 ? "32bit " : "64bit ";
		exinfo += (" Path:\r\n" + Application.persistentDataPath);
		GUI.Box (new Rect (0, 50, Screen.width, Screen.height - 50), "\r\n" + _context +
		"\r\n" + exinfo);
	}

	//A Player, Normal class
	public class Player
	{
		public long Id;
		public string Name;
		public int Score;
	}

	// An Item, Dynamic class
	public class Item : Dictionary<string, object>
	{
		public string Name {
			get {
				return (string)base ["Name"];
			}
			set {
				if (value.Length > 20) {
					throw new ArgumentOutOfRangeException ();
				}
				base ["Name"] = value;
			}
		}

		public int Position {
			get {
				return (int)base ["Position"];
			}
			set {
				base ["Position"] = value;
			}
		}
		//encrypt experience points
		public long XP {
			get {
				object ot;
				if (!base.TryGetValue ("_xp", out ot)) {
					return 0;
				}
				string t = ot as string;
				t = t.Replace ("fakeData", "");
				return Int64.Parse (t);
			}
			set {
				var t = "fakeData" + value;
				base ["_xp"] = t;
			}
		}
	}

	string Format (string s)
	{
		int pos = s.IndexOf (',', s.IndexOf (',', 0) + 1);
		if (pos > 0) {
			s = s.Substring (0, pos + 1) + "\r\n" + Format (s.Substring (pos + 1));
		}
		return s;
	}

}