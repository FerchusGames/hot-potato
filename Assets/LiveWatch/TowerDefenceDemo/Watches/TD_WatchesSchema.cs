using System.Collections.Generic;
using Ingvar.LiveWatch;
using Ingvar.LiveWatch.Generation;
using Ingvar.LiveWatch.TowerDefenceDemo;

namespace LIngvar.LiveWatch.TowerDefenceDemo
{
	public class TD_WatchesSchema : WatchGenerationSchema
	{
		public override void OnDefine()
		{
			base.OnDefine();

			Define<MobMain>()
				.IgnoreMember(nameof(MobMain.Health))
				.SetAlwaysCollapsable(nameof(MobMain.WaypointMover))
				.SetSortOrder(nameof(MobMain.Id), -1)
				.SetMinMaxModeAsCustom(nameof(MobMain.CurrentHealth), 0, 20);

			Define<MobWaypointMover>()
				.SetDecimalPlaces(nameof(MobWaypointMover.TravelledDist), 4);
			
			Define<WaveManager>()
				.RenameMember(nameof(WaveManager.CurrentWave), "WaveIndex")
				.RenameMember(nameof(WaveManager.CurrentSpawn), "SpawnIndex");
		}

		public override void OnGenerate()
		{
			Generate<MobManager>();
			Generate<LevelStateType>();
			Generate<HealthManager>();
			Generate<WaveManager>();
			Generate<EnergyManager>();
			Generate<TowerBuildManager>();
			Generate<Dictionary<string, TowerBase>>();
			Generate<LevelConfig>();
			Generate<EconomyConfig>();
		}
	}
}