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
	private MonsterCfg[] m_MonsterCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, MonsterCfg> m_DicMonsterCfg0 = new();
	private AIModuleCfg[] m_AIModuleCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, AIModuleCfg> m_DicAIModuleCfg0 = new();
	private AICfg[] m_AICfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, AICfg> m_DicAICfg0 = new();
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
	private LanguageCfg[] m_LanguageCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, LanguageCfg> m_DicLanguageCfg0 = new();
	private AccountLevelCfg[] m_AccountLevelCfg = null;
	private System.Collections.Generic.Dictionary<System.Int32, AccountLevelCfg> m_DicAccountLevelCfg0 = new();
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
		for (int i = 0; i < m_LanguageCfg.Length; i++)
		{
			var cfg = m_LanguageCfg[i];
			m_DicLanguageCfg0.Add(cfg.nLangId, cfg);
		}
		for (int i = 0; i < m_AccountLevelCfg.Length; i++)
		{
			var cfg = m_AccountLevelCfg[i];
			m_DicAccountLevelCfg0.Add(cfg.nLevelId, cfg);
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
