namespace HardwareStore.Tests
{
    using HardwareStore.Core.ViewModels.Admin;

    [TestFixture]
    public class AssemblyRoleMappingTests
    {
        [Test]
        public void ProductCategoryMatchesRole_Motherboard_accepts_synonyms()
        {
            Assert.That(
                AssemblyRoleMapping.ProductCategoryMatchesRole("Motherboard", AssemblyRoleKind.Motherboard),
                Is.True);
            Assert.That(
                AssemblyRoleMapping.ProductCategoryMatchesRole("Motherboards", AssemblyRoleKind.Motherboard),
                Is.True);
            Assert.That(
                AssemblyRoleMapping.ProductCategoryMatchesRole("GPU", AssemblyRoleKind.Motherboard),
                Is.False);
        }

        [Test]
        public void ProductCategoryMatchesRole_Custom_always_true()
        {
            Assert.That(
                AssemblyRoleMapping.ProductCategoryMatchesRole("Anything", AssemblyRoleKind.Custom),
                Is.True);
        }

        [Test]
        public void FromStoredRole_recognizes_Cooler()
        {
            Assert.That(AssemblyRoleMapping.FromStoredRole("Cooler"), Is.EqualTo(AssemblyRoleKind.Cooler));
        }

        [Test]
        public void ToPersistedRole_Cooler_round_trips()
        {
            Assert.That(
                AssemblyRoleMapping.ToPersistedRole(AssemblyRoleKind.Cooler, null),
                Is.EqualTo("Cooler"));
        }
    }
}
