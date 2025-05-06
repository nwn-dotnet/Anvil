using Anvil.API;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  [TestFixture]
  public class TalentTests
  {
    [Test(Description = "Creating a talent and disposing the talent explicitly frees the associated memory.")]
    public void CreateAndDisposeTalentFreesNativeStructure()
    {
      Talent talent = NwSkill.FromSkillType(Skill.Intimidate)!.ToTalent();
      Assert.That(talent.IsValid, Is.True, "Talent was not valid after creation.");
      talent.Dispose();
      Assert.That(talent.IsValid, Is.False, "Talent was still valid after disposing.");
    }
  }
}
