namespace HardwareStore.Tests.Unit
{
    using HardwareStore.Core.ViewModels.Admin;
    using HardwareStore.Infrastructure.Models.Enums;
    using NUnit.Framework;

    public class AssemblyRoleMappingTests
    {
        [Test]
        public void ProductCategoryMatchesRole_standard_slot_must_match_category()
        {
            Assert.Multiple(() =>
            {
                Assert.That(
                    AssemblyRoleMapping.ProductCategoryMatchesRole(CategoryAssemblySlot.Motherboard, AssemblyRoleKind.Motherboard),
                    Is.True);
                Assert.That(
                    AssemblyRoleMapping.ProductCategoryMatchesRole(CategoryAssemblySlot.Gpu, AssemblyRoleKind.Motherboard),
                    Is.False);
            });
        }

        [Test]
        public void ProductCategoryMatchesRole_Custom_always_true()
        {
            Assert.That(
                AssemblyRoleMapping.ProductCategoryMatchesRole(CategoryAssemblySlot.None, AssemblyRoleKind.Custom),
                Is.True);
        }

        [Test]
        public void FromStoredRole_and_ToPersistedRole_roundtrip_Cooler()
        {
            Assert.That(AssemblyRoleMapping.FromStoredRole("Cooler"), Is.EqualTo(AssemblyRoleKind.Cooler));
            Assert.That(
                AssemblyRoleMapping.ToPersistedRole(AssemblyRoleKind.Cooler, null),
                Is.EqualTo("Cooler"));
        }
    }
}
