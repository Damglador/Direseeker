﻿using System;
using EntityStates;
using EntityStates.LemurianBruiserMonster;
using RoR2;
using UnityEngine;

namespace DireseekerMod.States
{
	public class SpawnState : EntityState
	{
		public override void OnEnter()
		{
			base.OnEnter();
			base.GetModelAnimator();
			Util.PlaySound(EntityStates.LemurianBruiserMonster.SpawnState.spawnSoundString, base.gameObject);
			Util.PlaySound("DireseekerSpawn", base.gameObject);
			base.PlayAnimation("Body", "Spawn", "Spawn.playbackRate", SpawnState.duration);
			EffectManager.SimpleMuzzleFlash(EntityStates.LemurianBruiserMonster.SpawnState.spawnEffectPrefab, base.gameObject, "SpawnEffectOrigin", false);
			this.effectStopwatch = 0.7f;
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			this.effectStopwatch -= Time.fixedDeltaTime;
			bool flag = this.effectStopwatch <= 0f & base.fixedAge < 0.5f * SpawnState.duration;
			if (flag)
			{
				this.PlaySpawnEffect();
			}
			bool flag2 = base.fixedAge >= SpawnState.duration && base.isAuthority;
			if (flag2)
			{
				this.outer.SetNextStateToMain();
			}
		}

		private void PlaySpawnEffect()
		{
			this.effectStopwatch = 0.1f;
			for (int i = 0; i <= 5; i++)
			{
				Vector3 origin = base.characterBody.footPosition + UnityEngine.Random.insideUnitSphere * 22f;
				origin.y = base.characterBody.footPosition.y;
				EffectManager.SpawnEffect(EntityStates.LemurianBruiserMonster.SpawnState.spawnEffectPrefab, new EffectData
				{
					origin = origin,
					scale = 4f
				}, true);
			}
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Death;
		}

		public static float duration = 2.5f;
		private float effectStopwatch;
	}
}
