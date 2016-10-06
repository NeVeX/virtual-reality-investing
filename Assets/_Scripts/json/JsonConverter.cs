
using System.Reflection;
using System.Collections.Generic;
using System;
using external.JSON;

namespace Json
{
	public class JsonConverter
	{
		public static T ConvertToObject<T>(string json) where T : new()
		{
			JSONObject jo = new JSONObject (json);
			T t = new T ();
			if (!jo.IsArray) {
				t = ExtractObject<T>(jo);
			}
			return t;
		}

		public static List<T> ConvertToList<T>(string json) where T : new()
		{
			List<T> list = new List<T> ();
			JSONObject jo = new JSONObject (json);
			if (jo.IsArray) {
				foreach (JSONObject j in jo.list)
				{
					list.Add(ExtractObject<T>(j));
				}
			}
			return list;
		}

		private static T ExtractObject<T>(JSONObject jo) where T : new()
		{
			T t = new T ();
			foreach (FieldInfo f in typeof(T).GetFields()) {
				if (jo.keys.Contains (f.Name)) {
					JSONObject fieldJo = jo.GetField(f.Name);
					try
					{
						switch(fieldJo.type)
						{
							case JSONObject.Type.BOOL:
								f.SetValue (t, fieldJo.b);
								break;
							case JSONObject.Type.NUMBER:
								String realType = typeof(T).GetField(f.Name).ToString().ToLower(); // hacky shit, but mono hides the type from us
								if( realType.Contains("int"))
								{
									f.SetValue (t, int.Parse(""+fieldJo.n));
								}
								else
								{
									f.SetValue (t, fieldJo.n);
								}
								break;
							case JSONObject.Type.STRING:
								f.SetValue (t, fieldJo.str);
								break;
						}
					}
					catch(Exception e)
					{
						UnityEngine.Debug.Log("Could not set value ["+fieldJo.str+"] into field ["+f.Name+"]\n"+e);
					}
				}
			}
			return t;
		}

	}
}

