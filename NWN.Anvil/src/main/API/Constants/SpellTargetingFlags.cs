using System;
using NWN.Core;

namespace Anvil.API
{
  [Flags]
  public enum SpellTargetingFlags
  {
    None = NWScript.SPELL_TARGETING_FLAGS_NONE,
    HarmsEnemies = NWScript.SPELL_TARGETING_FLAGS_HARMS_ENEMIES,
    HarmsAllies = NWScript.SPELL_TARGETING_FLAGS_HARMS_ALLIES,
    HelpsAllies = NWScript.SPELL_TARGETING_FLAGS_HELPS_ALLIES,
    IgnoresSelf = NWScript.SPELL_TARGETING_FLAGS_IGNORES_SELF,
    OriginOnSelf = NWScript.SPELL_TARGETING_FLAGS_ORIGIN_ON_SELF,
    SuppressWithTarget = NWScript.SPELL_TARGETING_FLAGS_SUPPRESS_WITH_TARGET,
  }
}
