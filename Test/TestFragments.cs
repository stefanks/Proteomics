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

using NUnit.Framework;
using Proteomics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Test
{
    [TestFixture]
    public sealed class TestFragments
    {
        private Peptide _mockPeptideEveryAminoAcid;

        [SetUp]
        public void SetUp()
        {
            _mockPeptideEveryAminoAcid = new Peptide("ACDEFGHIKLMNPQRSTVWY");
        }

        [Test]
        public void FragmentNumberToLow()
        {
            Assert.Throws<IndexOutOfRangeException>(() => _mockPeptideEveryAminoAcid.Fragment(FragmentTypes.b, 0).ToList());
        }

        [Test]
        public void FragmentNumberToHigh()
        {
            Assert.Throws<IndexOutOfRangeException>(() => _mockPeptideEveryAminoAcid.Fragment(FragmentTypes.b, 25).ToList());
        }

        [Test]
        public void FragmentName()
        {
            Fragment fragment = _mockPeptideEveryAminoAcid.Fragment(FragmentTypes.a, 1).ToArray()[0];

            Assert.AreEqual("a1", fragment.ToString());
        }

        [Test]
        public void FragmentAllBIons()
        {
            List<Fragment> fragments = _mockPeptideEveryAminoAcid.Fragment(FragmentTypes.b).ToList();

            Assert.AreEqual(19, fragments.Count);
        }

        [Test]
        public void FragmentModifications()
        {
            _mockPeptideEveryAminoAcid.AddModification(new Modification(1, "mod1", ModificationSites.C));
            _mockPeptideEveryAminoAcid.AddModification(new Modification(2, "mod2", ModificationSites.D));
            _mockPeptideEveryAminoAcid.AddModification(new Modification(3, "mod3", ModificationSites.A));
            _mockPeptideEveryAminoAcid.AddModification(new Modification(4, "mod4", ModificationSites.Y));
            Fragment fragment = _mockPeptideEveryAminoAcid.Fragment(FragmentTypes.b, 1).First();
            Fragment fragmentEnd = _mockPeptideEveryAminoAcid.Fragment(FragmentTypes.y, 1).Last();

            Assert.IsTrue(fragment.GetModifications().SequenceEqual(new List<Modification>() { new Modification(3, "mod3", ModificationSites.A) }));
    
            Assert.IsTrue(fragmentEnd.GetModifications().SequenceEqual(new List<Modification>() { new Modification(4, "mod4", ModificationSites.Y) }));
        }

    }
}