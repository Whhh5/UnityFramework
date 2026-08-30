public partial class GameSchedule
{
	private ClipCfg[] m_ClipCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, ClipCfg> m_DicClipCfg0 = new();
	private SkillCfg[] m_SkillCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, SkillCfg> m_DicSkillCfg0 = new();
	private AnimLayerCfg[] m_AnimLayerCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, AnimLayerCfg> m_DicAnimLayerCfg0 = new();
	private CmdCfg[] m_CmdCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, CmdCfg> m_DicCmdCfg0 = new();
	private CmdTyoeCfg[] m_CmdTyoeCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, CmdTyoeCfg> m_DicCmdTyoeCfg0 = new();
	private CmdLevelCfg[] m_CmdLevelCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, CmdLevelCfg> m_DicCmdLevelCfg0 = new();
	private EffectCfg[] m_EffectCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, EffectCfg> m_DicEffectCfg0 = new();
	private AssetCfg[] m_AssetCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, AssetCfg> m_DicAssetCfg0 = new();
	private CharacterCfg[] m_CharacterCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, CharacterCfg> m_DicCharacterCfg0 = new();
	private MonsterCfg[] m_MonsterCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, MonsterCfg> m_DicMonsterCfg0 = new();
	private AIModuleCfg[] m_AIModuleCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, AIModuleCfg> m_DicAIModuleCfg0 = new();
	private AICfg[] m_AICfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, AICfg> m_DicAICfg0 = new();
	private MonsterControllerCfg[] m_MonsterControllerCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, MonsterControllerCfg> m_DicMonsterControllerCfg0 = new();
	private BuffCfg[] m_BuffCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, BuffCfg> m_DicBuffCfg0 = new();
	private BuffTypeCfg[] m_BuffTypeCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, BuffTypeCfg> m_DicBuffTypeCfg0 = new();
	private TimelineCfg[] m_TimelineCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, TimelineCfg> m_DicTimelineCfg0 = new();
	private ProjectileCfg[] m_ProjectileCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, ProjectileCfg> m_DicProjectileCfg0 = new();
	private AudioCfg[] m_AudioCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, AudioCfg> m_DicAudioCfg0 = new();
	private AudioLayerCfg[] m_AudioLayerCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, AudioLayerCfg> m_DicAudioLayerCfg0 = new();
	private MapCfg[] m_MapCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, MapCfg> m_DicMapCfg0 = new();
	private SkillBallCfg[] m_SkillBallCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, SkillBallCfg> m_DicSkillBallCfg0 = new();
	private SkillBallFussionCfg[] m_SkillBallFussionCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, SkillBallFussionCfg> m_DicSkillBallFussionCfg0 = new();
	private SkillBallPartCfg[] m_SkillBallPartCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, SkillBallPartCfg> m_DicSkillBallPartCfg0 = new();
	private AttributeCfg[] m_AttributeCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, AttributeCfg> m_DicAttributeCfg0 = new();
	private BattleWaveCfg[] m_BattleWaveCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, BattleWaveCfg> m_DicBattleWaveCfg0 = new();
	private LanguageCfg[] m_LanguageCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, LanguageCfg> m_DicLanguageCfg0 = new();
	private DamageTypeCfg[] m_DamageTypeCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, DamageTypeCfg> m_DicDamageTypeCfg0 = new();
	private AttributeValueCfg[] m_AttributeValueCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, AttributeValueCfg> m_DicAttributeValueCfg0 = new();
	private SkillPassivityCfg[] m_SkillPassivityCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, SkillPassivityCfg> m_DicSkillPassivityCfg0 = new();
	private ChapterCfg[] m_ChapterCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, ChapterCfg> m_DicChapterCfg0 = new();
	private CoreCfg[] m_CoreCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, CoreCfg> m_DicCoreCfg0 = new();
	private CoreTypeCfg[] m_CoreTypeCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, CoreTypeCfg> m_DicCoreTypeCfg0 = new();
	private CoreQualityCfg[] m_CoreQualityCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, CoreQualityCfg> m_DicCoreQualityCfg0 = new();
	private CoreEffectCfg[] m_CoreEffectCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, CoreEffectCfg> m_DicCoreEffectCfg0 = new();
	private GoodsCfg[] m_GoodsCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, GoodsCfg> m_DicGoodsCfg0 = new();
	private GoodsTypeCfg[] m_GoodsTypeCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, GoodsTypeCfg> m_DicGoodsTypeCfg0 = new();
	private AccountLevelCfg[] m_AccountLevelCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, AccountLevelCfg> m_DicAccountLevelCfg0 = new();
	private GemCfg[] m_GemCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, GemCfg> m_DicGemCfg0 = new();
	private GemTypeCfg[] m_GemTypeCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, GemTypeCfg> m_DicGemTypeCfg0 = new();
	private GemEffectTypeCfg[] m_GemEffectTypeCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, GemEffectTypeCfg> m_DicGemEffectTypeCfg0 = new();
	private GemQualityCfg[] m_GemQualityCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, GemQualityCfg> m_DicGemQualityCfg0 = new();
	private GlobalCfg[] m_GlobalCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, GlobalCfg> m_DicGlobalCfg0 = new();
	public void Initialization()
	{
		for (int i = 0; i < m_ClipCfg.Length; i++)
		{
			var cfg = m_ClipCfg[i];
			m_DicClipCfg0.Add(cfg.nClipID, cfg);
		}
		for (int i = 0; i < m_SkillCfg.Length; i++)
		{
			var cfg = m_SkillCfg[i];
			m_DicSkillCfg0.Add(cfg.nSkillID, cfg);
		}
		for (int i = 0; i < m_AnimLayerCfg.Length; i++)
		{
			var cfg = m_AnimLayerCfg[i];
			m_DicAnimLayerCfg0.Add(cfg.nLayer, cfg);
		}
		for (int i = 0; i < m_CmdCfg.Length; i++)
		{
			var cfg = m_CmdCfg[i];
			m_DicCmdCfg0.Add(cfg.nCmdID, cfg);
		}
		for (int i = 0; i < m_CmdTyoeCfg.Length; i++)
		{
			var cfg = m_CmdTyoeCfg[i];
			m_DicCmdTyoeCfg0.Add(cfg.nCmdID, cfg);
		}
		for (int i = 0; i < m_CmdLevelCfg.Length; i++)
		{
			var cfg = m_CmdLevelCfg[i];
			m_DicCmdLevelCfg0.Add(cfg.nLevelID, cfg);
		}
		for (int i = 0; i < m_EffectCfg.Length; i++)
		{
			var cfg = m_EffectCfg[i];
			m_DicEffectCfg0.Add(cfg.nEffectID, cfg);
		}
		for (int i = 0; i < m_AssetCfg.Length; i++)
		{
			var cfg = m_AssetCfg[i];
			m_DicAssetCfg0.Add(cfg.nAssetID, cfg);
		}
		for (int i = 0; i < m_CharacterCfg.Length; i++)
		{
			var cfg = m_CharacterCfg[i];
			m_DicCharacterCfg0.Add(cfg.nCharacterID, cfg);
		}
		for (int i = 0; i < m_MonsterCfg.Length; i++)
		{
			var cfg = m_MonsterCfg[i];
			m_DicMonsterCfg0.Add(cfg.nMonsterID, cfg);
		}
		for (int i = 0; i < m_AIModuleCfg.Length; i++)
		{
			var cfg = m_AIModuleCfg[i];
			m_DicAIModuleCfg0.Add(cfg.nModuleID, cfg);
		}
		for (int i = 0; i < m_AICfg.Length; i++)
		{
			var cfg = m_AICfg[i];
			m_DicAICfg0.Add(cfg.nAIID, cfg);
		}
		for (int i = 0; i < m_MonsterControllerCfg.Length; i++)
		{
			var cfg = m_MonsterControllerCfg[i];
			m_DicMonsterControllerCfg0.Add(cfg.nControllerID, cfg);
		}
		for (int i = 0; i < m_BuffCfg.Length; i++)
		{
			var cfg = m_BuffCfg[i];
			m_DicBuffCfg0.Add(cfg.nBuffID, cfg);
		}
		for (int i = 0; i < m_BuffTypeCfg.Length; i++)
		{
			var cfg = m_BuffTypeCfg[i];
			m_DicBuffTypeCfg0.Add(cfg.nTypeID, cfg);
		}
		for (int i = 0; i < m_TimelineCfg.Length; i++)
		{
			var cfg = m_TimelineCfg[i];
			m_DicTimelineCfg0.Add(cfg.nCmdID, cfg);
		}
		for (int i = 0; i < m_ProjectileCfg.Length; i++)
		{
			var cfg = m_ProjectileCfg[i];
			m_DicProjectileCfg0.Add(cfg.nProjectileId, cfg);
		}
		for (int i = 0; i < m_AudioCfg.Length; i++)
		{
			var cfg = m_AudioCfg[i];
			m_DicAudioCfg0.Add(cfg.nAudioID, cfg);
		}
		for (int i = 0; i < m_AudioLayerCfg.Length; i++)
		{
			var cfg = m_AudioLayerCfg[i];
			m_DicAudioLayerCfg0.Add(cfg.nLayer, cfg);
		}
		for (int i = 0; i < m_MapCfg.Length; i++)
		{
			var cfg = m_MapCfg[i];
			m_DicMapCfg0.Add(cfg.nMapId, cfg);
		}
		for (int i = 0; i < m_SkillBallCfg.Length; i++)
		{
			var cfg = m_SkillBallCfg[i];
			m_DicSkillBallCfg0.Add(cfg.nBallId, cfg);
		}
		for (int i = 0; i < m_SkillBallFussionCfg.Length; i++)
		{
			var cfg = m_SkillBallFussionCfg[i];
			m_DicSkillBallFussionCfg0.Add(cfg.nFussionId, cfg);
		}
		for (int i = 0; i < m_SkillBallPartCfg.Length; i++)
		{
			var cfg = m_SkillBallPartCfg[i];
			m_DicSkillBallPartCfg0.Add(cfg.nPartId, cfg);
		}
		for (int i = 0; i < m_AttributeCfg.Length; i++)
		{
			var cfg = m_AttributeCfg[i];
			m_DicAttributeCfg0.Add(cfg.nAttributeId, cfg);
		}
		for (int i = 0; i < m_BattleWaveCfg.Length; i++)
		{
			var cfg = m_BattleWaveCfg[i];
			m_DicBattleWaveCfg0.Add(cfg.nWaveId, cfg);
		}
		for (int i = 0; i < m_LanguageCfg.Length; i++)
		{
			var cfg = m_LanguageCfg[i];
			m_DicLanguageCfg0.Add(cfg.nLangId, cfg);
		}
		for (int i = 0; i < m_DamageTypeCfg.Length; i++)
		{
			var cfg = m_DamageTypeCfg[i];
			m_DicDamageTypeCfg0.Add(cfg.nDamageType, cfg);
		}
		for (int i = 0; i < m_AttributeValueCfg.Length; i++)
		{
			var cfg = m_AttributeValueCfg[i];
			m_DicAttributeValueCfg0.Add(cfg.nValueId, cfg);
		}
		for (int i = 0; i < m_SkillPassivityCfg.Length; i++)
		{
			var cfg = m_SkillPassivityCfg[i];
			m_DicSkillPassivityCfg0.Add(cfg.nSkillPassivityId, cfg);
		}
		for (int i = 0; i < m_ChapterCfg.Length; i++)
		{
			var cfg = m_ChapterCfg[i];
			m_DicChapterCfg0.Add(cfg.nChapterId, cfg);
		}
		for (int i = 0; i < m_CoreCfg.Length; i++)
		{
			var cfg = m_CoreCfg[i];
			m_DicCoreCfg0.Add(cfg.nCoreId, cfg);
		}
		for (int i = 0; i < m_CoreTypeCfg.Length; i++)
		{
			var cfg = m_CoreTypeCfg[i];
			m_DicCoreTypeCfg0.Add(cfg.nCoreType, cfg);
		}
		for (int i = 0; i < m_CoreQualityCfg.Length; i++)
		{
			var cfg = m_CoreQualityCfg[i];
			m_DicCoreQualityCfg0.Add(cfg.nCoreQuality, cfg);
		}
		for (int i = 0; i < m_CoreEffectCfg.Length; i++)
		{
			var cfg = m_CoreEffectCfg[i];
			m_DicCoreEffectCfg0.Add(cfg.nCoreEffectId, cfg);
		}
		for (int i = 0; i < m_GoodsCfg.Length; i++)
		{
			var cfg = m_GoodsCfg[i];
			m_DicGoodsCfg0.Add(cfg.nGoodsId, cfg);
		}
		for (int i = 0; i < m_GoodsTypeCfg.Length; i++)
		{
			var cfg = m_GoodsTypeCfg[i];
			m_DicGoodsTypeCfg0.Add(cfg.nGoodsTypeId, cfg);
		}
		for (int i = 0; i < m_AccountLevelCfg.Length; i++)
		{
			var cfg = m_AccountLevelCfg[i];
			m_DicAccountLevelCfg0.Add(cfg.nLevelId, cfg);
		}
		for (int i = 0; i < m_GemCfg.Length; i++)
		{
			var cfg = m_GemCfg[i];
			m_DicGemCfg0.Add(cfg.nGemId, cfg);
		}
		for (int i = 0; i < m_GemTypeCfg.Length; i++)
		{
			var cfg = m_GemTypeCfg[i];
			m_DicGemTypeCfg0.Add(cfg.nGemTypeId, cfg);
		}
		for (int i = 0; i < m_GemEffectTypeCfg.Length; i++)
		{
			var cfg = m_GemEffectTypeCfg[i];
			m_DicGemEffectTypeCfg0.Add(cfg.nEffectTypeId, cfg);
		}
		for (int i = 0; i < m_GemQualityCfg.Length; i++)
		{
			var cfg = m_GemQualityCfg[i];
			m_DicGemQualityCfg0.Add(cfg.nGemQualityId, cfg);
		}
		for (int i = 0; i < m_GlobalCfg.Length; i++)
		{
			var cfg = m_GlobalCfg[i];
			m_DicGlobalCfg0.Add(cfg.nId, cfg);
		}
	}
	public System.Int32 GetClipCfgCount()
	{
		return m_ClipCfg.Length;
	}
	public ClipCfg GetClipCfg(System.Int32 index)
	{
		return m_ClipCfg[index];
	}
	public ClipCfg GetClipCfg0(System.Int32 nClipID)
	{
		return m_DicClipCfg0[nClipID];
	}
	public System.Int32 GetSkillCfgCount()
	{
		return m_SkillCfg.Length;
	}
	public SkillCfg GetSkillCfg(System.Int32 index)
	{
		return m_SkillCfg[index];
	}
	public SkillCfg GetSkillCfg0(System.Int32 nSkillID)
	{
		return m_DicSkillCfg0[nSkillID];
	}
	public System.Int32 GetAnimLayerCfgCount()
	{
		return m_AnimLayerCfg.Length;
	}
	public AnimLayerCfg GetAnimLayerCfg(System.Int32 index)
	{
		return m_AnimLayerCfg[index];
	}
	public AnimLayerCfg GetAnimLayerCfg0(System.Int32 nLayer)
	{
		return m_DicAnimLayerCfg0[nLayer];
	}
	public System.Int32 GetCmdCfgCount()
	{
		return m_CmdCfg.Length;
	}
	public CmdCfg GetCmdCfg(System.Int32 index)
	{
		return m_CmdCfg[index];
	}
	public CmdCfg GetCmdCfg0(System.Int32 nCmdID)
	{
		return m_DicCmdCfg0[nCmdID];
	}
	public System.Int32 GetCmdTyoeCfgCount()
	{
		return m_CmdTyoeCfg.Length;
	}
	public CmdTyoeCfg GetCmdTyoeCfg(System.Int32 index)
	{
		return m_CmdTyoeCfg[index];
	}
	public CmdTyoeCfg GetCmdTyoeCfg0(System.Int32 nCmdID)
	{
		return m_DicCmdTyoeCfg0[nCmdID];
	}
	public System.Int32 GetCmdLevelCfgCount()
	{
		return m_CmdLevelCfg.Length;
	}
	public CmdLevelCfg GetCmdLevelCfg(System.Int32 index)
	{
		return m_CmdLevelCfg[index];
	}
	public CmdLevelCfg GetCmdLevelCfg0(System.Int32 nLevelID)
	{
		return m_DicCmdLevelCfg0[nLevelID];
	}
	public System.Int32 GetEffectCfgCount()
	{
		return m_EffectCfg.Length;
	}
	public EffectCfg GetEffectCfg(System.Int32 index)
	{
		return m_EffectCfg[index];
	}
	public EffectCfg GetEffectCfg0(System.Int32 nEffectID)
	{
		return m_DicEffectCfg0[nEffectID];
	}
	public System.Int32 GetAssetCfgCount()
	{
		return m_AssetCfg.Length;
	}
	public AssetCfg GetAssetCfg(System.Int32 index)
	{
		return m_AssetCfg[index];
	}
	public AssetCfg GetAssetCfg0(System.Int32 nAssetID)
	{
		return m_DicAssetCfg0[nAssetID];
	}
	public System.Int32 GetCharacterCfgCount()
	{
		return m_CharacterCfg.Length;
	}
	public CharacterCfg GetCharacterCfg(System.Int32 index)
	{
		return m_CharacterCfg[index];
	}
	public CharacterCfg GetCharacterCfg0(System.Int32 nCharacterID)
	{
		return m_DicCharacterCfg0[nCharacterID];
	}
	public System.Int32 GetMonsterCfgCount()
	{
		return m_MonsterCfg.Length;
	}
	public MonsterCfg GetMonsterCfg(System.Int32 index)
	{
		return m_MonsterCfg[index];
	}
	public MonsterCfg GetMonsterCfg0(System.Int32 nMonsterID)
	{
		return m_DicMonsterCfg0[nMonsterID];
	}
	public System.Int32 GetAIModuleCfgCount()
	{
		return m_AIModuleCfg.Length;
	}
	public AIModuleCfg GetAIModuleCfg(System.Int32 index)
	{
		return m_AIModuleCfg[index];
	}
	public AIModuleCfg GetAIModuleCfg0(System.Int32 nModuleID)
	{
		return m_DicAIModuleCfg0[nModuleID];
	}
	public System.Int32 GetAICfgCount()
	{
		return m_AICfg.Length;
	}
	public AICfg GetAICfg(System.Int32 index)
	{
		return m_AICfg[index];
	}
	public AICfg GetAICfg0(System.Int32 nAIID)
	{
		return m_DicAICfg0[nAIID];
	}
	public System.Int32 GetMonsterControllerCfgCount()
	{
		return m_MonsterControllerCfg.Length;
	}
	public MonsterControllerCfg GetMonsterControllerCfg(System.Int32 index)
	{
		return m_MonsterControllerCfg[index];
	}
	public MonsterControllerCfg GetMonsterControllerCfg0(System.Int32 nControllerID)
	{
		return m_DicMonsterControllerCfg0[nControllerID];
	}
	public System.Int32 GetBuffCfgCount()
	{
		return m_BuffCfg.Length;
	}
	public BuffCfg GetBuffCfg(System.Int32 index)
	{
		return m_BuffCfg[index];
	}
	public BuffCfg GetBuffCfg0(System.Int32 nBuffID)
	{
		return m_DicBuffCfg0[nBuffID];
	}
	public System.Int32 GetBuffTypeCfgCount()
	{
		return m_BuffTypeCfg.Length;
	}
	public BuffTypeCfg GetBuffTypeCfg(System.Int32 index)
	{
		return m_BuffTypeCfg[index];
	}
	public BuffTypeCfg GetBuffTypeCfg0(System.Int32 nTypeID)
	{
		return m_DicBuffTypeCfg0[nTypeID];
	}
	public System.Int32 GetTimelineCfgCount()
	{
		return m_TimelineCfg.Length;
	}
	public TimelineCfg GetTimelineCfg(System.Int32 index)
	{
		return m_TimelineCfg[index];
	}
	public TimelineCfg GetTimelineCfg0(System.Int32 nCmdID)
	{
		return m_DicTimelineCfg0[nCmdID];
	}
	public System.Int32 GetProjectileCfgCount()
	{
		return m_ProjectileCfg.Length;
	}
	public ProjectileCfg GetProjectileCfg(System.Int32 index)
	{
		return m_ProjectileCfg[index];
	}
	public ProjectileCfg GetProjectileCfg0(System.Int32 nProjectileId)
	{
		return m_DicProjectileCfg0[nProjectileId];
	}
	public System.Int32 GetAudioCfgCount()
	{
		return m_AudioCfg.Length;
	}
	public AudioCfg GetAudioCfg(System.Int32 index)
	{
		return m_AudioCfg[index];
	}
	public AudioCfg GetAudioCfg0(System.Int32 nAudioID)
	{
		return m_DicAudioCfg0[nAudioID];
	}
	public System.Int32 GetAudioLayerCfgCount()
	{
		return m_AudioLayerCfg.Length;
	}
	public AudioLayerCfg GetAudioLayerCfg(System.Int32 index)
	{
		return m_AudioLayerCfg[index];
	}
	public AudioLayerCfg GetAudioLayerCfg0(System.Int32 nLayer)
	{
		return m_DicAudioLayerCfg0[nLayer];
	}
	public System.Int32 GetMapCfgCount()
	{
		return m_MapCfg.Length;
	}
	public MapCfg GetMapCfg(System.Int32 index)
	{
		return m_MapCfg[index];
	}
	public MapCfg GetMapCfg0(System.Int32 nMapId)
	{
		return m_DicMapCfg0[nMapId];
	}
	public System.Int32 GetSkillBallCfgCount()
	{
		return m_SkillBallCfg.Length;
	}
	public SkillBallCfg GetSkillBallCfg(System.Int32 index)
	{
		return m_SkillBallCfg[index];
	}
	public SkillBallCfg GetSkillBallCfg0(System.Int32 nBallId)
	{
		return m_DicSkillBallCfg0[nBallId];
	}
	public System.Int32 GetSkillBallFussionCfgCount()
	{
		return m_SkillBallFussionCfg.Length;
	}
	public SkillBallFussionCfg GetSkillBallFussionCfg(System.Int32 index)
	{
		return m_SkillBallFussionCfg[index];
	}
	public SkillBallFussionCfg GetSkillBallFussionCfg0(System.Int32 nFussionId)
	{
		return m_DicSkillBallFussionCfg0[nFussionId];
	}
	public System.Int32 GetSkillBallPartCfgCount()
	{
		return m_SkillBallPartCfg.Length;
	}
	public SkillBallPartCfg GetSkillBallPartCfg(System.Int32 index)
	{
		return m_SkillBallPartCfg[index];
	}
	public SkillBallPartCfg GetSkillBallPartCfg0(System.Int32 nPartId)
	{
		return m_DicSkillBallPartCfg0[nPartId];
	}
	public System.Int32 GetAttributeCfgCount()
	{
		return m_AttributeCfg.Length;
	}
	public AttributeCfg GetAttributeCfg(System.Int32 index)
	{
		return m_AttributeCfg[index];
	}
	public AttributeCfg GetAttributeCfg0(System.Int32 nAttributeId)
	{
		return m_DicAttributeCfg0[nAttributeId];
	}
	public System.Int32 GetBattleWaveCfgCount()
	{
		return m_BattleWaveCfg.Length;
	}
	public BattleWaveCfg GetBattleWaveCfg(System.Int32 index)
	{
		return m_BattleWaveCfg[index];
	}
	public BattleWaveCfg GetBattleWaveCfg0(System.Int32 nWaveId)
	{
		return m_DicBattleWaveCfg0[nWaveId];
	}
	public System.Int32 GetLanguageCfgCount()
	{
		return m_LanguageCfg.Length;
	}
	public LanguageCfg GetLanguageCfg(System.Int32 index)
	{
		return m_LanguageCfg[index];
	}
	public LanguageCfg GetLanguageCfg0(System.Int32 nLangId)
	{
		return m_DicLanguageCfg0[nLangId];
	}
	public System.Int32 GetDamageTypeCfgCount()
	{
		return m_DamageTypeCfg.Length;
	}
	public DamageTypeCfg GetDamageTypeCfg(System.Int32 index)
	{
		return m_DamageTypeCfg[index];
	}
	public DamageTypeCfg GetDamageTypeCfg0(System.Int32 nDamageType)
	{
		return m_DicDamageTypeCfg0[nDamageType];
	}
	public System.Int32 GetAttributeValueCfgCount()
	{
		return m_AttributeValueCfg.Length;
	}
	public AttributeValueCfg GetAttributeValueCfg(System.Int32 index)
	{
		return m_AttributeValueCfg[index];
	}
	public AttributeValueCfg GetAttributeValueCfg0(System.Int32 nValueId)
	{
		return m_DicAttributeValueCfg0[nValueId];
	}
	public System.Int32 GetSkillPassivityCfgCount()
	{
		return m_SkillPassivityCfg.Length;
	}
	public SkillPassivityCfg GetSkillPassivityCfg(System.Int32 index)
	{
		return m_SkillPassivityCfg[index];
	}
	public SkillPassivityCfg GetSkillPassivityCfg0(System.Int32 nSkillPassivityId)
	{
		return m_DicSkillPassivityCfg0[nSkillPassivityId];
	}
	public System.Int32 GetChapterCfgCount()
	{
		return m_ChapterCfg.Length;
	}
	public ChapterCfg GetChapterCfg(System.Int32 index)
	{
		return m_ChapterCfg[index];
	}
	public ChapterCfg GetChapterCfg0(System.Int32 nChapterId)
	{
		return m_DicChapterCfg0[nChapterId];
	}
	public System.Int32 GetCoreCfgCount()
	{
		return m_CoreCfg.Length;
	}
	public CoreCfg GetCoreCfg(System.Int32 index)
	{
		return m_CoreCfg[index];
	}
	public CoreCfg GetCoreCfg0(System.Int32 nCoreId)
	{
		return m_DicCoreCfg0[nCoreId];
	}
	public System.Int32 GetCoreTypeCfgCount()
	{
		return m_CoreTypeCfg.Length;
	}
	public CoreTypeCfg GetCoreTypeCfg(System.Int32 index)
	{
		return m_CoreTypeCfg[index];
	}
	public CoreTypeCfg GetCoreTypeCfg0(System.Int32 nCoreType)
	{
		return m_DicCoreTypeCfg0[nCoreType];
	}
	public System.Int32 GetCoreQualityCfgCount()
	{
		return m_CoreQualityCfg.Length;
	}
	public CoreQualityCfg GetCoreQualityCfg(System.Int32 index)
	{
		return m_CoreQualityCfg[index];
	}
	public CoreQualityCfg GetCoreQualityCfg0(System.Int32 nCoreQuality)
	{
		return m_DicCoreQualityCfg0[nCoreQuality];
	}
	public System.Int32 GetCoreEffectCfgCount()
	{
		return m_CoreEffectCfg.Length;
	}
	public CoreEffectCfg GetCoreEffectCfg(System.Int32 index)
	{
		return m_CoreEffectCfg[index];
	}
	public CoreEffectCfg GetCoreEffectCfg0(System.Int32 nCoreEffectId)
	{
		return m_DicCoreEffectCfg0[nCoreEffectId];
	}
	public System.Int32 GetGoodsCfgCount()
	{
		return m_GoodsCfg.Length;
	}
	public GoodsCfg GetGoodsCfg(System.Int32 index)
	{
		return m_GoodsCfg[index];
	}
	public GoodsCfg GetGoodsCfg0(System.Int32 nGoodsId)
	{
		return m_DicGoodsCfg0[nGoodsId];
	}
	public System.Int32 GetGoodsTypeCfgCount()
	{
		return m_GoodsTypeCfg.Length;
	}
	public GoodsTypeCfg GetGoodsTypeCfg(System.Int32 index)
	{
		return m_GoodsTypeCfg[index];
	}
	public GoodsTypeCfg GetGoodsTypeCfg0(System.Int32 nGoodsTypeId)
	{
		return m_DicGoodsTypeCfg0[nGoodsTypeId];
	}
	public System.Int32 GetAccountLevelCfgCount()
	{
		return m_AccountLevelCfg.Length;
	}
	public AccountLevelCfg GetAccountLevelCfg(System.Int32 index)
	{
		return m_AccountLevelCfg[index];
	}
	public AccountLevelCfg GetAccountLevelCfg0(System.Int32 nLevelId)
	{
		return m_DicAccountLevelCfg0[nLevelId];
	}
	public System.Int32 GetGemCfgCount()
	{
		return m_GemCfg.Length;
	}
	public GemCfg GetGemCfg(System.Int32 index)
	{
		return m_GemCfg[index];
	}
	public GemCfg GetGemCfg0(System.Int32 nGemId)
	{
		return m_DicGemCfg0[nGemId];
	}
	public System.Int32 GetGemTypeCfgCount()
	{
		return m_GemTypeCfg.Length;
	}
	public GemTypeCfg GetGemTypeCfg(System.Int32 index)
	{
		return m_GemTypeCfg[index];
	}
	public GemTypeCfg GetGemTypeCfg0(System.Int32 nGemTypeId)
	{
		return m_DicGemTypeCfg0[nGemTypeId];
	}
	public System.Int32 GetGemEffectTypeCfgCount()
	{
		return m_GemEffectTypeCfg.Length;
	}
	public GemEffectTypeCfg GetGemEffectTypeCfg(System.Int32 index)
	{
		return m_GemEffectTypeCfg[index];
	}
	public GemEffectTypeCfg GetGemEffectTypeCfg0(System.Int32 nEffectTypeId)
	{
		return m_DicGemEffectTypeCfg0[nEffectTypeId];
	}
	public System.Int32 GetGemQualityCfgCount()
	{
		return m_GemQualityCfg.Length;
	}
	public GemQualityCfg GetGemQualityCfg(System.Int32 index)
	{
		return m_GemQualityCfg[index];
	}
	public GemQualityCfg GetGemQualityCfg0(System.Int32 nGemQualityId)
	{
		return m_DicGemQualityCfg0[nGemQualityId];
	}
	public System.Int32 GetGlobalCfgCount()
	{
		return m_GlobalCfg.Length;
	}
	public GlobalCfg GetGlobalCfg(System.Int32 index)
	{
		return m_GlobalCfg[index];
	}
	public GlobalCfg GetGlobalCfg0(System.Int32 nId)
	{
		return m_DicGlobalCfg0[nId];
	}
}
