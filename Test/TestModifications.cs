﻿// Copyright 2012, 2013, 2014 Derek J. Bailey
// Modified work copyright 2016 Stefan Solntsev
// 
// This file (TestFragments.cs) is part of Proteomics.
// 
// Proteomics is free software: you can redistribute it and/or modify it
// under the terms of the GNU Lesser General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Proteomics is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
// FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public
// License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with Proteomics. If not, see <http://www.gnu.org/licenses/>.

using Chemistry;
using NUnit.Framework;
using Proteomics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Test
{
    [TestFixture]
    public sealed class TestModifications
    {

        [Test]
        public void NameAndSites()
        {
            // Empty modification, has no name and by default has Sites = ModificationSites.Any
            Modification a = new Modification();
            Modification b = new Modification(a);
            Assert.AreEqual(" (Any)", b.NameAndSites);
        }

        [Test]
        public void ModificationEquality()
        {
            // Empty modification, has no name and by default has Sites = ModificationSites.Any
            Modification a = new Modification();
            Modification b = new Modification();
            Modification c = new Modification(0, "c");
            Modification d = new Modification(0, "", ModificationSites.E);
            Assert.IsTrue(a.Equals(b));
            Assert.IsFalse(a.Equals(c));
            Assert.IsFalse(a.Equals(d));
        }

        [Test]
        public void ModificationSitesTest()
        {
            // Empty modification, has no name and by default has Sites = ModificationSites.Any
            var a = ModificationSites.A | ModificationSites.E;
            var b = a.Set('C');
            Assert.AreEqual(ModificationSites.A | ModificationSites.E | ModificationSites.C, b);

        }

        [Test]
        public void ModificationSitesTest2()
        {
            // Empty modification, has no name and by default has Sites = ModificationSites.Any
            var a = ModificationSites.A | ModificationSites.E;
            var b = a.Set(AminoAcid.GetResidue("D"));
            Assert.AreEqual(ModificationSites.A | ModificationSites.E | ModificationSites.D, b);
        }

        [Test]
        public void Sites()
        {
            // Empty modification, has no name and by default has Sites = ModificationSites.Any
            var a = ModificationSites.A | ModificationSites.C | ModificationSites.E;
            Assert.IsTrue(a.ContainsSite(ModificationSites.E));

            Assert.IsTrue(a.ContainsSite(ModificationSites.A | ModificationSites.C));
            Assert.IsFalse(a.ContainsSite(ModificationSites.N));
            Assert.IsFalse(a.ContainsSite(ModificationSites.N | ModificationSites.C));
            var b = a.GetActiveSites();
            Assert.IsTrue(b.Count() == 3);
        }

        [Test]
        public void ModificationCollectionTest()
        {
            ModificationCollection a = new ModificationCollection(new Modification(1, "Mod1"), new Modification(2, "Mod2"));
            Assert.AreEqual("Mod1 | Mod2", a.ToString());
            a.Add(new Modification(3, "Mod3"));
            Assert.AreEqual("Mod1 | Mod2 | Mod3", a.ToString());
            Assert.IsTrue(a.Contains(new Modification(2, "Mod2")));
            IHasMass[] myArray = new IHasMass[4];
            a.CopyTo(myArray, 1);
            Assert.AreEqual(3, myArray.Sum(b => b == null ? 0 : 1));
            Assert.AreEqual(3, a.Count());
            Assert.IsFalse(a.IsReadOnly);
            a.Remove(new Modification(2, "Mod2"));
            Assert.AreEqual("Mod1 | Mod3", a.ToString());
            double ok = 0;
            foreach (var b in a)
                ok += b.MonoisotopicMass;
            Assert.AreEqual(4, ok);
            a.Clear();
            Assert.AreEqual("", a.ToString());
        }

        [Test]
        public void ModificationWithMultiplePossibilitiesTest()
        {
            var m = new ModificationWithMultiplePossibilities("My Iso Mod", ModificationSites.E);
            m.AddModification(new Modification(1, "My Mod1a", ModificationSites.E));
            m.AddModification(new Modification(2, "My Mod2b", ModificationSites.E));
            Assert.AreEqual(2, m.Count);
            Assert.AreEqual("My Mod2b", m[1].Name);
            Assert.Throws<ArgumentException>(()=> { m.AddModification(new Modification(1, "gg", ModificationSites.R)); }, "Unable to add a modification with sites other than ModificationSites.E");
            Assert.IsTrue(m.Contains(new Modification(2, "My Mod2b", ModificationSites.E)));

        }

        [Test]
        public void ModificationSitesTest55()
        {
            Assert.IsTrue( ModificationSites.E.ContainsSite(ModificationSites.Any));
            Assert.IsFalse(ModificationSites.E.ContainsSite(ModificationSites.None));
            Assert.IsTrue(ModificationSites.None.ContainsSite(ModificationSites.None));
        }



        [Test]
        public void ChemicalFormulaModificaiton()
        {
            ChemicalFormulaModification a = new ChemicalFormulaModification("OH");
            ChemicalFormulaModification b = new ChemicalFormulaModification(a);
            Assert.AreEqual(a, b);
        }
    }
}