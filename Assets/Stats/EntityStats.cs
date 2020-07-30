namespace Stats
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;

	// Add your stats to this enum
	public enum Stat
	{
		Health = 0,
		MaxHealth,
		Attack,
		Length // DO NOT DELETE OR REMOVE. This must be last in the enum.
	}


	public class EntityStats : MonoBehaviour
	{
		public float[] BaseStats = new float[(int)Stat.Length];
		private float[] BuffedStats = new float[(int)Stat.Length];

		// Our algorithm:
		// Final Stat = ((BaseStats + AddPreMult) * Mult) + AddPostMult
		private float[] AddVariablePreMultiply = new float[(int)Stat.Length];
		private float[] MultiplyVariable = new float[(int)Stat.Length];
		private float[] AddVariablePostMultiply = new float[(int)Stat.Length];

		List<Buff> _buffs = new List<Buff>();

		[HideInInspector]
		public bool IsAlive = true;

		private void Awake()
		{
			BaseStats[(int)Stat.Health] = BaseStats[(int)Stat.MaxHealth];
		}

		public float GetBuffedStat(Stat targetStat)
		{
			return BuffedStats[(int)targetStat];
		}

		public float GetBaseStat(Stat targetStat)
		{
			return BaseStats[(int)targetStat];
		}

		public void SetBaseStat(Stat targetStat, float value)
		{
			BaseStats[(int)targetStat] = value;
		}

		public void ModifyBaseStat(Stat targetStat, float valueToAdd)
		{
			BaseStats[(int)targetStat] += valueToAdd;
		}

		public void AddBuff(Buff b)
		{
			// TODO: this uses Linq. It isn't efficient. It reads well and can easily be optimized later.
			switch (b.StackSetting)
			{
				case Buff.StackableSetting.OnePerSource:
					//remove all other buffs from this source
					this._buffs.RemoveAll(x => x.BuffOriginID == b.BuffOriginID);
					break;
				case Buff.StackableSetting.OneOnly:
					//remove all other buffs with matching ID.
					this._buffs.RemoveAll(x => x.BuffID == b.BuffID);
					break;
				case Buff.StackableSetting.Stacks:
				default:
					break;
			}
			this._buffs.Add(b);
		}

		/// Update our modified stats using buffs process those stats with our game-specific logic
		private void FixedUpdate()
		{
			// clear algorithm arrays
			Array.Clear(AddVariablePreMultiply, 0, (int)Stat.Length);
			Array.Clear(MultiplyVariable, 0, (int)Stat.Length);
			Array.Clear(AddVariablePostMultiply, 0, (int)Stat.Length);

			// apply buffs and debuffs to algorithm values
			for (int i = 0; i < (int)_buffs.Count; i++)
			{
				Buff b = _buffs[i];
				switch (b.BuffExpirationMode)
				{
					case Buff.ExpirationType.Timed:
						b.Duration -= Time.deltaTime;
						if (_buffs[i].Duration <= 0f)
						{
							this._buffs.RemoveAt(i);
							i--;
							continue;
						}
						break;
					default:
						break;
				}

				switch (b.BuffType)
				{
					case Buff.TargetVariable.AddPreMultiply:
						AddVariablePreMultiply[(int)b.TargetStat] += b.Value;
						break;
					case Buff.TargetVariable.Multiplier:
						MultiplyVariable[(int)b.TargetStat] += b.Value;
						break;
					case Buff.TargetVariable.AddPostMultiply:
						AddVariablePostMultiply[(int)b.TargetStat] += b.Value;
						break;
					default:
						break;
				}

				this._buffs[i] = b;
			}

			//calculate stats
			for (int i = 0; i < (int)Stat.Length; i++)
			{
				BuffedStats[i] = ((BaseStats[i] + AddVariablePreMultiply[i]) * (1f + MultiplyVariable[i])) + AddVariablePostMultiply[i];
			}

			// 
			// PUT POST-BUFF LOGIC HERE.
			// PROCESS STATS WITH GAME-SPECIFIC LOGIC
			//

			//clamp base hp value between 0 and buffed maxhp
			this.BaseStats[(int)Stat.Health] = Mathf.Clamp(this.BaseStats[(int)Stat.Health], 0f, this.BuffedStats[(int)Stat.MaxHealth]);
			//check if we're alive
			IsAlive = BuffedStats[(int)Stat.Health] > 0f;
		}

		#region CONVENIENCE METHODS
		public void Hit(float damage)
		{
			BaseStats[(int)Stat.Health] -= damage;
		}

		public void Heal(float healthToRestore)
		{
			BaseStats[(int)Stat.Health] += healthToRestore;
		}

		public void Kill()
		{
			BaseStats[(int)Stat.Health] = float.MinValue; ///will die next frame
		}
		#endregion
	}

}