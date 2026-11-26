using DG.Tweening;
using DG.Tweening.Core;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace orangesGame
{
	public static class GameUtils
	{
		private static Dictionary<string, int> _unitDic = new Dictionary<string, int>
		{
			{
				string.Empty,
				0
			},
			{
				"K",
				3
			},
			{
				"M",
				6
			},
			{
				"B",
				9
			},
			{
				"T",
				12
			},
			{
				"P",
				15
			},
			{
				"E",
				18
			},
			{
				"Z",
				21
			},
			{
				"Y",
				24
			},
			{
				"S",
				27
			}
		};

		private static Dictionary<int, string> _numberDic = new Dictionary<int, string>
		{
			{
				0,
				string.Empty
			},
			{
				3,
				"K"
			},
			{
				6,
				"M"
			},
			{
				9,
				"B"
			},
			{
				12,
				"T"
			},
			{
				15,
				"P"
			},
			{
				18,
				"E"
			},
			{
				21,
				"Z"
			},
			{
				24,
				"Y"
			},
			{
				27,
				"S"
			}
		};

		public static void InitTranLocal(Transform tran)
		{
			tran.localPosition = Vector2.zero;
			tran.localScale = Vector2.one;
			tran.localRotation = Quaternion.identity;
		}

		public static float UnitToFloat(string valueStr)
		{
			int num = valueStr.Length - 1;
			string key = valueStr.Substring(num, 1);
			string s;
			if (!_unitDic.ContainsKey(key))
			{
				key = string.Empty;
				s = valueStr;
			}
			else
			{
				s = valueStr.Substring(0, num);
			}
			int num2 = _unitDic[key];
			return float.Parse(s) * Mathf.Pow(10f, num2);
		}

		public static string FloatToUnit(float value)
		{
			string text = value.ToString();
			int num2;
			if (text.Contains("E"))
			{
				int num = int.Parse(value.ToString().Substring(value.ToString().Length - 2, 2));
				num2 = num / 3 * 3;
			}
			else
			{
				value = Mathf.Floor(value);
				num2 = (value.ToString().Length - 1) / 3 * 3;
			}
			string str = _numberDic[num2];
			float num3 = value / Mathf.Pow(10f, num2);
			if (num3 % 1f == 0f)
			{
				return num3.ToString() + str;
			}
			return (Mathf.Floor(num3 * 10f) / 10f).ToString("F1") + str;
		}

		public static int Compare(Unit a, Unit b)
		{
			if (a.Number > b.Number)
			{
				return 1;
			}
			if (a.Number < b.Number)
			{
				return -1;
			}
			if (a.Value > b.Value)
			{
				return 1;
			}
			if (a.Value < b.Value)
			{
				return -1;
			}
			return 0;
		}

		public static Unit UnitSub(Unit a, Unit b)
		{
			Unit unit = new Unit();
			int num = Compare(a, b);
			if (num == 0)
			{
				unit.Value = 0f;
				unit.Name = string.Empty;
				unit.Number = 0;
				return unit;
			}
			float num2 = 0f;
			int num3 = 0;
			int num4 = 0;
			if (num == 1)
			{
				num3 = a.Number;
				num4 = 1;
				num2 = a.Value - b.Value * Mathf.Pow(10f, b.Number - a.Number);
			}
			else
			{
				num3 = b.Number;
				num4 = -1;
				num2 = b.Value - a.Value * Mathf.Pow(10f, a.Number - b.Number);
			}
			float num5 = Mathf.Pow(10f, num3);
			num2 = Mathf.Round(num2 * num5) / num5;
			if (num2 < 1f)
			{
				unit.Value = num2 * 1000f * (float)num4;
				num3 = (unit.Number = num3 - 3);
				unit.Name = _numberDic[num3];
				return unit;
			}
			unit.Value = num2 * (float)num4;
			unit.Number = num3;
			unit.Name = _numberDic[num3];
			return unit;
		}

		public static void SetTranOrigin(Transform tran)
		{
			tran.localPosition = Vector3.zero;
			tran.rotation = Quaternion.identity;
			tran.localScale = Vector3.one;
		}

		public static void SetUIStretch(Transform tran)
		{
			RectTransform rectTransform = tran as RectTransform;
			rectTransform.offsetMax = Vector2.zero;
			rectTransform.offsetMin = Vector2.zero;
		}

		public static int GetDir(int index)
		{
			return -2 * index + 1;
		}

		public static float GetDir(float index)
		{
			return -2f * index + 1f;
		}

		public static int HitCountParse(float value)
		{
			int num = Mathf.CeilToInt(value);
			return (num >= 0) ? num : 0;
		}

		public static string CoinParse(int value)
		{
			if (value > 10000000)
			{
				return $"{(float)value / 1E+07f:f1}Y";
			}
			if (value > 1000000)
			{
				return $"{(float)value / 1000000f:f1}M";
			}
			if (value > 1000)
			{
				return $"{(float)value / 1000f:f1}K";
			}
			return value.ToString();
		}

		public static Tweener SetDOAnchorPosIn(RectTransform tran, float x, float y, float time, float delay)
		{
			Vector2 anchoredPosition = tran.anchoredPosition;
			tran.anchoredPosition += new Vector2(x, y);
			return tran.DOAnchorPos(anchoredPosition, time).SetDelay(delay).SetEase(Ease.OutBack)
				.SetAutoKill(autoKillOnCompletion: true);
		}

		public static Tweener SetDOAnchorPosOut(RectTransform tran, float x, float y, float time, float delay)
		{
			return tran.DOAnchorPos(new Vector2(x, y), time).SetDelay(delay).SetEase(Ease.OutBack)
				.SetAutoKill(autoKillOnCompletion: true);
		}

		public static string ParseMillisecond(int millisecond)
		{
			int num = millisecond / 1000;
			int num2 = millisecond % 1000 / 10;
			string text = string.Empty;
			if (num < 10)
			{
				text += "0";
			}
			text = text + num + ":";
			if (num2 < 10)
			{
				text += "0";
			}
			return text + num2;
		}

		public static string ParseSecondToMMSS(int scenond)
		{
			int num = scenond / 3600;
			int num2 = scenond / 60 - num * 60;
			if (num < 0)
			{
				num = 0;
			}
			if (num2 < 0)
			{
				num2 = 0;
			}
			return $"{$"{num:d2}"}:{$"{num2:d2}"}";
		}

		public static string ParseSecondToHHMMSS(int scenond)
		{
			int num = scenond / 3600;
			int num2 = scenond / 60 - num * 60;
			int num3 = scenond - num2 * 60 - num * 3600;
			if (num < 0)
			{
				num = 0;
			}
			if (num2 < 0)
			{
				num2 = 0;
			}
			if (num3 < 0)
			{
				num3 = 0;
			}
			return $"{$"{num:d2}"}:{$"{num2:d2}"}:{$"{num3:d2}"}";
		}

		public static void RollNumber(float oldValue, float newValue, float duration, DOSetter<float> callback)
		{
			Sequence sequence = DOTween.Sequence();
			sequence.SetAutoKill(autoKillOnCompletion: false);
			sequence.Append(DOTween.To(callback, oldValue, newValue, duration));
		}

		public static void DebugLogArr(int[] arr, string separator = " | ")
		{
			if (arr != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < arr.Length; i++)
				{
					stringBuilder.Append(arr[i]);
					stringBuilder.Append(separator);
				}
				UnityEngine.Debug.Log(stringBuilder.ToString());
			}
		}

		public static long GetTimestamp(DateTime time)
		{
			return (time.ToUniversalTime().Ticks - 621355968000000000L) / 10000;
		}

		public static bool IsArryNullOrEmpty<T>(T[] arry)
		{
			return arry == null || arry.Length == 0;
		}

		public static DateTime GetTargetTime(int day, int hour)
		{
			DateTime dateTime = DateTime.Now.AddDays(day);
			return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, hour, 0, 0);
		}

		public static DateTime GetTargetTimeTick(int day)
		{
			DateTime dateTime = DateTime.Now.AddDays(day);
			return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
		}

		public static Material GetGrayMaterial()
		{
			Material material = new Material(Shader.Find("IceMark/UIGray"));
			material.name = "UIGray";
			return material;
		}

		public static TimeSpan GetTimeSpan(long unixDate)
		{
			DateTime d = new DateTime(unixDate);
			return d - DateTime.Now;
		}

		public static string GetTimeUnixDate(long unixDate)
		{
			TimeSpan timeSpan = GetTimeSpan(unixDate);
			string empty = string.Empty;
			if (timeSpan.Days <= 0)
			{
				return timeSpan.Hours + "h " + timeSpan.Minutes + "m ";
			}
			return timeSpan.Days + "d " + timeSpan.Hours + "h";
		}

		public static double GetNextDayTime()
		{
			DateTime now = DateTime.Now;
			int hour = now.Hour;
			DateTime d;
			if (hour >= 0 && hour < 5)
			{
				d = new DateTime(now.Year, now.Month, now.Day, 5, 0, 0);
			}
			else
			{
				now = now.AddDays(1.0);
				d = new DateTime(now.Year, now.Month, now.Day, 5, 0, 0);
			}
			return (d - DateTime.Now).TotalSeconds;
		}
	}
}
