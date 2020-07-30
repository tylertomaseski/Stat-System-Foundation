using UnityEngine;

namespace Stats
{
	[CreateAssetMenu(menuName = "PL/Buff Template")]
	public class BuffTemplate : ScriptableObject
	{
		[SerializeField]
		Buff _buff;
		//Add other resources here. Like icon or display name

		public Buff GetBuff(int originID)
		{
			Buff b = _buff;
			b.BuffOriginID = originID;
			b.Template = this;
			return b;
		}

		private void OnValidate()
		{
			if (_buff.BuffID == 0)
				_buff.BuffID = Random.Range(int.MinValue, int.MaxValue);
		}
	}
}