using NWN.Core;

namespace NWM.API.Constants
{
  public enum IPAlignment
  {
    LawfulGood = NWScript.IP_CONST_ALIGNMENT_LG,
    LawfulNeutral = NWScript.IP_CONST_ALIGNMENT_LN,
    LawfulEvil = NWScript.IP_CONST_ALIGNMENT_LE,
    NeutralGood = NWScript.IP_CONST_ALIGNMENT_NG,
    TrueNeutral = NWScript.IP_CONST_ALIGNMENT_TN,
    NeutralEvil = NWScript.IP_CONST_ALIGNMENT_NE,
    ChaoticGood = NWScript.IP_CONST_ALIGNMENT_CG,
    ChaoticNeutral = NWScript.IP_CONST_ALIGNMENT_CN,
    ChaoticEvil = NWScript.IP_CONST_ALIGNMENT_CE
  }
}